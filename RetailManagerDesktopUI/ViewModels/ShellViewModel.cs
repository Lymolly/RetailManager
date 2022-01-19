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
        private IEventAggregator _eventAggregator;
        private SalesViewModel _salesViewModel;
        public ShellViewModel(SalesViewModel svm,IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
            _salesViewModel = svm;

            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);
        }
    }
}
