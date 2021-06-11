using System.Threading.Tasks;

namespace CryptoPrice.Api.Hubs
{
    public interface IPriceClient
    {
        Task UpdatePrice(Price price);
    }
}
