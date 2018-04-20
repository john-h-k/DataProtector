using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace DataProtector
{
    /// <summary>
    /// Interaction logic for DecryptPage.xaml
    /// </summary>
    public partial class DecryptPage : Page
    {

        private string filePath;

        private byte[] data;

        public static string EntropyPath;

        public DecryptPage()
        {
            InitializeComponent();
            if (Is_Entropy_External.IsChecked == false)
            {
                Entropy_Browse_Label.Opacity = 0.5;
                Entropy_Browse_Button.Opacity = 0.5;
                Entropy_Browse_Button.IsEnabled = false;
                Entropy_File_Path.Opacity = 0.5;
                Entropy_File_Path.IsEnabled = false;
                EntropyPath = Environment.GetFolder(Environment.SpecialFolder.MyComputer) + "@\Documents";
                //Directory.CreateDirectory(EntropyPath);
            }
        }

        private void Decrypt(object sender, RoutedEventArgs e)
        {
            var entropy = new byte[16];
            int ID;
            using (var BinReader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                ID = BinReader.ReadInt32();
                EntropyPath += ID.ToString() + ".dat";
            }
            using (var BinReader = new BinaryReader(File.Open(EntropyPath, FileMode.Open)))
            {
                entropy = BinReader.ReadBytes(16);
            }
            long length = new FileInfo(filePath).Length;
            data = SecureDataProtector.UnprotectDataFromFile(entropy, filePath, length);
            Decrypted_Data_Box.Text = Encoding.UTF8.GetString(data);
        }

        private void Browse_File(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileBrowser = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = Environment.GetFolder(Environment.SpecialFolder.MyComputer)
            };
            bool? result = fileBrowser.ShowDialog();
            if (result == true)
            {
                Data_File_Path.Text = fileBrowser.FileName;
                filePath = fileBrowser.FileName;
            }
        }

        private void Browse_Entropy_File(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileBrowser = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = Environment.GetFolder(Environment.SpecialFolder.MyComputer)
            };
            bool? result = fileBrowser.ShowDialog();
            if (result == true)
            {
                Entropy_File_Path.Text = fileBrowser.FileName;
                EntropyPath = fileBrowser.FileName;
            }
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SelectionPage();
        }

        private void On_Entropy_State_Changed(object sender, RoutedEventArgs e)
        {
            if (Is_Entropy_External.IsChecked == false)
            {
                Entropy_Browse_Label.Opacity = 0.5;
                Entropy_Browse_Button.Opacity = 0.5;
                Entropy_Browse_Button.IsEnabled = false;
                Entropy_File_Path.Opacity = 0.5;
                Entropy_File_Path.IsEnabled = false;
                EntropyPath = String.Format(Environment.GetFolder(Environment.SpecialFolder.MyComputer) + @"\DataProtector\{0}", Environment.UserName);
            }
            else
            {
                Entropy_Browse_Label.Opacity = 1;
                Entropy_Browse_Button.Opacity = 1;
                Entropy_Browse_Button.IsEnabled = true;
                Entropy_File_Path.Opacity = 1;
                Entropy_File_Path.IsEnabled = true;
                EntropyPath = null;
            }
        }
    }
}
