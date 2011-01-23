using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using convendro;
using convendro.Properties;
using libconvendro;

namespace convendro.Classes {
    public static class Config {
        public static string LocalAppPath = "";

        /// <summary>
        /// Static accessor to the xx.xxx.xxx.xxx.xxx crap.
        /// </summary>
        public static Settings Settings {
            get { return convendro.Properties.Settings.Default; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string generateLocalAppFolder() {
            DirectoryInfo ning = Directory.CreateDirectory(Functions.GetCurrentLocalAppPath());

            return ning.FullName;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadFileFolderSettings() {
            if (Directory.Exists(Functions.GetCurrentLocalAppPath())) {
                LocalAppPath = Functions.GetCurrentLocalAppPath();
            } else {
                // The application runs in the ide or dev environment:
                // the Installer at all times should create the LocalAppPath
                // folder.
                LocalAppPath = Application.StartupPath;
            }

            // Find and set FFMPEG.
            if (String.IsNullOrEmpty(Settings.FFMPEGFilePath)) {
                Settings.FFMPEGFilePath = locateDefaultExecutablePath(
                    Functions.FILENAME_EXECUTABLE_FFMPEG);
            }

            // Find and set the Presetdata file
            if (String.IsNullOrEmpty(Settings.LastUsedPresetFile)) {
                Settings.LastUsedPresetFile = locateDefaultDataFilePath(
                    Functions.FILENAME_PRESETSDATA);
            }

            // Find and set the Commanddescription file.
            if (String.IsNullOrEmpty(Settings.LastUsedCommandDescriptionFile)) {
                Settings.LastUsedCommandDescriptionFile = locateDefaultDataFilePath(
                    Functions.FILENAME_COMMANDLINEDESCRIPTION);
            }


            // Generate default locations for storing input/output/mediasets
            GenerateDefaultFolders();
        }

        /// <summary>
        /// Initializes and loads settings from the AppConfig file.
        /// </summary>
        /// <param name="aform"></param>
        public static void LoadFormSettings(frmMain aform) {
            if (aform != null) {
                aform.Location = Settings.mainFormLocation;
                aform.Size = Settings.mainFormSize;
                aform.WindowState = Settings.mainFormState;

                string[] s = Settings.fileListViewColumns.Split(new char[] { '|' });

                if (s.Length >= aform.FileListView.Columns.Count) {
                    for (int i = 0; i < aform.FileListView.Columns.Count; i++) {
                        int disp = Convert.ToInt32(s[i]);
                        // TODO...
                        aform.FileListView.Columns[i].DisplayIndex = disp;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the Form settings to the INI file
        /// </summary>
        /// <param name="aform"></param>
        public static void SaveSettings(frmMain aform) {
            Settings.mainFormLocation = aform.Location;
            Settings.mainFormSize = aform.Size;
            Settings.mainFormState = aform.WindowState ;

            string s = "";
            foreach (ColumnHeader h in aform.FileListView.Columns) {
                s += h.DisplayIndex.ToString() + "|";
            }

            // Remove trailing "|"
            s = s.Trim(new char[] { '|' });
            Settings.fileListViewColumns = s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string locateDefaultExecutablePath(string filename) {
            string res = null;
            
            string defaultfilename = Functions.CombineCurrentFilePath(filename);

            if (File.Exists(defaultfilename)) {
                res = defaultfilename;
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string locateDefaultDataFilePath(string filename) {
            string res = null;

            string defaultfilename = Path.Combine(LocalAppPath, filename);

            if (File.Exists(defaultfilename)) {
                res = defaultfilename;
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void GenerateDefaultFolders() {
            
            if (String.IsNullOrEmpty(Settings.LastUsedOutputFolder)) {
                Settings.LastUsedOutputFolder = 
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }


            if (String.IsNullOrEmpty(Settings.LastUsedMediaSetFolder)) {
                Settings.LastUsedMediaSetFolder =
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            if (String.IsNullOrEmpty(Settings.PluginFolders) || !Directory.Exists(Settings.PluginFolders)) {
                try {
                    Settings.PluginFolders = Path.Combine(LocalAppPath, "plugins");
                    Directory.CreateDirectory(Settings.PluginFolders);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
