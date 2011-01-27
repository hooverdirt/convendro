using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using libconvendro.Persistence;
using System.Reflection;

namespace libconvendro
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHELLEXECUTEINFO {
        public int cbSize;
        public uint fMask;
        public IntPtr hwnd;
        
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpVerb;
        
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpFile;
        
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpParameters;
        
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpDirectory;
        
        public int nShow;
        public IntPtr hInstApp;
        public IntPtr lpIDList;
    
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpClass;
        public IntPtr hkeyClass;
        public uint dwHotKey;
        public IntPtr hIcon;
        public IntPtr hProcess;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SizeStrings {
        Unknown = 0,
        B = 1,
        Kb = 2,
        Mb = 3,
        Gb = 4,
        Tb = 5,
        Uh = 6
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ProcessState {
        Unknown = 0,
        Working = 1,
        Error = 2,
        Success = 3,
        Warning = 4
    }

    public static class Functions {
        public const string FILENAME_EXECUTABLE_FFMPEG = "ffmpeg.exe";
        public const string FILENAME_EXECUTABLE_MENCODER = "mencoder.exe";
        public const string FORM_TERMINAL_TITLE = "Test Terminal";
        public const string FILENAME_PRESETSDATA = "presetsdata.xml";
        public const string FILENAME_COMMANDLINEDESCRIPTION = "cmddesc.xml";
        public const string MEDIAFILES_FILTER_TXT = "txt files (*.txt)|*.txt";

        public const string MEDIAFILES_FILTER_AVI = "avi files (*.avi)|*.avi";
        public const string MEDIAFILES_FILTER_MP4 = "mp4 files (*.mp4)|*.mp4";
        public const string MEDIAFILES_FILTER_M4V = "m4v files (*.m4v)|*.m4v";
        public const string MEDIAFILES_FILTER_WMV = "wmv files (*.wmv)|*.wmv";
        public const string MEDIAFILES_FILTER_MPG = "mpeg files (*.mpg)|*.mpg";
        public const string MEDIAFILES_FILTER_MPG2 = "mpeg2 files (*.mpeg)|*.mpeg";
        public const string MEDIAFILES_FILTER_FLV = "flash video files (*.flv)|*.flv";

        public const string MEDIAFILES_FILTER_MEDIA = "Media files|*.avi;*.mp4;*.m4v;*.wmv;*.mpg;*mpeg;*.flv";
        public const string MEDIAFILES_FILTER_ALL = "All files (*.*)|*.*";
        public const string MEDIAFILES_FILTER = MEDIAFILES_FILTER_AVI + "|" +
            MEDIAFILES_FILTER_FLV + "|" +
            MEDIAFILES_FILTER_MP4 + "|" + MEDIAFILES_FILTER_M4V + "|" +
            MEDIAFILES_FILTER_WMV + "|" + MEDIAFILES_FILTER_MPG + "|" +
            MEDIAFILES_FILTER_MPG2 + "|" + MEDIAFILES_FILTER_MEDIA + "|"
            + MEDIAFILES_FILTER_ALL;
        public const string MEDIAFILES_FILTER_XML = "xml files (*.xml)|*.xml";
        public const string MEDIAFILES_FILTER_XSET = "mediaset files (*.xset)|*.xset";
        public const string TIMEFORMAT_HHMMSS = "{0:D2}:{1:D2}:{2:D2}";
        public const string WINFF_NODE_LABEL = "label";
        public const string WINFF_NODE_PARAMS = "params";
        public const string WINFF_NODE_EXTENSION = "extension";
        public const string WINFF_NODE_CATEGORY = "category";

        // IConvendroHost/mainFomr constants.
        public const int SUBCOL_FILENAME = 0;
        public const int SUBCOL_PATH = 1;
        public const int SUBCOL_SIZE = 2;
        public const int SUBCOL_PRESETNAME = 3;
        public const int SUBCOL_DURATION = 4;
        public const int SUBCOL_STARTED = 5;
        public const int SUBCOL_FINISHED = 6;

        // Shell32 constants...
        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;

        // Key constants
        public const int KEY_PRESSED = -32767;



        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        internal static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        /// <summary>
        /// Shows the property Shell window
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool ShowPropertiesWindow(string filename) {

            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = filename;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            return ShellExecuteEx(ref info);
        }

        /// <summary>
        /// Creates a (numbered) backup file.
        /// </summary>
        /// <param name="afilename"></param>
        /// <returns></returns>
        public static bool CreateBackupFile(string afilename) {
            bool res = false;
            try {
                string file = Path.GetFileNameWithoutExtension(afilename);
                string path = Path.GetDirectoryName(afilename);

                int i = 1;

                string newfilename = Path.Combine(path, file + i.ToString() + ".bak");
                while (File.Exists(newfilename)) {
                    i++;
                    newfilename = Path.Combine(path, file + i.ToString() + ".bak");
                }

                File.Copy(afilename, newfilename);
                res = true;
            } catch {
                res = false;
            }

            return res;
        }

        /// <summary>
        /// Restores last backup file on creation date
        /// </summary>
        /// <returns></returns>
        public static bool RestoreBackupFile(string filename) {
            bool res = false;
            DateTime comparedatetime = DateTime.MinValue;
            FileInfo savedinfo = null;

            string actfile = Path.GetFileNameWithoutExtension(filename);
            string actpath = Path.GetDirectoryName(filename);

            string[] files = Directory.GetFiles(actpath, actfile + "*.bak", SearchOption.TopDirectoryOnly);

            if (files.Length > 0) {

                foreach (string afile in files) {

                    FileInfo info = new FileInfo(afile);

                    if (comparedatetime.CompareTo(info.CreationTime) < 0) {
                        savedinfo = info;
                        comparedatetime = info.CreationTime;
                    }
                    Application.DoEvents();
                }
            }

            if (savedinfo != null) {
                try {
                    File.Copy(savedinfo.FullName, filename, true);
                    res = true;
                } catch (Exception ex) {
                    res = false;
                }
                
            }

            return res;
        }

        /// <summary>
        /// Cleanup backup files.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static int CleanupBackupFiles(string path, string mask) {
            int res = 0;
            if (Directory.Exists(path)) {
                string[] files = Directory.GetFiles(path, mask);
                foreach (string file in files) {
                    // We may need to do something with dates..
                    FileInfo fileinfo = new FileInfo(file);                   
                    fileinfo.Delete();
                }
            }
            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandstring"></param>
        /// <param name="paramsplit"></param>
        /// <param name="paramvaluesplit"></param>
        /// <returns></returns>
        public static ArrayList BreakUpCommandline(string commandstring, string[] paramsplit, 
            string paramvaluesplit) {
            
            ArrayList res = null;

            if (!String.IsNullOrEmpty(commandstring)) {
                string[] arguments = commandstring.Split(paramsplit, StringSplitOptions.RemoveEmptyEntries);

                if (arguments.Length > 0) {

                    foreach (string s in arguments) {
                        string cld = s.Trim();

                        if (cld.Contains(paramvaluesplit)) {
                            string[] param = cld.Split(new string[] {paramvaluesplit}, StringSplitOptions.RemoveEmptyEntries);

                            if (param.Length > 0) {
                                string[] sarr = new string[]{param[0], (param.Length > 1 ? param[1] : "" )};
                                res.Add(sarr);
                            }
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Opens up a folder in File Explorer
        /// </summary>
        /// <param name="foldername"></param>
        /// <returns></returns>
        public static bool ShowFolderExplorer(string foldername) {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "open";
            info.lpFile = foldername;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            return ShellExecuteEx(ref info);
        }

        /// <summary>
        /// Converts a filesize to its proper string representation
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ConvertFileSizeToString(long size) {
            string res = "";
            string formatstr = "{0:0.00} {1}";
            SizeStrings sz = SizeStrings.Unknown;

            float i = 0F;
            try {
                long lasti = size;
                float templasti = 0F;
                do {
                    i = templasti;
                    templasti = (float)lasti / 1024f;
                    lasti = (int)templasti;
                    sz++;
                } while (lasti > 0);

            } catch {
                i = 0F;
                sz = SizeStrings.B;
            } finally {
                res = String.Format(formatstr, i, sz);
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static float TotalStringToSeconds(string duration) {
            float res = 0.00F;
            // 01234567890
            // ##:##:##.00
            string s = duration.Trim();
            res = (float) (Convert.ToDouble(s.Substring(0, 2)) * 60 * 60) +
                (float) (Convert.ToDouble(s.Substring(3, 2)) * 60) +
                (float) (Convert.ToDouble(s.Substring(6, 2)) +
                (float) (Convert.ToDouble("00." + s.Substring(9, 2))));
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="durationstring"></param>
        /// <returns></returns>
        public static string ExtractDuration(string durationstring) {
            string s = "";
            s = durationstring.Replace("Duration:", "  ");
            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestring"></param>
        /// <returns></returns>
        public static string ExtractTime(string timestring) {
            string s = "";
            if (timestring.Contains("time=")) {
                if (timestring.Contains("bitrate")) {
                    int s1 = timestring.IndexOf("time=");
                    int s2 = timestring.IndexOf("bitrate=");
                    s = timestring.Substring(s1 + 5, s2 - (s1+5));
                }
            }
            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestring"></param>
        /// <returns></returns>
        public static float CurrentStringToSeconds(string timestring) {
            float res = 0.00F;

            string s = timestring.Trim();

            res = (float)Convert.ToDouble(s);
            return res;
        }

        /// <summary>
        /// Combines the current path with the filename.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string CombineCurrentFilePath(string filename) {
            return Path.Combine(Application.StartupPath, filename);
        }

        /// <summary>
        /// Gets the currentLocalAppPath (c:\users\current_user\AppData\Local\Convendro)
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLocalAppPath() {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="presetdata"></param>
        /// <returns></returns>
        public static bool SerializePresetsData(string filename, PresetsFile presetdata) {
            bool res = true;

            XmlSerializer nser = new XmlSerializer(typeof(PresetsFile));
            TextWriter ntext = new StreamWriter(filename);
            try {
                nser.Serialize(ntext, presetdata);
                ntext.Flush();
            } catch {
                res = false;
            } finally {
                ntext.Dispose();
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static PresetsFile DeserializePresetsData(string filename) {
            PresetsFile pres = null;

            // Open up the file...
            if (File.Exists(filename)) {
                XmlSerializer nser = new XmlSerializer(typeof(PresetsFile));
                TextReader ntext = new StreamReader(filename);
                try {
                    pres = (PresetsFile)nser.Deserialize(ntext);
                } catch {
                    pres = null;
                } finally {
                    ntext.Close();
                    ntext.Dispose();
                }
            }

            return pres;
        }

        /// <summary>
        /// Fold these in generic...
        /// Note: Complete presets are stored in the MediaFileList, in case these need to be
        /// restored on a third party-computer. "Take your Queue to where-ever you want"
        /// </summary>
        /// <param name="afilename"></param>
        /// <param name="filelist"></param>
        /// <returns></returns>
        public static bool SerializeMediaFileList(string afilename, MediaFileList filelist) {
            bool res = false;

            XmlSerializer nser = new XmlSerializer(typeof(MediaFileList));
            TextWriter ntext = new StreamWriter(afilename);
            try {
                nser.Serialize(ntext, filelist);
                ntext.Flush();
                res = true;
            } catch {
                res = false;
            } finally {
                ntext.Dispose();
            }

            return res;
        }

        /// <summary>
        /// Fold these in a generic.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static MediaFileList DeserializeMediaFileList(string filename) {
            MediaFileList desc = null;

            if (File.Exists(filename)) {
                XmlSerializer nser = new XmlSerializer(typeof(MediaFileList));
                TextReader ntext = new StreamReader(filename);
                try {
                    desc = (MediaFileList)nser.Deserialize(ntext);
                } finally {
                    ntext.Close();
                    ntext.Dispose();
                }
            }

            return desc;
        }



        /// <summary>
        /// Fold these in a generic...
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="descriptionfile"></param>
        /// <returns></returns>
        public static bool SerializeDescriptionFile(string filename, DescriptionFile descriptionfile) {
            bool b = false;

            XmlSerializer nser = new XmlSerializer(typeof(DescriptionFile));
            TextWriter ntext = new StreamWriter(filename);
            try {
                nser.Serialize(ntext, descriptionfile);
                ntext.Flush();
                b = true;
            } catch {
                b = false;
            } finally {
                ntext.Dispose();
            }

            return b;
        }

        /// <summary>
        /// Fold these in a generic
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static DescriptionFile DeserializeDescriptionFile(string filename) {
            DescriptionFile desc = null;

            if (File.Exists(filename)) {
                XmlSerializer nser = new XmlSerializer(typeof(DescriptionFile));
                TextReader ntext = new StreamReader(filename);
                try {
                    desc = (DescriptionFile)nser.Deserialize(ntext);
                } finally {
                    ntext.Close();
                    ntext.Dispose();
                }
            }

            return desc;
        }

        /// <summary>
        /// Returns the FileVersionInfo of an executable or DLL.
        /// </summary>
        /// <param name="anexecutablename"></param>
        /// <returns></returns>
        public static FileVersionInfo GetFileVersionInfo(string anexecutablename) {
            return FileVersionInfo.GetVersionInfo(anexecutablename);
        }
    }     
}