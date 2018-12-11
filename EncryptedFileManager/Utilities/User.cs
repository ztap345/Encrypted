using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedFileManager.Utilities
{
    public class User
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string DisplayName { get; set; }

        public User(string _username, string _password, string _displayName)
        {
            UserName = _username;
            DisplayName = _displayName;
            PassWord = _password;
        }

        public User(string _username, string _password)
        {
            UserName = _username;
            PassWord = _password;
        }

        public User() {}
    }
}
