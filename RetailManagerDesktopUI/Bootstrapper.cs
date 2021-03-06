using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using RetailManagerDesktopUI.Helpers;
using RetailManagerDesktopUI.Library.Api;
using RetailManagerDesktopUI.Library.Helpers;
using RetailManagerDesktopUI.Library.Models;
using RetailManagerDesktopUI.ViewModels;

namespace RetailManagerDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer simpleContainer = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged"
            );
        }
        protected override void Configure()
        {
            simpleContainer.Instance(simpleContainer)
                .PerRequest<IProductEndpoint, ProductEndpoint>()
                .PerRequest<ISaleEndpoint, SaleEndpoint>();

            simpleContainer
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoginUserModel, LoginUserModel>()
                .Singleton<IConfigHelper,ConfigHelper>()
                //.Singleton<IProductEndpoint,ProductEndpoint>()
                //.Singleton<ISaleEndpoint,SaleEndpoint>()
                .Singleton<IApiHelper, ApiHelper>();
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel")).ToList()
                .ForEach(type => simpleContainer.RegisterPerRequest(type,type.ToString(),type));

        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
        protected override object GetInstance(Type service, string key)
        {
            return simpleContainer.GetInstance(service, key);
        }
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return simpleContainer.GetAllInstances(service);
        }
        protected override void BuildUp(object instance)
        {
            simpleContainer.BuildUp(instance);
        }
    }
}
