using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RetailManagerDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private LoginViewModel _loginViewModel;

        public ShellViewModel(LoginViewModel lvm)
        {
            _loginViewModel = lvm; 
            ActivateItemAsync(_loginViewModel);
        }
    }
}
