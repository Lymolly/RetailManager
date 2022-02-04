using System.Threading.Tasks;
using RetailManagerDesktopUI.Library.Models;

namespace RetailManagerDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel model);
    }
}