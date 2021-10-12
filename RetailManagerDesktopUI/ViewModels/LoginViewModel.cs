using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using RetailManagerDesktopUI.EventModels;
using RetailManagerDesktopUI.Helpers;
using RetailManagerDesktopUI.Library.Api;
using RetailManagerDesktopUI.Models;

namespace RetailManagerDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private bool isErrorVisible;
        private string errorMessage;
        private readonly IApiHelper _apiHelper;
        private IEventAggregator _eventAggregator;
        public LoginViewModel(IApiHelper apiHelper,IEventAggregator eventAggregator)
        {
            _apiHelper = apiHelper;
            _eventAggregator = eventAggregator;
        }
        public string Username
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool output = false;
                if (Username?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }
                return output;
            }
        }

        
        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }
        

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
                errorMessage = value;
            }
        }



        public async Task LogIn()
        {
            try
            {
                var user =  await _apiHelper.Authenticate(Username, Password);
                await _apiHelper.GetLoggedInUserInfo(user.Access_Token);

                _eventAggregator.PublishOnUIThreadAsync(new LogOnEventModel());
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }


    }
}
