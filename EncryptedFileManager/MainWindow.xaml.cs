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
using System.Windows.Navigation;
using EncryptedFileManager.Utilities;
using System.IO.Compression;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;


namespace EncryptedFileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User current;
        private HiddenDirectory dir;
        private string zipName;
        private string zipPath;
        private string encryptedZipPath;
        private string userDir;
        private string userDirPath;
        private string userStorageFolder = "storage";

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(User c)
        {
            this.current = c;
            dir = new HiddenDirectory(userStorageFolder);
            userDir = current.UserName;
            zipName = userDir + ".zip";
            userDirPath = Path.Combine(dir.path, userDir);
            zipPath = Path.Combine(dir.path, zipName);
            encryptedZipPath = zipPath + "E";
            if(!Directory.Exists(userDirPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(userDirPath);
            }
            UnZipDirectory();
            InitializeComponent();
            WindowHeader.Text = current.DisplayName + "'s Secure Files";
            LoadFiles();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            var win = new LoginScreen();
            win.Show();
            this.Close();
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();

            if(fileBrowser.ShowDialog() == true)
            {
                string moved = Path.Combine(userDirPath, Path.GetFileName(fileBrowser.FileName));
                try
                {
                    File.Move(fileBrowser.FileName, moved);
                    Trace.WriteLine(fileBrowser.FileName + " has been moved to " + userDirPath);
                }
                catch(IOException err)
                {
                    Trace.WriteLine(err);
                    if (File.Exists(moved))
                    {
                        MessageBoxResult Confirmation = MessageBox.Show("File Already Exists", "Error", MessageBoxButton.OK);
                    }
                }
            }

            LoadFiles();
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult Confirmation = MessageBox.Show("Are you sure you would like to delete the selected file?","Delete Confirmation", MessageBoxButton.YesNo);

            if (Confirmation == MessageBoxResult.Yes)
            {
                TextBlock txt = (FileHolder.SelectedItem as TextBlock);
                File.Delete(Path.Combine(userDirPath,txt.Text));
                Trace.WriteLine(txt.Text + " Deleted");
                LoadFiles();
            }
            else
            {
                Trace.WriteLine("File Not Deleted");
            }
        }

        private void RemoveFile_Click(object sender, RoutedEventArgs e)
        {
            using(var folderBrowser = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = folderBrowser.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    TextBlock txt = (FileHolder.SelectedItem as TextBlock);
                    File.Move(Path.Combine(userDirPath, txt.Text), Path.Combine(folderBrowser.SelectedPath,txt.Text));
                    Trace.WriteLine(folderBrowser.SelectedPath);
                    LoadFiles();
                }
            }
        }

        private void RenameFile_Click(object sender, RoutedEventArgs e)
        {
            TextBlock txt = (FileHolder.SelectedItem as TextBlock);
            FileInfo currentFile = new FileInfo(Path.Combine(userDirPath, txt.Text));

            RenameWindow r = new RenameWindow(currentFile);
            if (r.ShowDialog() == true) { }

            LoadFiles();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            TextBlock txt = (FileHolder.SelectedItem as TextBlock);
            Process.Start(Path.Combine(userDirPath, txt.Text));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ZipDirectory();
        }

        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void LoadFiles()
        {
            string[] File = Directory.GetFiles(userDirPath);
            FileHolder.Items.Clear();

            foreach(string s in File)
            {
                TextBlock txt = new TextBlock();
                txt.Text = Path.GetFileName(s);
                FileHolder.Items.Add(txt);
            }
        }

        private void ZipDirectory()
        {
            if (File.Exists(zipPath))
                File.Delete(zipPath);
            ZipFile.CreateFromDirectory(userDirPath, zipPath,CompressionLevel.Optimal,false);
            dir.Delete(userDir);
            EncryptZip();
            File.Delete(zipPath);
        }

        private void UnZipDirectory()
        {
            if (File.Exists(encryptedZipPath))
            {

                DecryptZip();
                File.Delete(encryptedZipPath);

                using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(userDirPath, entry.Name));
                    }
                }
            }
            File.Delete(zipPath);
        }

        private void EncryptZip()
        {
            EncryptedFile.Encrypt(zipPath, current);
        }

        private void DecryptZip()
        {
            EncryptedFile.Decrypt(zipPath, current);
        }
    }
}
