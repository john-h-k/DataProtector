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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class EncryptPage : Page
    {

        private string text;

        private string filePath;

        private string entropyPath;

        private bool isEntropyExternal;

        public bool IsEntropyExternal
        {
            get
            {
                return isEntropyExternal;
            }
        }
        public long _LastLength;

        public EncryptPage()
        {
            InitializeComponent();
            isEntropyExternal = true;
            Is_Using_Entropy.IsChecked = isEntropyExternal;
            Is_Data_User_Specific.IsChecked = true;
            Is_Entropy_External.IsChecked = false;
            entropyPath = String.Format(Environment.GetFolder(Environment.SpecialFolder.MyComputer) + @"\DataProtector\data.bin", Environment.UserName);
            var a = new object();
            var b = new RoutedEventArgs();
            On_Entropy_State_Changed(a, b);
        }

        private void Browse_File(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileBrowser = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = Environment.GetFolder(Environment.SpecialFolder.MyComputer);
            };
            bool? result = fileBrowser.ShowDialog();
            if (result == true)
            {
                Data_File_Path.Text = fileBrowser.FileName;
                filePath = fileBrowser.FileName;
            }
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
            }
            else
            {
                Entropy_Browse_Label.Opacity = 1;
                Entropy_Browse_Button.Opacity = 1;
                Entropy_Browse_Button.IsEnabled = true;
                Entropy_File_Path.Opacity = 1;
                Entropy_File_Path.IsEnabled = true;
                entropyPath = null;
            }
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            bool fail = false;
            if (Data_Box.Text == "")
            {
                Error_Box.Content = "Please enter some text\n";
                fail = true;
            }
            this.text = Data_Box.Text;
            if (filePath == "" || filePath == null)
            {
                Error_Box.Content += "Please select a file";
                fail = true;
            }
            if ((entropyPath == "" || entropyPath == null) && isEntropyExternal == true)
            {
                Error_Box.Content += "Please select an entropy file";
                fail = true;
            }
            if (fail == true)
            {
                return;
            }

            
            int ID = 0;
            var rng = new Random();
            
            Loop:
            ID = rng.Next(int.MaxValue);
            
            foreach (var item in SettingsPage.IDArray)
            {
                if (ID == item)
                {
                    rng.Next(ID);
                    goto Loop;
                }
            }
            
            using (var BinWriter = new BinaryWriter(File.Create(filePath)))
            {
                BinWriter.Write(ID);
            }
            
            entropyPath = DecryptPage.EntropyPath = String.Format(Environment.GetFolder(Environment.SpecialFolder.MyComputer) + @"\DataProtector\{0}", ID.ToString());
            _LastLength = SecureDataProtector.ProtectDataToFile(Encoding.UTF8.GetBytes(text), filePath, entropyPath);
            Application.Current.MainWindow.Content = new EncryptionSuccesfulPage();
        }

        private void Browse_Entropy_File(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileBrowser = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            };
            bool? result = fileBrowser.ShowDialog();
            if (result == true)
            {
                Entropy_File_Path.Text = fileBrowser.FileName;
                entropyPath = fileBrowser.FileName;
            }
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SelectionPage();
        }
    }
}
