using System.Net.Http;
using RetailManagerDesktopUI.Models;
using System.Threading.Tasks;

namespace RetailManagerDesktopUI.Library.Api
{
    public interface IApiHelper
    {
        Task<AuthenticateUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token); 
        HttpClient ApiClient { get; set; }
    }
}