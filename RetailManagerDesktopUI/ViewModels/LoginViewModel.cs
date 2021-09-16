﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using RetailManagerDesktopUI.Helpers;
using RetailManagerDesktopUI.Models;

namespace RetailManagerDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private IApiHelper _apiHelper;
        public LoginViewModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
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



        public async Task LogIn()
        {
            try
            {
                var user =  await _apiHelper.Authenticate(Username, Password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }
}
