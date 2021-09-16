using System.Threading.Tasks;
using RetailManagerDesktopUI.Models;

namespace RetailManagerDesktopUI.Helpers
{
    public interface IApiHelper
    {
        Task<AuthenticateUser> Authenticate(string username, string password);
    }
}