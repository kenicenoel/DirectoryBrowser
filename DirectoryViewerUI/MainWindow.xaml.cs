using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
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
            // Get every drive in the system
           Directory.GetLogicalDrives().ToList().ForEach(drive =>
           {
               // Create a item to put into the treeview setting the header and the path
               var item = new TreeViewItem()
               {
                    // Set the Header
                   Header = drive,
                   // Full Path
                   Tag = drive
               };
               item.Items.Add(null);

               // Listen for when the item has been expanded
               if (HasWritePermissionOnDir(drive.ToString()))
               {
                   item.Expanded += Folder_Expanded;
               }
               

               // Add the item to the treeview
               FolderView.Items.Add(item);
           });
        }
        #endregion

        #region Events

        public void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem) sender;
            
            // If the item only contains the dummy data
            if (item.Items.Count != 1 || item.Items[0] != null) { return;}
            
            // Clear dummy data
            item.Items.Clear();

            var fullPath = (string) item.Tag;
            Directory.GetDirectories(fullPath).ToList()
                .ForEach(path =>
                {
                    var subItem = new TreeViewItem()
                    {
                        Header = path

                    };
                    item.Items.Add(subItem);

                });

        }

        #endregion

        public static bool HasWritePermissionOnDir(string path)
        {
            var writeAllow = false;
            var writeDeny = false;
            var accessControlList = Directory.GetAccessControl(path);
            var accessRules = accessControlList?.GetAccessRules(true, true,
                typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                    continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    writeAllow = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    writeDeny = true;
            }

            return writeAllow && !writeDeny;
        }

    }
}
