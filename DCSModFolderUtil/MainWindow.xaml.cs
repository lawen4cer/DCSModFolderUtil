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
using Microsoft.Win32;

namespace DCSModFolderUtil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InputDirectory = folderBrowserDialog.SelectedPath;
                pathToDirectoryTextBox.Text = folderBrowserDialog.SelectedPath;
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




            string normalizedFirstPath = output.TrimEnd(new char[] { '\\' });
            string normalizedSecondPath = modPath.TrimStart(new char[] { '\\' });
            var strFinalPath = System.IO.Path.Combine(normalizedFirstPath, normalizedSecondPath);


            var createdDirectory = System.IO.Path.Combine(normalizedFirstPath, normalizedSecondPath);
            System.Windows.MessageBox.Show($"Created Directory at {createdDirectory}", "created path", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            System.IO.Directory.CreateDirectory(strFinalPath);

        }
    }
}
