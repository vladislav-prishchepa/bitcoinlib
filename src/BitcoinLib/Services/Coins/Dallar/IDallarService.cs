// Copyright (c) 2014 - 2016 George Kimionis
// See the accompanying file LICENSE for the Software License Aggrement

using System.Threading;
using System.Threading.Tasks;
using BitcoinLib.CoinParameters.Dallar;
using BitcoinLib.Services.Coins.Base;

namespace BitcoinLib.Services.Coins.Dallar
{
    public interface IDallarService : ICoinService, IDallarConstants
    {
        Task<decimal> GetEstimateFeeForSendToAddressAsync(string Address, decimal Amount, CancellationToken cancellationToken);
    }
}