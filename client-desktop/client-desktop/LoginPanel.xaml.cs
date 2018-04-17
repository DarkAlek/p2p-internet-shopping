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
using System.Net.Http;
using System.Net.Http.Headers;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using client_desktop.Model.Models;
using client_desktop.Model;

namespace client_desktop
{
    /// <summary>
    /// Interaction logic for LoginPanel.xaml
    /// </summary>
    public partial class LoginPanel
    {

        public LoginPanel()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text;
            string password = passwordBox.Password;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:50747");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(login.Trim() + ":" + password.Trim())));
            client.DefaultRequestHeaders.Authorization = authValue;

            try
            {
                HttpResponseMessage response = await client.GetAsync("api/users");
                response.EnsureSuccessStatusCode();
                DataContainer.Instance.Login = login;
                DataContainer.Instance.Password = password;
                MainWindow mainwindow = new MainWindow();
                mainwindow.Show();
                this.Close();

            }
            catch (Exception)
            {
                await this.ShowMessageAsync("Error", "Wrong data to access application or problem with connection");
            }

            //    if (check_data(login, password))
            //    {
            //        MainWindow mainwindow = new MainWindow();
            //        this.Close();
            //        mainwindow.Show();

            //    }
            //    else
            //    {
            //        this.ShowMessageAsync("Error", "Wrong data to access application");
            //    }
        }

        //private bool check_data(string login, string password)
        //{
        //    string path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + "\\data.txt";

        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(path))
        //        {
        //            string check_login = sr.ReadLine();
        //            string check_password = sr.ReadLine();

        //            string hash_login = Hash(login);
        //            string hash_password = Hash(password);

        //            if (hash_login == check_login && hash_password == check_password)
        //            {
        //                UsersWebViewModel.Instance.Login = login;
        //                UsersWebViewModel.Instance.Password = password;
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}

        //private string Hash(string data)
        //{
        //    var bytes = new UTF8Encoding().GetBytes(data);
        //    byte[] hashBytes;
        //    using (var algorithm = new System.Security.Cryptography.SHA512Managed())
        //    {
        //        hashBytes = algorithm.ComputeHash(bytes);
        //    }
        //    return Convert.ToBase64String(hashBytes);
        //}
    }
}
