using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedFileManager.Utilities
{
    public class EncryptedUser:User
    {
        public Int64 EuserName { get; set; }
        public Int64 EpassWord { get; set; }

        public EncryptedUser(User user)
        {
            UserName = user.UserName;
            PassWord = user.PassWord;
            DisplayName = user.DisplayName;
            Encrypt();
        }

        public EncryptedUser(Int64 _EuserName, Int64 _EpassWord, string dn)
        {
            EuserName = _EuserName;
            EpassWord = _EpassWord;
            DisplayName = dn;
        }

        private void Encrypt()
        {
            EuserName = Encryption(UserName);
            EpassWord = Encryption(PassWord);
        }


        private Int64 Encryption(string s)
        {
            Int64 e = 0;

            foreach (char c in s)
            {
                e += (int)c;
            }

            return e;
        }

        public static User GetUser(EncryptedUser e)
        {
            return new User(e.UserName, e.PassWord, e.DisplayName);
        }
    }
}
