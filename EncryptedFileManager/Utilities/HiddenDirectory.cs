using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EncryptedFileManager.Utilities
{
    class HiddenDirectory
    {
        public string path;

        public HiddenDirectory(string p = "hidden")
        {
            path = p;
            Create();
        }

        private void Empty(string userDir)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(path,userDir));
            foreach(FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach(DirectoryInfo d in di.EnumerateDirectories())
            {

                d.Delete(true);
            }
        }

        public void Show()
        {
            DirectoryInfo di = new DirectoryInfo(path);
            di.Attributes = FileAttributes.Normal;
        }

        public void Hide()
        {
            DirectoryInfo di = new DirectoryInfo(path);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden | FileAttributes.System;
        }

        private void Create()
        {
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden | FileAttributes.System;
            }
        }

        public void Delete(string userDir)
        {
            Empty(userDir);
            Directory.Delete(Path.Combine(path,userDir));
        }
        
    }
}
