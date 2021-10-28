using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using RetailManagerDesktopUI.Library.Models;
using RetailManagerDesktopUI.Models;


namespace RetailManagerDesktopUI.Library.Api
{
    //TODO this class is just for a time(working now)
    //TODO refactor tis piece of shit
    public class ApiHelper : IApiHelper
    {
        private HttpClient _apiclient;
        public HttpClient ApiClient
        {
            get => _apiclient;
            set => _apiclient = value;
        }

        private ILoginUserModel _loginUser;
        public ApiHelper(ILoginUserModel loginUser)
        {
            InitializeClient();
            this._loginUser = loginUser;
        }

        private void InitializeClient()
        {
            var config = ConfigurationManager.AppSettings["baseUri"];
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(config);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticateUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string,string>("password",password),
            });
            using (HttpResponseMessage respMsg = await ApiClient.PostAsync("Token",data))
            {
                if (respMsg.IsSuccessStatusCode)
                {
                    var result = await respMsg.Content.ReadAsAsync<AuthenticateUser>();
                    return result;
                }
                else
                {
                    throw new Exception(respMsg.ReasonPhrase);
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            ApiClient.DefaultRequestHeaders.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");

            using (var response = await ApiClient.GetAsync("api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoginUserModel>();
                    _loginUser = result;
                    _loginUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            
        }
    }
}
