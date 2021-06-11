using CryptoPrice.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace CryptoPrice.Api
{
    public class PriceCallback : IPriceCallback
    {
        private readonly IPriceClient _proxy;

        public PriceCallback(IPriceClient proxy)
        {
            _proxy = proxy;
        }

        public async Task OnPriceUpdate(Price price)
        {
            await _proxy?.UpdatePrice(price);
        }
    }
}
