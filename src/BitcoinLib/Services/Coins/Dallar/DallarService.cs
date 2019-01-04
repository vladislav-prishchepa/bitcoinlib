// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.CoinParameters.Dallar;
using BitcoinLib.Requests.CreateRawTransaction;
using BitcoinLib.Responses;
using BitcoinLib.RPC.Specifications;

namespace BitcoinLib.Services.Coins.Dallar
{
    public class DallarService : CoinService, IDallarService
    {
        public DallarService(bool useTestnet = false) : base(useTestnet)
        {
        }

        public DallarService(string daemonUrl, string rpcUsername, string rpcPassword, string walletPassword = null)
            : base(daemonUrl, rpcUsername, rpcPassword, walletPassword)
        {
        }

        public DallarService(string daemonUrl, string rpcUsername, string rpcPassword, string walletPassword, short rpcRequestTimeoutInSeconds)
            : base(daemonUrl, rpcUsername, rpcPassword, walletPassword, rpcRequestTimeoutInSeconds)
        {
        }

        public DallarConstants.Constants Constants => DallarConstants.Constants.Instance;

        public async Task<decimal> GetEstimateFeeForSendToAddressAsync(string Address, decimal Amount, CancellationToken cancellationToken)
        {
            var txRequest = new CreateRawTransactionRequest();
            txRequest.AddOutput(Address, Amount);
            var rawTransaction = await CreateRawTransactionAsync(txRequest, cancellationToken);
            var fundTransaction = await GetFundRawTransactionAsync(rawTransaction, cancellationToken);
            return fundTransaction.Fee;
        }
    }
}