// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.ExceptionHandling.Rpc;
using BitcoinLib.RPC.RequestResponse;
using BitcoinLib.RPC.Specifications;
using BitcoinLib.Services;
using BitcoinLib.Services.Coins.Base;
using Newtonsoft.Json;

namespace BitcoinLib.RPC.Connector
{
    public sealed class AsyncRpcConnector : IAsyncRpcConnector
    {
        private readonly ICoinService _coinService;
        private readonly Lazy<HttpClient> _httpClient;
        private volatile bool _disposed;

        public AsyncRpcConnector(ICoinService coinService)
        {
            _coinService = coinService;
            _httpClient = new Lazy<HttpClient>(() =>
            {
	            var httpClient = new HttpClient(new RpcClientHandler(coinService.Parameters));

	            var authInfo = $"{coinService.Parameters.RpcUsername}:{coinService.Parameters.RpcPassword}";
	            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

	            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);

	            return httpClient;
            });
        }

        public async Task<T> MakeRequestAsync<T>(RpcMethods rpcMethod, CancellationToken cancellationToken, params object[] parameters)
        {
	        if (_disposed)
		        throw new ObjectDisposedException(nameof(_httpClient));

	        var jsonRpcRequest = new JsonRpcRequest(1, rpcMethod.ToString(), parameters);

            using (var request = new HttpRequestMessage(HttpMethod.Post, _coinService.Parameters.SelectedDaemonUrl))
            {
                var requestContent = jsonRpcRequest.GetBytes();

                request.Content = new ByteArrayContent(requestContent);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-rpc");
                request.Content.Headers.ContentLength = requestContent.Length;

                try
                {
	                using (var response = await _httpClient.Value.SendAsync(request,
		                HttpCompletionOption.ResponseContentRead, cancellationToken))
	                {
		                if (response.Content == null)
			                throw new RpcException(
				                "The RPC request was either not understood by the server or there was a problem executing the request");

		                var responseContent = await response.Content.ReadAsStringAsync();

		                if (!response.IsSuccessStatusCode)
		                {
			                switch (response.StatusCode)
			                {
				                case HttpStatusCode.InternalServerError:
					                try
					                {
						                var jsonRpcResponseObject =
							                JsonConvert.DeserializeObject<JsonRpcResponse<object>>(responseContent);

						                var internalServerErrorException =
							                new RpcInternalServerErrorException(jsonRpcResponseObject.Error.Message)
							                {
								                RpcErrorCode = jsonRpcResponseObject.Error.Code
							                };

						                throw internalServerErrorException;
					                }
					                catch (JsonException)
					                {
						                throw new RpcException(responseContent);
					                }
				                default:
					                throw new RpcException(
						                "The RPC request was either not understood by the server or there was a problem executing the request");
			                }
		                }

		                var rpcResponse = JsonConvert.DeserializeObject<JsonRpcResponse<T>>(responseContent);
		                return rpcResponse.Result;
	                }
                }
                catch (HttpRequestException httpRequestException)
                {
	                throw new RpcException(
		                "The RPC request was either not understood by the server or there was a problem executing the request",
		                httpRequestException);
                }
                catch (JsonException jsonException)
                {
	                throw new RpcResponseDeserializationException(
		                "There was a problem deserializing the response from the wallet", jsonException);
                }
                catch (TimeoutException)
                {
	                throw new RpcRequestTimeoutException("The operation has timed out");
                }
                catch (RpcInternalServerErrorException)
                {
	                throw;
                }
                catch (RpcException)
                {
	                throw;
                }
                catch (Exception exception)
                {
                    var queryParameters = jsonRpcRequest.Parameters.Cast<string>().Aggregate(string.Empty, (current, parameter) => current + (parameter + " "));
                    throw new Exception($"A problem was encountered while calling MakeRpcRequest() for: {jsonRpcRequest.Method} with parameters: {queryParameters}. \nException: {exception.Message}");
                }
            }
        }

        public void Dispose()
        {
	        if (_disposed)
		        return;

	        _disposed = true;

	        if (!_httpClient.IsValueCreated)
		        return;

	        _httpClient.Value.Dispose();
        }
    }
}