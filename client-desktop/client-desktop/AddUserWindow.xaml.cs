using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using client_desktop.Model.Models;

namespace client_desktop
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow
    {
        private int _noOfErrorsOnScreen = 0;

        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            List<UserApi> list = UsersWebViewModel.Instance.Users.ToList();
            list.Remove(list.LastOrDefault());
            UsersWebViewModel.Instance.Users = list;
            UsersWebViewModel.Instance.SelectedUser = UsersWebViewModel.Instance.Users.LastOrDefault();
        }
    }


}
