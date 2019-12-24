using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows.Forms;

namespace DCSModFolderUtil
{
    public static class ConfigurationUtil
    {

        public static string GetSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return String.Empty;
            }
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;



                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else if (settings[key].Value == value)
                {
                    return;
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                MessageBox.Show("Error occurred saving configuration settings", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
