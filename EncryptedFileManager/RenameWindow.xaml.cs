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
using System.IO;
using System.Diagnostics;

namespace EncryptedFileManager
{
    /// <summary>
    /// Interaction logic for RenameWindow.xaml
    /// </summary>
    public partial class RenameWindow : Window
    {
        private FileInfo currentFile;

        public RenameWindow(FileInfo c)
        {
            currentFile = c;
            InitializeComponent();
        }

        private void SubmitRename_Click(object sender, RoutedEventArgs e)
        {
            if (!HasInvalidChar(FileChanger.Text))
            {

                string newFileName = Path.Combine(currentFile.Directory.FullName, FileChanger.Text + currentFile.Extension);

                Trace.WriteLine(currentFile.FullName);
                Trace.WriteLine(newFileName);

                if (!File.Exists(newFileName))
                {
                    currentFile.MoveTo(newFileName);
                    Trace.WriteLine("File Renamed");
                    this.Close();
                }
                else
                {
                    MessageBoxResult Confirmation = MessageBox.Show("File Already Exists", "Error", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBoxResult Confirmation = MessageBox.Show("Invalid Characters In Filename.", "Error", MessageBoxButton.OK);
            }
        }

        private bool HasInvalidChar(string s)
        {
            foreach(var c in Path.GetInvalidFileNameChars())
            {
                if (s.Contains(c))
                    return true;
            }

            return false;
        }
    }
}
