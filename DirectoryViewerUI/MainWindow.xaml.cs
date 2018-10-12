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

namespace DirectoryViewerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region On Loaded

         private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
           Directory.GetLogicalDrives().ToList().ForEach(drive =>
           {
               var item = new TreeViewItem()
               {
                   Header = drive,
                   
               };
               FolderView.Items.Add(item);
           });
        }

        #endregion
       
    }
}
