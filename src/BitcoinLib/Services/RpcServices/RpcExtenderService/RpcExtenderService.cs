// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.Auxiliary;
using BitcoinLib.ExceptionHandling.RpcExtenderService;
using BitcoinLib.ExtensionMethods;
using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Responses;
using BitcoinLib.RPC.Specifications;
using BitcoinLib.Services.Coins.Base;

namespace BitcoinLib.Services
{
    public partial class CoinService
    {
        //  Note: This will return funky results if the address in question along with its private key have been used to create a multisig address with unspent funds
        public async Task<decimal> GetAddressBalanceAsync(string inWalletAddress, int minConf, bool validateAddressBeforeProcessing, CancellationToken cancellationToken)
        {
            if (validateAddressBeforeProcessing)
            {
                var validateAddressResponse = await ValidateAddressAsync(inWalletAddress, cancellationToken).ConfigureAwait(false);

                if (!validateAddressResponse.IsValid)
                {
                    throw new GetAddressBalanceException($"Address {inWalletAddress} is invalid!");
                }

                if (!validateAddressResponse.IsMine)
                {
                    throw new GetAddressBalanceException($"Address {inWalletAddress} is not an in-wallet address!");
                }
            }

            var listUnspentResponses = await ListUnspentAsync(
                minConf,
                9999999,
                new List<string>
                {
                    inWalletAddress
                },
                cancellationToken)
                .ConfigureAwait(false);

            return listUnspentResponses.Any() ? listUnspentResponses.Sum(x => x.Amount) : 0;
        }

        public async Task<string> GetImmutableTxIdAsync(string txId, bool getSha256Hash, CancellationToken cancellationToken)
        {
            var response = await GetRawTransactionAsync(txId, 1, cancellationToken).ConfigureAwait(false);
            var text = response.Vin.First().TxId + "|" + response.Vin.First().Vout + "|" + response.Vout.First().Value;
            return getSha256Hash ? Hashing.GetSha256(text) : text;
        }

        //  Get a rough estimate on fees for non-free txs, depending on the total number of tx inputs and outputs
        [Obsolete("Please don't use this method to calculate tx fees, its purpose is to provide a rough estimate only")]
        public Task<decimal> GetMinimumNonZeroTransactionFeeEstimateAsync(short numberOfInputs, short numberOfOutputs, CancellationToken cancellationToken)
        {
            var rawTransactionRequest = new CreateRawTransactionRequest(new List<CreateRawTransactionInput>(numberOfInputs), new Dictionary<string, decimal>(numberOfOutputs));

            for (short i = 0; i < numberOfInputs; i++)
            {
                rawTransactionRequest.AddInput(new CreateRawTransactionInput
                {
                    TxId = "dummyTxId" + i.ToString(CultureInfo.InvariantCulture), Vout = i
                });
            }

            for (short i = 0; i < numberOfOutputs; i++)
            {
                rawTransactionRequest.AddOutput(new CreateRawTransactionOutput
                {
                    Address = "dummyAddress" + i.ToString(CultureInfo.InvariantCulture), Amount = i + 1
                });
            }

            return GetTransactionFeeAsync(rawTransactionRequest, false, true, cancellationToken);
        }

        public async Task<Dictionary<string, string>> GetMyPublicAndPrivateKeyPairsAsync(CancellationToken cancellationToken)
        {
            const short secondsToUnlockTheWallet = 30;
            var keyPairs = new Dictionary<string, string>();
            await WalletPassphraseAsync(Parameters.WalletPassword, secondsToUnlockTheWallet, cancellationToken).ConfigureAwait(false);
            var myAddresses = await ListReceivedByAddressAsync(0, true, null, cancellationToken).ConfigureAwait(false);

            foreach (var listReceivedByAddressResponse in myAddresses)
            {
                var validateAddressResponse = await ValidateAddressAsync(listReceivedByAddressResponse.Address, cancellationToken).ConfigureAwait(false);

                if (validateAddressResponse.IsMine && validateAddressResponse.IsValid && !validateAddressResponse.IsScript)
                {
                    var privateKey = await DumpPrivKeyAsync(listReceivedByAddressResponse.Address, cancellationToken).ConfigureAwait(false);
                    keyPairs.Add(validateAddressResponse.PubKey, privateKey);
                }
            }

            await WalletLockAsync(cancellationToken).ConfigureAwait(false);
            return keyPairs;
        }

        //  Note: As RPC's gettransaction works only for in-wallet transactions this had to be extended so it will work for every single transaction.
        public async Task<DecodeRawTransactionResponse> GetPublicTransactionAsync(string txId, CancellationToken cancellationToken)
        {
            var rawTransaction = (await GetRawTransactionAsync(txId, 0, cancellationToken)).Hex;
            return await DecodeRawTransactionAsync(rawTransaction, cancellationToken).ConfigureAwait(false);
        }

        [Obsolete("Please use EstimateFee() instead. You can however keep on using this method until the network fully adjusts to the new rules on fee calculation")]
        public async Task<decimal> GetTransactionFeeAsync(CreateRawTransactionRequest transaction, bool checkIfTransactionQualifiesForFreeRelay, bool enforceMinimumTransactionFeePolicy, CancellationToken cancellationToken)
        {
            if (checkIfTransactionQualifiesForFreeRelay && await IsTransactionFreeAsync(transaction, cancellationToken).ConfigureAwait(false))
            {
                return 0;
            }

            decimal transactionSizeInBytes = GetTransactionSizeInBytes(transaction);
            var transactionFee = ((transactionSizeInBytes / Parameters.FreeTransactionMaximumSizeInBytes) + (transactionSizeInBytes % Parameters.FreeTransactionMaximumSizeInBytes == 0 ? 0 : 1)) * Parameters.FeePerThousandBytesInCoins;

            if (transactionFee.GetNumberOfDecimalPlaces() > Parameters.CoinsPerBaseUnit.GetNumberOfDecimalPlaces())
            {
                transactionFee = decimal.Round(transactionFee, Parameters.CoinsPerBaseUnit.GetNumberOfDecimalPlaces(), MidpointRounding.AwayFromZero);
            }

            if (enforceMinimumTransactionFeePolicy && Parameters.MinimumTransactionFeeInCoins != 0 && transactionFee < Parameters.MinimumTransactionFeeInCoins)
            {
                transactionFee = Parameters.MinimumTransactionFeeInCoins;
            }

            return transactionFee;
        }

        public async Task<GetRawTransactionResponse> GetRawTxFromImmutableTxIdAsync(string rigidTxId, int listTransactionsCount, int listTransactionsFrom, bool getRawTransactionVersbose, bool rigidTxIdIsSha256, CancellationToken cancellationToken)
        {
            var allTransactions = await ListTransactionsAsync("*", listTransactionsCount, listTransactionsFrom, null, cancellationToken).ConfigureAwait(false);

            var tcs = new TaskCompletionSource<GetRawTransactionResponse>();

            using (var cancelPendingCts = new CancellationTokenSource())
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cancelPendingCts.Token))
            {
                var tasks = allTransactions
                    .Select(rawTransaction => GetImmutableTxIdAsync(rawTransaction.TxId, rigidTxIdIsSha256, cts.Token)
                        .ContinueWith(
                            async task =>
                            {
                                if (rigidTxId == task.Result)
                                {
                                    var transaction = await GetRawTransactionAsync(rawTransaction.TxId, getRawTransactionVersbose ? 1 : 0, cancellationToken).ConfigureAwait(false);
                                    tcs.TrySetResult(transaction);
                                }
                            },
                            TaskContinuationOptions.OnlyOnRanToCompletion));

                try
                {
                    await Task.WhenAny(Task.WhenAll(tasks), tcs.Task).ConfigureAwait(false);

                    tcs.TrySetResult(null);

                    cancelPendingCts.Cancel();

                    return tcs.Task.Result;
                }
                catch (OperationCanceledException)
                {
                    if (cancelPendingCts.IsCancellationRequested)
                        return tcs.Task.Result;

                    throw;
                }
            }
        }

        public async Task<decimal> GetTransactionPriorityAsync(CreateRawTransactionRequest transaction, CancellationToken cancellationToken)
        {
            if (transaction.Inputs.Count == 0)
            {
                return 0;
            }

            var unspentInputs = await ListUnspentAsync(0, 99999, null, cancellationToken).ConfigureAwait(false);
            var sumOfInputsValueInBaseUnitsMultipliedByTheirAge = transaction.Inputs
                .Select(input => unspentInputs.First(x => x.TxId == input.TxId))
                .Select(unspentResponse => unspentResponse.Amount * Parameters.OneCoinInBaseUnits * unspentResponse.Confirmations)
                .Sum();

            return sumOfInputsValueInBaseUnitsMultipliedByTheirAge / GetTransactionSizeInBytes(transaction);
        }

        public decimal GetTransactionPriority(IList<ListUnspentResponse> transactionInputs, int numberOfOutputs)
        {
            if (transactionInputs.Count == 0)
            {
                return 0;
            }

            return transactionInputs.Sum(input => input.Amount * Parameters.OneCoinInBaseUnits * input.Confirmations) / GetTransactionSizeInBytes(transactionInputs.Count, numberOfOutputs);
        }

        //  Note: Be careful when using GetTransactionSenderAddress() as it just gives you an address owned by someone who previously controlled the transaction's outputs
        //  which might not actually be the sender (e.g. for e-wallets) and who may not intend to receive anything there in the first place. 
        [Obsolete("Please don't use this method in production enviroment, it's for testing purposes only")]
        public async Task<string> GetTransactionSenderAddressAsync(string txId, CancellationToken cancellationToken)
        {
            var rawTransaction = (await GetRawTransactionAsync(txId, 0, cancellationToken).ConfigureAwait(false)).Hex;
            var decodedRawTransaction = await DecodeRawTransactionAsync(rawTransaction, cancellationToken).ConfigureAwait(false);
            var transactionInputs = decodedRawTransaction.Vin;
            var rawTransactionHex = (await GetRawTransactionAsync(transactionInputs[0].TxId, 0, cancellationToken).ConfigureAwait(false)).Hex;
            var inputDecodedRawTransaction = await DecodeRawTransactionAsync(rawTransactionHex, cancellationToken).ConfigureAwait(false);
            var vouts = inputDecodedRawTransaction.Vout;
            return vouts[0].ScriptPubKey.Addresses[0];
        }

        public int GetTransactionSizeInBytes(CreateRawTransactionRequest transaction)
        {
            return GetTransactionSizeInBytes(transaction.Inputs.Count, transaction.Outputs.Count);
        }

        public int GetTransactionSizeInBytes(int numberOfInputs, int numberOfOutputs)
        {
            return numberOfInputs * Parameters.TransactionSizeBytesContributedByEachInput
                   + numberOfOutputs * Parameters.TransactionSizeBytesContributedByEachOutput
                   + Parameters.TransactionSizeFixedExtraSizeInBytes
                   + numberOfInputs;
        }

        public async Task<bool> IsInWalletTransactionAsync(string txId, CancellationToken cancellationToken)
        {
            //  Note: This might not be efficient if iterated, consider caching ListTransactions' results.
            return (await ListTransactionsAsync(null, int.MaxValue, 0, null, cancellationToken).ConfigureAwait(false)).Any(listTransactionsResponse => listTransactionsResponse.TxId == txId);
        }

        public async Task<bool> IsTransactionFreeAsync(CreateRawTransactionRequest transaction, CancellationToken cancellationToken)
        {
            return transaction.Outputs.Any(x => x.Value < Parameters.FreeTransactionMinimumOutputAmountInCoins)
                   && GetTransactionSizeInBytes(transaction) < Parameters.FreeTransactionMaximumSizeInBytes
                   && await GetTransactionPriorityAsync(transaction, cancellationToken).ConfigureAwait(false) > Parameters.FreeTransactionMinimumPriority;
        }

        public bool IsTransactionFree(IList<ListUnspentResponse> transactionInputs, int numberOfOutputs, decimal minimumAmountAmongOutputs)
        {
            return minimumAmountAmongOutputs < Parameters.FreeTransactionMinimumOutputAmountInCoins
                   && GetTransactionSizeInBytes(transactionInputs.Count, numberOfOutputs) < Parameters.FreeTransactionMaximumSizeInBytes
                   && GetTransactionPriority(transactionInputs, numberOfOutputs) > Parameters.FreeTransactionMinimumPriority;
        }

        public async Task<bool> IsWalletEncryptedAsync(CancellationToken cancellationToken)
        {
            return !(await HelpAsync(RpcMethods.walletlock.ToString(), cancellationToken).ConfigureAwait(false)).Contains("unknown command");
        }
    }
}