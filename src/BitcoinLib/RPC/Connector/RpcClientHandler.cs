using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BitcoinLib.Services
{
    internal class RpcClientHandler : DelegatingHandler
    {
        private readonly CoinService.CoinParameters _coinParameters;

        public RpcClientHandler(CoinService.CoinParameters coinParameters)
        {
            _coinParameters = coinParameters;
            InnerHandler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(_coinParameters.RpcUsername, _coinParameters.RpcPassword)
            };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_coinParameters.RpcRequestTimeoutInSeconds <= 0)
                return await base.SendAsync(request, cancellationToken);

            using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(_coinParameters.RpcRequestTimeoutInSeconds)))
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token))
            {
                try
                {
                    return await base.SendAsync(request, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    if (timeoutCts.IsCancellationRequested)
                        throw new TimeoutException();

                    throw;
                }
            }
        }
    }
}
