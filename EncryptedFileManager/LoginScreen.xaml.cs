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
using Microsoft.SqlServer;
using EncryptedFileManager.Utilities;
using System.Diagnostics;

namespace EncryptedFileManager
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {

        private string un;
        private string pw;
        private LoginFile login;
        private User Current;


        public LoginScreen()
        {
            Trace.WriteLine("Im open!");
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            login = new LoginFile();
            un = txtUsername.Text;
            pw = txtPassword.Password.ToString();

            User u = new User(un, pw);

            if (login.AuthenticateLogin(u)) {
                Current = u;
                Current.DisplayName = login.Find(u);
                var win = new MainWindow(Current);
                win.Show();
                this.Close();
            }
            else
            {
                LoginErrMessage.Text = "Incorrect Username and/or Password, Please Try Again.";
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var win = new RegistrationScreen();
            win.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(login != null)
                login.UnloadLogin();
        }
    }
}
