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
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {

        public static int[] IDArray;

        public SettingsPage()
        {
            InitializeComponent();
        }

        static SettingsPage() 
        {
            string p = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Console.WriteLine(p);
            var files = Directory.GetFiles(p + @"\DataProtector");
            IDArray = new int[files.Length];
            for (var i = 0; i < files.Length; i++)
            {
                var num = files[i].Substring(0, files[i].Length - 4);
                var a = num.Split('\\');
                num = a[a.Length - 1];
                IDArray[i] = int.Parse(num);
            }
        }


    }
}
