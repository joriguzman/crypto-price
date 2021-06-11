using System.Threading.Tasks;

namespace CryptoPrice.Api
{
    public interface IPriceCallback
    {
        Task OnPriceUpdate(Price price);
    }
}
