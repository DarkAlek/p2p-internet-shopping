using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using client_desktop;
using System.Windows.Input;
using client_desktop.Commands;
using client_desktop.Model.UnitOfWorks;
using client_desktop.Model.Repositories;
using Ninject;

namespace client_desktop.Model.Models
{
    public class UsersWebViewModel : INotifyPropertyChanged
    {

        private static  UsersWebViewModel instance;

        public event PropertyChangedEventHandler PropertyChanged = null;
        private IWebUnitOfWork context;
        public ICommand AddCustomerCmd { get; set; }
        public ICommand AddProviderCmd { get; set; }
        public ICommand AddAdminCmd { get; set; }
        public ICommand SaveUserCmd { get; set; }
        public ICommand DeleteUserCmd { get; set; }
        public ICommand RefreshUsersCmd { get; set; }

        public static UsersWebViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UsersWebViewModel();
                }
                return instance;
            }
        }

        public UsersWebViewModel()
        {
            LoadData();
            Init();
        }

        public UsersWebViewModel(IWebUnitOfWork c)
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

        private IEnumerable<UserApi> users;
        public IEnumerable<UserApi> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }


        private UserApi selecteduser;
        public UserApi SelectedUser
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
            context = kernel.Get<IWebUnitOfWork>();

            Users = context.UserWebRepository.GetUsersApi().Result;
        }

        private void AddCustomer()
        {
            var user = new UserApi
            {
                UserName = SelectedUser.UserName,
                Email = SelectedUser.Email,
                Password = SelectedUser.Password,
                PhoneNumber = SelectedUser.PhoneNumber,
                Type = 0
            };
            try
            {
                var task = Task.Run(async () => { await context.UserWebRepository.InsertUserApi(user); });
                task.Wait();
                LoadData();
                SelectedUser = Users.FirstOrDefault(u => u.Id == user.Id);
            }
            catch(Exception)
            {

            }
        }

        private void AddProvider()
        {
            Object thisLock = new Object();

            var user = new UserApi
            {
                UserName = SelectedUser.UserName,
                Email = SelectedUser.Email,
                Password = SelectedUser.Password,
                PhoneNumber = SelectedUser.PhoneNumber,
                Type = 1
            };
            try
            {
                var task = Task.Run(async () => { await context.UserWebRepository.InsertUserApi(user); });
                task.Wait();
                LoadData();
                SelectedUser = Users.FirstOrDefault(u => u.Id == user.Id);
            }
            catch (Exception)
            {

            }
        }

        private void AddAdmin()
        {
            var user = new UserApi
            {
                UserName = SelectedUser.UserName,
                Email = SelectedUser.Email,
                Password = SelectedUser.Password,
                PhoneNumber = SelectedUser.PhoneNumber,
                Type = 2
            };
            try
            {
                var task = Task.Run(async () => { await context.UserWebRepository.InsertUserApi(user); });
                task.Wait();
                LoadData();
                SelectedUser = Users.FirstOrDefault(u => u.Id == user.Id);
            }
            catch(Exception)
            {

            }
        }

        public void SaveUser()
        {
            // TO CHECK
            if (SelectedUser != null)
            {
                context.UserWebRepository.UpdateUserApi(SelectedUser);
            }
        }

        public void DeleteUser()
        {
            if (SelectedUser != null)
            {
                try
                {
                    var task = Task.Run(async () => { await context.UserWebRepository.DeleteUserApiById(SelectedUser.Id); });
                    task.Wait();
                    Users = context.UserWebRepository.GetUsersApi().Result;
                    SelectedUser = Users.LastOrDefault();
                }
                catch(Exception)
                {

                }
            }
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
