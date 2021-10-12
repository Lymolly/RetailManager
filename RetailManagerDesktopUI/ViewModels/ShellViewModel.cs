using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using RetailManagerDesktopUI.EventModels;

namespace RetailManagerDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>,IHandle<LogOnEventModel>
    {
        private LoginViewModel _loginViewModel;
        private IEventAggregator _eventAggregator;
        private SalesViewModel _salesViewModel;
        private SimpleContainer _container;
        public ShellViewModel(LoginViewModel lvm,SalesViewModel svm,IEventAggregator eventAggregator,
            SimpleContainer container)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            _container = container;

            _salesViewModel = svm;
            _loginViewModel = lvm; 

            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);
        }
    }
}
