using Caliburn.Micro;
using RetailManagerDesktopUI.Library.Api;
using RetailManagerDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RetailManagerDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly StatusInfoViewModel status;
        private readonly IWindowManager windowManager;
        private readonly IUserEndpoint userEndpoint;

        public BindingList<UserModel> users; 
        public BindingList<UserModel> Users
        { 
            get => users; 
            set
            { 
                users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }
        private UserModel _selectedUser;
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                SelectedUserName = value.Email;
                UserRoles.Clear();
                UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles();
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        private BindingList<string> _availableRoles = new BindingList<string>();
        public BindingList<string> AvailableRoles
        {
            get { return _availableRoles; }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        private string _selectedUserName;
        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }
        private BindingList<string> _userRoles = new BindingList<string>();
        public BindingList<string> UserRoles
        {
            get { return _userRoles; }
            set
            {
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }
        private string _selectedUserRole;

        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set 
            {
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
            }
        }
        private string _selectedAvailableRole;

        public string SelectedAvailableRole
        {
            get { return _selectedAvailableRole; }
            set 
            {
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }


        public UserDisplayViewModel(StatusInfoViewModel status,IWindowManager windowManager,IUserEndpoint userEndpoint)
        {
            this.status = status;
            this.windowManager = windowManager;
            this.userEndpoint = userEndpoint;
        }
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System error";
                if (ex.Message == "Unauthorized")
                {
                    status.UpdateMessage("Unauthorized access", "You dont have a permission");
                    await windowManager.ShowDialogAsync(status, null, settings);
                }
                else
                {
                    status.UpdateMessage("Fatal error", ex.Message);
                    await windowManager.ShowDialogAsync(status, null, settings);
                }
                await TryCloseAsync();
            }
        }
        private async Task LoadUsers()
        {
            var userList = await userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }
        private async Task LoadRoles()
        {
            var roles = await userEndpoint.GetAllRoles();
            foreach (var role in roles)
            {
                if (UserRoles.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }
        public async Task AddSelectedRole()
        {
            
            await userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);
            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);
          
        }
        public async Task RemoveSelectedRole()
        {
            await userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);
            UserRoles.Remove(SelectedUserRole);
            AvailableRoles.Add(SelectedUserRole);
        }
    }
}
