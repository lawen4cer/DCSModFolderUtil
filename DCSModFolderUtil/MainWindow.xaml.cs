using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace DCSModFolderUtil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileSecurity fileSecurity;

        string fileNamePattern = @"(\\\w+)?(\.\w+)";

        public string InputDirectory { get; set; }
        public string OutputDirectory { get; set; }

        public MainWindow()
        {

            InitializeComponent();
            dcsPathTextBox.Text = ConfigurationManager.AppSettings.Get("DCS_PATH");


        }

        private void openDcsPathFolderExplorer(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dcsPathTextBox.Text = folderBrowserDialog.SelectedPath;
                ConfigurationManager.AppSettings.Set("DCS_PATH", folderBrowserDialog.SelectedPath);

            }
        }

        private void openPathToDirectoryFolderExplorer(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InputDirectory = openFileDialog.FileName;
                pathToDirectoryTextBox.Text = InputDirectory;
            }
        }

        private void openPathToOutputFolderExplorer(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OutputDirectory = folderBrowserDialog.SelectedPath;
                pathToOutputTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            var modPath = InputDirectory.Replace(ConfigurationManager.AppSettings.Get("DCS_PATH"), String.Empty);

            var output = System.IO.Path.Combine(System.IO.Path.GetFullPath(OutputDirectory), ModName.Text);

            var initData = "";

            //TODO: fix regex
            var fileName = Regex.Match(InputDirectory, fileNamePattern).Value;




            string normalizedFirstPath = output.TrimEnd(new char[] { '\\' });
            string normalizedSecondPath = modPath.TrimStart(new char[] { '\\' });
            var strFinalPath = System.IO.Path.Combine(normalizedFirstPath, normalizedSecondPath);


            var createdDirectory = System.IO.Path.Combine(normalizedFirstPath, normalizedSecondPath).TrimEnd(fileName.ToCharArray());


            System.IO.Directory.CreateDirectory(createdDirectory);



            byte[] result = File.ReadAllBytes(InputDirectory);

            using (var file = File.Create(strFinalPath, 2048, FileOptions.None))
            {
                fileSecurity = new FileSecurity(strFinalPath, AccessControlSections.All);

                file.SetAccessControl(fileSecurity);
                file.Write(result, 0, result.Length);
            }



            System.Windows.MessageBox.Show($"Created Directory at {createdDirectory}", "created path", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
