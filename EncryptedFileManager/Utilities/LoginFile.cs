using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace EncryptedFileManager.Utilities
{

    class LoginFile
    {
        const string fileName = "Login.bin";
        const string encryptedFileName = "Login.binE";
        private List<EncryptedUser> userList = new List<EncryptedUser>();

        public LoginFile()
        {
            LoadFile();
        }

        public bool AuthenticateLogin(User userInfo)
        {
            if (Contains(new EncryptedUser(userInfo)))
            {
                Trace.WriteLine("FOUND ME");
                return true;
            }
            else
            {
                Trace.WriteLine("DID NOT FIND ME");
                return false;
            }
        }

        public void UpdateFile(User userInfo)
        {
            if (File.Exists(encryptedFileName))
            {
                EncryptedFile.DecryptLogin(fileName);
                File.Delete(encryptedFileName);
            }

            using(BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Append)))
            {
                EncryptedUser u = new EncryptedUser(userInfo);
                writer.Write(u.EuserName);
                writer.Write(u.EpassWord);
                writer.Write(u.DisplayName);
                Trace.WriteLine(u.EuserName + ", " + u.EpassWord + " and "+ u.DisplayName + " are written to the file!");
            }

            EncryptedFile.EncryptLogin(fileName);
            File.Delete(fileName);
        }

        private void LoadFile()
        {
            if (File.Exists(encryptedFileName))
            {
                EncryptedFile.DecryptLogin(fileName);
                File.Delete(encryptedFileName);
            }

            if (File.Exists(fileName))
            {
                using(BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    Trace.WriteLine("Login.bin was opened!");

                    Int64 username, password;
                    string displayName;

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        username = reader.ReadInt64();
                        password = reader.ReadInt64();
                        displayName = reader.ReadString();

                        Trace.WriteLine(username + "," + password + " and " + displayName + " have been read!");

                        userList.Add(new EncryptedUser(username,password,displayName));
                    }
                }
            }
            else
            {
                using(BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Create)))
                {
                    Trace.WriteLine("Login.bin was created");
                }
            }
        }

        public void UnloadLogin()
        {
            if (!File.Exists(encryptedFileName))
            {
                EncryptedFile.EncryptLogin(fileName);
                File.Delete(fileName);
            }
        }

        public string Find(User user)
        {
            EncryptedUser u = new EncryptedUser(user);

            foreach(EncryptedUser e in userList)
            {

                if (e.EuserName == u.EuserName && e.EpassWord == u.EpassWord)
                    return e.DisplayName;
            }
                

            return u.UserName;
        }

        private bool Contains(EncryptedUser Euser)
        {
            foreach(EncryptedUser e in userList)
            {
                if (e.EuserName == Euser.EuserName && e.EpassWord == Euser.EpassWord)
                    return true;
            }
            return false;
        }
    }
}
