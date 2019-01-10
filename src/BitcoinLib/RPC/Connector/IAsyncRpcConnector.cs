// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System;
using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.RPC.Specifications;

namespace BitcoinLib.RPC.Connector
{
    public interface IAsyncRpcConnector : IDisposable
    {
		/// <exception cref="ObjectDisposedException"></exception>
        Task<T> MakeRequestAsync<T>(RpcMethods rpcMethod, CancellationToken cancellationToken, params object[] parameters);
    }
}