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
            _dataProvider.Callback = new PriceCallback(Clients.All);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _dataProvider.Callback = new PriceCallback(Clients.All);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
