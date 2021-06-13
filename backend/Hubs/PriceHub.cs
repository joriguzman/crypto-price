using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace CryptoPrice.Api.Hubs
{
    public class PriceHub : Hub<IPriceClient>
    {
        private readonly PriceDataProvider _dataProvider;

        public PriceHub(PriceDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task StreamPrices()
        {
            await _dataProvider.StartSubscriptions();
        }

        public async Task StopStreaming()
        {
            await _dataProvider.StopSubscriptions();
        }

        public override Task OnConnectedAsync()
        {
            var clients = Clients.All;
            _dataProvider.OnPriceUpdate = price =>
            {
                clients.UpdatePrice(price);
            };
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var clients = Clients.All;
            _dataProvider.OnPriceUpdate = price =>
            {
                clients.UpdatePrice(price);
            };
            return base.OnDisconnectedAsync(exception);
        }
    }
}
