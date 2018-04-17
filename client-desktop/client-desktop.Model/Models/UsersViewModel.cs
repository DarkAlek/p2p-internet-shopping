using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using client_desktop;
using System.Windows.Input;
using client_desktop.Commands;
using client_desktop.Model.UnitOfWorks;
using Ninject;

namespace client_desktop.Model.ViewModel
{
    public class UsersViewModel : INotifyPropertyChanged
    {
        private static UsersViewModel instance;

        public event PropertyChangedEventHandler PropertyChanged = null;
        private IUnitOfWork context;
        public ICommand AddCustomerCmd { get; set; }
        public ICommand AddProviderCmd { get; set; }
        public ICommand AddAdminCmd { get; set; }
        public ICommand SaveUserCmd { get; set; }
        public ICommand DeleteUserCmd { get; set; }
        public ICommand RefreshUsersCmd { get; set; }

        public static UsersViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UsersViewModel();
                }
                return instance;
            }
        }

        public UsersViewModel()
        {
            LoadData();
            Init();
        }

        public UsersViewModel(IUnitOfWork c)
        {
            context = c;

            Init();
        }

        private void Init()
        {
            AddCustomerCmd = new RelayCommand(x => AddCustomer());
            AddProviderCmd = new RelayCommand(x => AddProvider());
            AddAdminCmd = new RelayCommand(x => AddAdmin());
            RefreshUsersCmd = new RelayCommand(x => LoadData());
            DeleteUserCmd = new RelayCommand(x => DeleteUser());
            SaveUserCmd = new RelayCommand(x => SaveUser());
        }

        private IEnumerable<User> users;
        public IEnumerable<User> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }


        private User selecteduser;
        public User SelectedUser
        {
            get { return selecteduser; }
            set
            {
                selecteduser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public void LoadData()
        {
            IKernel kernel = new StandardKernel(new NinjectKernel());
            context = kernel.Get<IUnitOfWork>();

            Users = context.UserRepository.GetUsers();
        }

        private void AddCustomer()
        {
            var user = new Model.Customer
            {
                FirstName = "",
                SecondName = "",
                PhoneNumber = "",
                Activated = false,
            };
            context.UserRepository.InsertUser(user);
            context.UserRepository.Save();
            LoadData();
            SelectedUser = Users.FirstOrDefault(u => u.Id == user.Id);
        }

        private void AddProvider()
        {
            var user = new Model.Provider
            {
                FirstName = "",
                SecondName = "",
                PhoneNumber = "",
                Activated = false,
            };
            context.UserRepository.InsertUser(user);
            context.UserRepository.Save();
            LoadData();
            SelectedUser = Users.FirstOrDefault(u => u.Id == user.Id);
        }

        private void AddAdmin()
        {
            var user = new Model.Admin
            {
                FirstName = "",
                SecondName = "",
                Activated = false,
            };
            context.UserRepository.InsertUser(user);
            context.UserRepository.Save();
            Users = context.UserRepository.GetUsers();
            LoadData();
            SelectedUser = Users.FirstOrDefault(u => u.Id == user.Id);
        }

        public void SaveUser()
        {
            if (SelectedUser != null)
            {
                context.UserRepository.UpdateUser(SelectedUser);
                context.UserRepository.Save();
            }
        }

        public void DeleteUser()
        {
            if (SelectedUser != null)
            {
                context.UserRepository.DeleteUserById(SelectedUser.Id);
                context.UserRepository.Save();
                Users = context.UserRepository.GetUsers();
                SelectedUser = Users.LastOrDefault();
            }
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private UsersViewModel GetViewModel()
        {
            return new UsersViewModel();
        }
    }
}
