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
using System.Windows.Shapes;

namespace DataProtector
{
    /// <summary>
    /// Interaction logic for SelectionPage.xaml
    /// </summary>
    public partial class SelectionPage : Page
    {
        public SelectionPage()
        {
            InitializeComponent();
        }

        private void Open_Encrypt(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new EncryptPage();
        }

        private void Open_Decrypt(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new DecryptPage();
        }

        private void Open_Settings(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new SettingsPage();
        }
    }
}
