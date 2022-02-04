using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RetailManagerDesktopUI.Library.Models;

namespace RetailManagerDesktopUI.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {

        private IApiHelper _apiHelper;
        public SaleEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel model)
        {
            using (var response = await _apiHelper.ApiClient.PostAsJsonAsync<SaleModel>("api/Sale",model))
            {
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
