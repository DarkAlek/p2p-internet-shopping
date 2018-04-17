using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using MahApps.Metro.Controls;
using client_desktop.Model.UnitOfWorks;
using client_desktop.Model.Models;

namespace client_desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int _noOfErrorsOnScreen = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = UsersWebViewModel.Instance;
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (UsersWebViewModel.Instance.Users != null)
            {
                List<UserApi> list = UsersWebViewModel.Instance.Users.ToList();
                list.Add(new UserApi());
                UsersWebViewModel.Instance.Users = list;
                UsersWebViewModel.Instance.SelectedUser = UsersWebViewModel.Instance.Users.LastOrDefault();

                AddUserWindow adduserwin = new AddUserWindow();
                adduserwin.DataContext = UsersWebViewModel.Instance;
                adduserwin.Owner = MyWindow;
                MyWindow.Opacity = 0.3;
                adduserwin.ShowDialog();
                MyWindow.Opacity = 1;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.EditWindow.IsOpen = true;
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            this.EditWindow.IsOpen = false;
            
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            this.EditWindow.IsOpen = true;
        }
    }
}
