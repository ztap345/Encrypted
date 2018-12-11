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
using EncryptedFileManager.Utilities;

namespace EncryptedFileManager
{
    public partial class RegistrationScreen : Window
    {

        private LoginFile login;
        private string un, pw, dn;

        public RegistrationScreen()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            login = new LoginFile();
            dn = txtDisplayName.Text;
            un = txtUsername.Text;
            pw = txtPassword.Password.ToString();

            if ( un!="" && pw!="" && pw.Length >= 8) {
                login.UpdateFile(new User(un, pw, dn));
                var win = new LoginScreen();
                win.Show();
                this.Close();
            }
            else if(un == "" || pw == "")
            {
                RegErrMessage.Text = "Username and/or Password cannot be empty.";
            }
            else
            {
                RegErrMessage.Text = "Password must be be at least 8 characters.";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(login != null)
                login.UnloadLogin();
        }
    }
}
