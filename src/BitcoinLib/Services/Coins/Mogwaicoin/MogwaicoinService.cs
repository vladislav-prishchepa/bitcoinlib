using BitcoinLib.CoinParameters.Mogwaicoin;
using BitcoinLib.RPC.Specifications;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BitcoinLib.Services.Coins.Mogwaicoin
{
    public class MogwaicoinService : CoinService, IMogwaicoinService
    {
        public MogwaicoinService(bool useTestnet = false) : base(useTestnet)
        {
        }

        public MogwaicoinService(string daemonUrl, string rpcUsername, string rpcPassword, string walletPassword)
            : base(daemonUrl, rpcUsername, rpcPassword, walletPassword)
        {
        }

        public MogwaicoinService(string daemonUrl, string rpcUsername, string rpcPassword, string walletPassword,
            short rpcRequestTimeoutInSeconds)
            : base(daemonUrl, rpcUsername, rpcPassword, walletPassword, rpcRequestTimeoutInSeconds)
        {
        }

        /// <inheritdoc />
        public Task<string> SendToAddressAsync(string mogwaiAddress, decimal amount, string comment = null, string commentTo = null,
            bool subtractFeeFromAmount = false, bool useInstantSend = false, bool usePrivateSend = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _rpcConnector.MakeRequestAsync<string>(RpcMethods.sendtoaddress, cancellationToken, mogwaiAddress, amount, comment, commentTo,
                subtractFeeFromAmount, useInstantSend, usePrivateSend);
        }

        public Task<MirrorAddressResponse> MirrorAddressAsync(string mogwaiAddress, CancellationToken cancellationToken)
        {
            return _rpcConnector.MakeRequestAsync<MirrorAddressResponse>(RpcMethods.mirroraddress, cancellationToken, mogwaiAddress);
        }

        public Task<List<ListMirrorTransactionsResponse>> ListMirrorTransactionsAsync(string mogwaiAddress, CancellationToken cancellationToken)
        {
            return _rpcConnector.MakeRequestAsync<List<ListMirrorTransactionsResponse>>(RpcMethods.listmirrtransactions, cancellationToken, mogwaiAddress);
        }

        public MogwaicoinConstants.Constants Constants => MogwaicoinConstants.Constants.Instance;
    }

    public class MirrorAddressResponse
    {
        public bool IsValid { get; set; }
        public string Address { get; set; }
        public bool IsMine { get; set; }
        public bool IsScript { get; set; }
        public string PubKey { get; set; }
        public bool IsCompressed { get; set; }
        public string MirKey { get; set; }
        public bool MirKeyValid { get; set; }
        public bool IsMirAddrValid { get; set; }
        public string MirAddress { get; set; }
    }

    public class ListMirrorTransactionsResponse
    {
        public string Address { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public int Vout { get; set; }
        public decimal Fee { get; set; }
        public int Confirmations { get; set; }
        public bool Instantlock { get; set; }
        public bool Generated { get; set; }
        public bool Trusted { get; set; }
        public string BlockHash { get; set; }
        public double BlockIndex { get; set; }
        public double BlockTime { get; set; }
        public string TxId { get; set; }
        public double Time { get; set; }
        public double TimeReceived { get; set; }
        public bool Abandoned { get; set; }
    }
}