using System.Collections.Generic;
using System.Threading.Tasks;
using RetailManagerDesktopUI.Library.Models;

namespace RetailManagerDesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}