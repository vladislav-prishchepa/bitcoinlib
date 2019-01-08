// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Responses;

namespace BitcoinLib.Services.RpcServices.RpcExtenderService
{
    public interface IAsyncRpcExtenderService
    {
        Task<decimal> GetAddressBalanceAsync(string inWalletAddress, int minConf = 0, bool validateAddressBeforeProcessing = true, CancellationToken cancellationToken = default);
        Task<decimal> GetMinimumNonZeroTransactionFeeEstimateAsync(short numberOfInputs = 1, short numberOfOutputs = 1, CancellationToken cancellationToken = default);
        Task<Dictionary<string, string>> GetMyPublicAndPrivateKeyPairsAsync(CancellationToken cancellationToken = default);
        Task<DecodeRawTransactionResponse> GetPublicTransactionAsync(string txId, CancellationToken cancellationToken = default);
        Task<decimal> GetTransactionFeeAsync(CreateRawTransactionRequest createRawTransactionRequest, bool checkIfTransactionQualifiesForFreeRelay = true, bool enforceMinimumTransactionFeePolicy = true, CancellationToken cancellationToken = default);
        Task<decimal> GetTransactionPriorityAsync(CreateRawTransactionRequest transaction, CancellationToken cancellationToken = default);
        Task<string> GetTransactionSenderAddressAsync(string txId, CancellationToken cancellationToken = default);
        Task<GetRawTransactionResponse> GetRawTxFromImmutableTxIdAsync(string rigidTxId, int listTransactionsCount = int.MaxValue, int listTransactionsFrom = 0, bool getRawTransactionVersbose = true, bool rigidTxIdIsSha256 = false, CancellationToken cancellationToken = default);
        Task<string> GetImmutableTxIdAsync(string txId, bool getSha256Hash = false, CancellationToken cancellationToken = default);
        Task<bool> IsInWalletTransactionAsync(string txId, CancellationToken cancellationToken = default);
        Task<bool> IsTransactionFreeAsync(CreateRawTransactionRequest createRawTransactionRequest, CancellationToken cancellationToken = default);
        Task<bool> IsWalletEncryptedAsync(CancellationToken cancellationToken = default);
    }
}