// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Responses;

namespace BitcoinLib.Services.RpcServices.RpcExtenderService
{
    public interface IRpcExtenderService
    {
        Task<decimal> GetAddressBalanceAsync(string inWalletAddress, int minConf, bool validateAddressBeforeProcessing, CancellationToken cancellationToken);
        Task<decimal> GetMinimumNonZeroTransactionFeeEstimateAsync(short numberOfInputs, short numberOfOutputs, CancellationToken cancellationToken);
        Task<Dictionary<string, string>> GetMyPublicAndPrivateKeyPairsAsync(CancellationToken cancellationToken);
        Task<DecodeRawTransactionResponse> GetPublicTransactionAsync(string txId, CancellationToken cancellationToken);
        Task<decimal> GetTransactionFeeAsync(CreateRawTransactionRequest createRawTransactionRequest, bool checkIfTransactionQualifiesForFreeRelay, bool enforceMinimumTransactionFeePolicy, CancellationToken cancellationToken);
        Task<decimal> GetTransactionPriorityAsync(CreateRawTransactionRequest transaction, CancellationToken cancellationToken);
        decimal GetTransactionPriority(IList<ListUnspentResponse> transactionInputs, int numberOfOutputs);
        Task<string> GetTransactionSenderAddressAsync(string txId, CancellationToken cancellationToken);
        int GetTransactionSizeInBytes(CreateRawTransactionRequest createRawTransactionRequest);
        int GetTransactionSizeInBytes(int numberOfInputs, int numberOfOutputs);
        Task<GetRawTransactionResponse> GetRawTxFromImmutableTxIdAsync(string rigidTxId, int listTransactionsCount, int listTransactionsFrom, bool getRawTransactionVersbose, bool rigidTxIdIsSha256, CancellationToken cancellationToken);
        Task<string> GetImmutableTxIdAsync(string txId, bool getSha256Hash, CancellationToken cancellationToken);
        Task<bool> IsInWalletTransactionAsync(string txId, CancellationToken cancellationToken);
        Task<bool> IsTransactionFreeAsync(CreateRawTransactionRequest createRawTransactionRequest, CancellationToken cancellationToken);
        bool IsTransactionFree(IList<ListUnspentResponse> transactionInputs, int numberOfOutputs, decimal minimumAmountAmongOutputs);
        Task<bool> IsWalletEncryptedAsync(CancellationToken cancellationToken);
    }
}