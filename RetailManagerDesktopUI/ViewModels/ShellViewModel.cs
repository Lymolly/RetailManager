using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using RetailManagerDesktopUI.EventModels;
using RetailManagerDesktopUI.Library.Api;
using RetailManagerDesktopUI.Library.Models;

namespace RetailManagerDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>,IHandle<LogOnEventModel>
    {
        private IEventAggregator _eventAggregator;
        private SalesViewModel _salesViewModel;
        private ILoginUserModel _user;
        private IApiHelper _apiHelper;
        public ShellViewModel(SalesViewModel svm,IEventAggregator eventAggregator, ILoginUserModel user, IApiHelper apiHelper)
        {
            _eventAggregator = eventAggregator;
            _salesViewModel = svm;
            _user = user;
            _apiHelper = apiHelper;

            _eventAggregator.SubscribeOnPublishedThread(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }
        public async Task ExitApplication()
        {
            await TryCloseAsync();
        }
        public async Task LogOut()
        {
            _user.LogOffUser();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsAccountVisible);
        }
        public async Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);
            NotifyOfPropertyChange(() => IsAccountVisible);
        }
        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>());
        }
        public bool IsAccountVisible
        {
            get
            {
                //Always true cause token is null
                bool output = true;//false;
                if (!string.IsNullOrWhiteSpace(_user.Token))
                {
                    output = true;
                }
                return output;
            }

        }
    }
}
