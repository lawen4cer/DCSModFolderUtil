using System;
using System.Configuration;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace DCSModFolderUtil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string InputDirectory { get; set; }
        public string OutputDirectory { get; set; } = ConfigurationManager.AppSettings.Get("DefaultOutput");
        public string DcsRootPath { get; set; } = ConfigurationManager.AppSettings.Get("DCS_PATH");



        public MainWindow()
        {

            InitializeComponent();
            dcsPathTextBox.Text = DcsRootPath;
            pathToOutputTextBox.Text = OutputDirectory;
            VersionLabel.Content = $"Version: {Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}";


        }

        private void openDcsPathFolderExplorer(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DcsRootPath = folderBrowserDialog.SelectedPath;
                dcsPathTextBox.Text = DcsRootPath;
                
            }
        }

        private void openPathToDirectoryFolderExplorer(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = DcsRootPath;
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
            var modPath = InputDirectory.Replace(DcsRootPath, String.Empty);

            var output = System.IO.Path.Combine(System.IO.Path.GetFullPath(OutputDirectory), ModName.Text);

            var fileName = System.IO.Path.GetFileName(InputDirectory);







            string normalizedFirstPath = output.TrimEnd(new char[] { '\\' });
            string normalizedSecondPath = modPath.TrimStart(new char[] { '\\' });
            var strFinalPath = System.IO.Path.Combine(normalizedFirstPath, normalizedSecondPath);


            var createdDirectory = System.IO.Path.Combine(normalizedFirstPath, normalizedSecondPath).TrimEnd(fileName.ToCharArray()).TrimEnd(new char[] { '\\' });

            try
            {
                if (!Directory.Exists(createdDirectory))
                {
                    Directory.CreateDirectory(createdDirectory);
                }
                if (File.Exists(strFinalPath))
                {
                    File.Delete(strFinalPath);
                }

                File.Copy(InputDirectory, strFinalPath);
                System.Windows.MessageBox.Show($"Created mod structure at {createdDirectory}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ConfigurationManager.AppSettings.Set("DCS_PATH", DcsRootPath);
                ConfigurationManager.AppSettings.Set("DefaultOutput", OutputDirectory);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"An error occurred while trying to copy the file. Make sure the output path doesn't already exist", "File Copy Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            

        }
    }
}
