using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using convendro.Classes;
using libconvendro;
using libconvendro.Threading;
using libconvendro.Persistence;
using libconvendro.Comparers;
using libconvendro.Import;
using libconvendro.Dialogs;
using libconvendro.Plugins;
using libconvendro.Forms;

namespace convendro {
    public partial class frmMain : Form, IThreadingHost, IConvendroHost {
        private PresetsFile presetdata = null;
        private MediaFileList mediafilelist = new MediaFileList();
        private bool newPresetFile = false;
        private FFMPEGConverter ffmpegconverter = null;
        private ManualResetEvent stopThreadEvent = new ManualResetEvent(false);
        private ManualResetEvent threadHasStoppedEvent = new ManualResetEvent(false);
        private PluginManager pluginManager = null;

        public frmMain() {
            InitializeComponent();
        }

        public TextBox LogBox {
            get { return this.textLogger; }
            set { this.textLogger = value; }
        }

        public Label TotalTime {
            get { return this.labelTotal; }
            set { this.labelTotal = value; }
        }

        public Label CurrentTime {
            get { return this.labelCurrent; }
            set { this.labelCurrent = value; }
        }

        public StatusStrip Statusbar {
            get { return this.mainStatusStrip; }
            set { this.mainStatusStrip = value; }
        }

        public ToolStripProgressBar FileProgress {
            get { return this.progressBarMain; }
            set { this.progressBarMain = value; }
        }

        public ListView FileListView {
            get { return this.listViewFiles; }
            set { this.listViewFiles = value; }
        }

        public PresetsFile PresetsFile {
            get { return this.presetdata; }
            set { this.presetdata = value; }
        }

        /// <summary>
        /// Indicates if this is the first time a preset
        /// file has been generated.
        /// </summary>
        public bool HasNewPresetFile {
            get { return this.newPresetFile; }
        }

        /// <summary>
        /// Verify if thread is running or not...
        /// </summary>
        /// <returns></returns>
        public bool IsThreadRunning() {
            bool ret = false;

            // Check if thread is active..
            if (this.ffmpegconverter != null) {
                if (this.ffmpegconverter.CurrentThread.IsAlive) {
                    ret = true;
                }
            }

            return ret;
        }

        private void listViewFiles_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent("FileDrop")) {
                // check if thread is still running...
                e.Effect = (!IsThreadRunning() ? DragDropEffects.Copy : DragDropEffects.None);
            }
        }

        private void listViewFiles_DragDrop(object sender, DragEventArgs e) {
            string[] files = (string[])e.Data.GetData("FileDrop");
            for (int x = 0; x < files.Length; x++) {
                if (File.Exists(files[x])) {
                    AddFile(files[x]);
                }
            }
            SetControlsThreading(true);
            updateStatusBar1();
        }

        /// <summary>
        /// Adds a file to the filelistview...
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private long AddFile(string p) {
            FileInfo n = new FileInfo(p);
            long ftotal = 0;
            if (n != null) {
                ListViewItem nitem = new ListViewItem();
                nitem.ImageIndex = 0;
                nitem.Text = n.Name;
                nitem.SubItems.Add(Path.GetDirectoryName(n.FullName));
                ftotal = n.Length;
                string s = Functions.ConvertFileSizeToString(n.Length);
                nitem.SubItems.Add(s);
                nitem.SubItems.Add("");
                nitem.SubItems.Add("");
                nitem.SubItems.Add("");
                nitem.SubItems.Add("");
                listViewFiles.Items.Add(nitem);
            }
            return ftotal;
        }

        /// <summary>
        /// 
        /// </summary>
        private void updateStatusBar1() {
            lblFilesBarMain.Text = String.Format("Files: {0}",
                listViewFiles.Items.Count);
        }

        /// <summary>
        /// refreshes the preset menu...
        /// </summary>
        private void refreshPresetMenu() {
            // reset the menus...
            int con = mediafilesPresetsToolStripMenuItem.DropDownItems.IndexOf(
                mediafileSelectPresetListToolStripMenuItem);

            // context menu...
            int con1 = presetToolStripMenuItem.DropDownItems.IndexOf(
                selectPresetToolStripMenuItem);

            if (con != 0) {
                // delete everything
                int count = mediafileSelectPresetListToolStripMenuItem.DropDownItems.Count;

                while (count > 1) {
                    mediafilesPresetsToolStripMenuItem.DropDownItems.RemoveAt(0);
                    count = mediafilesPresetsToolStripMenuItem.DropDownItems.Count;
                }
            }

            // remove contexmenuitems...
            if (con != 0) {
                int count = presetToolStripMenuItem.DropDownItems.Count;
                while (count > 1) {
                    presetToolStripMenuItem.DropDownItems.RemoveAt(0);
                    count = presetToolStripMenuItem.DropDownItems.Count;
                }
            }

            
            if (presetdata.Presets.Count > 0) {
                mediafilesPresetsToolStripMenuItem.DropDownItems.Insert(0, new ToolStripSeparator());
                presetToolStripMenuItem.DropDownItems.Insert(0, new ToolStripSeparator());
                // sort the data...
                PresetsUsedCountSorter p = new PresetsUsedCountSorter();
                try {
                    p.Reverse = true;
                    this.presetdata.Sort(p);
                    int x = 0;

                    while (x < 5 && x < this.presetdata.Presets.Count) {
                        Preset npreset = this.presetdata.Presets[x];
                        ToolStripMenuItem menuitem = new ToolStripMenuItem();

                        menuitem.Text = npreset.Name;
                        menuitem.ToolTipText = npreset.Description;
                        menuitem.Click += new EventHandler(dynamicPresetMenuItem_Click);
                        menuitem.ShortcutKeys = (Keys.Control | Keys.Alt |
                            ((Keys)Enum.Parse(typeof(Keys), "D" + x.ToString())));
                        mediafilesPresetsToolStripMenuItem.DropDownItems.Insert(0, menuitem);

                        // contextmenu
                        ToolStripMenuItem contextmenuitem = new ToolStripMenuItem();
                        contextmenuitem.Text = npreset.Name;
                        contextmenuitem.ToolTipText = npreset.Description;
                        contextmenuitem.Click += new EventHandler(dynamicPresetMenuItem_Click);
                        contextmenuitem.ShortcutKeys = (Keys.Control | Keys.Alt |
                            ((Keys)Enum.Parse(typeof(Keys), "D" + x.ToString())));
                        presetToolStripMenuItem.DropDownItems.Insert(0, contextmenuitem);

                        x++;
                    }
                } finally {
                    this.presetdata.Sort();
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dynamicPresetMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem n = (sender as ToolStripMenuItem);

            if (n != null) {
                if (this.listViewFiles.SelectedItems.Count > 0) {
                    Preset npreset = this.presetdata.FindPreset(n.Text);

                    if (npreset != null) {
                        foreach (ListViewItem it in listViewFiles.SelectedItems) {
                            it.SubItems[Functions.SUBCOL_PRESETNAME].Text = npreset.Name;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void setUpToolbars() {
            toolStripContainer1.TopToolStripPanel.Join(pluginsToolStrip);
            toolStripContainer1.TopToolStripPanel.Join(toolsToolStrip);
            toolStripContainer1.TopToolStripPanel.Join(conversionToolStrip);
            toolStripContainer1.TopToolStripPanel.Join(fileToolStrip);
            toolStripContainer1.TopToolStripPanel.Join(mainMenuStrip);
        }

        /// <summary>
        /// Main Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e) {
            // Load File Folder settings.
            Config.LoadFileFolderSettings();


            // Load Form settings...
            Config.LoadFormSettings(this);
            
            pluginManager = new PluginManager(Config.Settings.PluginFolders);
            pluginManager.OnPluginLoad += new PluginLoadedEvent(pluginManager_OnPluginLoad);
            pluginManager.Load();


            // Prepare PresetFile
            if (String.IsNullOrEmpty(Config.Settings.LastUsedPresetFile)) {
                this.presetdata = null;
                Config.Settings.LastUsedPresetFile = Path.Combine(Functions.GetCurrentLocalAppPath(),
                    Functions.FILENAME_PRESETSDATA);
            } else {
                this.presetdata = Functions.DeserializePresetsData(
                    Config.Settings.LastUsedPresetFile);
            }

            // 
            if (String.IsNullOrEmpty(Config.Settings.LastUsedCommandDescriptionFile)) {
                Config.Settings.LastUsedCommandDescriptionFile =
                    Path.Combine(Functions.GetCurrentLocalAppPath(), Functions.FILENAME_COMMANDLINEDESCRIPTION);
            }

            // Create the file automatically...

            if (this.presetdata == null) {
                this.newPresetFile = true;
                this.presetdata = new PresetsFile();
            }

            refreshPresetMenu();
            setUpToolbars();
            SetControlsThreading(true);

            // If FFMEPG doesn't exist, we should probably show a dialog...
            if (String.IsNullOrEmpty(Config.Settings.FFMPEGFilePath)) {
                MessageBox.Show("FFMPeg was not found " + Config.Settings.FFMPEGFilePath + ". You may wish to set this in the settings", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Sets up the plugins UI actions and controls.
        /// </summary>
        /// <param name="anobject"></param>
        /// <param name="plugin"></param>
        private void pluginManager_OnPluginLoad(object anobject, IConvendroPlugin plugin) {
            // set the host..
            plugin.Host = this;
            // prepare the plugin
            this.setupPluginMainMenu(plugin);
            this.setupPluginToolBar(plugin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        private void setupPluginMainMenu(IConvendroPlugin plugin) {
            // use default bitmap where possible.
            Bitmap defaultimage = plugin.MenuBitmap;

            if (plugin.MenuBitmap == null) {
                defaultimage = Properties.Resources.plugin;
            }

            ToolStripMenuItem nitem = new ToolStripMenuItem(plugin.Description,
                plugin.MenuBitmap);
            try {
                if (toolsPluginsToolStripMenuItem.DropDownItems.Count == 1) {
                    toolsPluginsToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
                }

                toolsPluginsToolStripMenuItem.DropDownItems.Add(nitem);
                nitem.Tag = plugin.Guid;
                nitem.Text = plugin.Caption;
                nitem.Click += new EventHandler(pluginMenuItem_Click);
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        private void setupPluginToolBar(IConvendroPlugin plugin) {
            Bitmap defaultimage = plugin.MenuBitmap;

            if (plugin.MenuBitmap == null) {
                defaultimage = Properties.Resources.plugin;
            }

            ToolStripButton tsbutton = new ToolStripButton(defaultimage);
            try {
                if (pluginsToolStrip.Items.Count == 1) {
                    pluginsToolStrip.Items.Add(new ToolStripSeparator());
                }
                pluginsToolStrip.Items.Add(tsbutton);
                tsbutton.Tag = plugin.Guid;
                tsbutton.ToolTipText = plugin.Caption;
                tsbutton.Click += new EventHandler(pluginToolButtonItem_Click);

            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pluginToolButtonItem_Click(object sender, EventArgs e) {
            if (sender is ToolStripButton) {
                if ((sender as ToolStripButton).Tag != null) {
                    IConvendroPlugin plugin = this.pluginManager.FindPlugin(
                        (Guid)(sender as ToolStripButton).Tag);
                    if (plugin != null) {
                        plugin.Execute();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pluginMenuItem_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem) {
                if ((sender as ToolStripMenuItem).Tag != null) {
                    IConvendroPlugin plugin = this.pluginManager.FindPlugin(
                        (Guid)(sender as ToolStripMenuItem).Tag);
                    if (plugin != null) {
                        plugin.Execute();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolsPresetsEditorToolStripMenuItem_Click(object sender, EventArgs e) {
            frmPresetsEditor nform = new frmPresetsEditor(this.presetdata);

            nform.StartPosition = FormStartPosition.CenterParent;

            DialogResult res = nform.ShowDialog();
            try {
                if (res == DialogResult.OK) {
                    // Save Presetfile.
                    if (Config.Settings.MakeBackupsXMLFiles) {
                        Functions.CreateBackupFile(Config.Settings.LastUsedPresetFile);
                    }

                    bool b = Functions.SerializePresetsData(
                        Config.Settings.LastUsedPresetFile,
                        this.presetdata);

                    if (b == false && Config.Settings.MakeBackupsXMLFiles) {
                        // I could throw it but...
                        DialogResult a = MessageBox.Show("Do you want your stuff restored?", "Convendro", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (a == DialogResult.Yes) {
                            // if backup file was loaded...
                            if (Functions.RestoreBackupFile(Config.Settings.LastUsedPresetFile)) {
                                this.presetdata = Functions.DeserializePresetsData(
                                    Config.Settings.LastUsedPresetFile);
                            }
                        }
                    }

                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Convendro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                // Save commandline descriptions...
                nform.SaveDescriptionSettings(Config.Settings.LastUsedCommandDescriptionFile);
                nform.Dispose();
            }
        }

        /// <summary>
        /// Shows the about dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            frmAbout nabout = new frmAbout();
            try {
                nabout.ShowDialog();
            } finally {
                nabout.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileExitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        private void buildMediaFileList() {
            this.mediafilelist.Clear();
            foreach (ListViewItem n in listViewFiles.Items) {
                string filename = Path.Combine(n.SubItems[Functions.SUBCOL_PATH].Text, n.SubItems[Functions.SUBCOL_FILENAME].Text);
                Preset preset = presetdata.FindPreset(n.SubItems[Functions.SUBCOL_PRESETNAME].Text);
                
                // reset time stats.
                n.SubItems[Functions.SUBCOL_STARTED].Text = "";
                n.SubItems[Functions.SUBCOL_FINISHED].Text = "";
                n.SubItems[Functions.SUBCOL_DURATION].Text = "";

                if (preset != null) {
                    this.mediafilelist.AddMediaFile(filename, preset, n.Index);
                }

                Application.DoEvents();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void startThread() {
            buildMediaFileList();
            threadHasStoppedEvent.Reset();
            stopThreadEvent.Reset();
            SetControlsThreading(false);
            ffmpegconverter = new FFMPEGConverter(stopThreadEvent,
                threadHasStoppedEvent);
            try {
                ffmpegconverter.Form = this;
                ffmpegconverter.MediaFileItems = this.mediafilelist;
                ffmpegconverter.Executable = Config.Settings.FFMPEGFilePath;
                ffmpegconverter.Execute();
            } catch {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesStartConversionToolStripMenuItem_Click(object sender, EventArgs e) {
            startThread();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesStopConversionToolStripMenuItem_Click(object sender, EventArgs e) {
            stopThread();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesClearListToolStripMenuItem_Click(object sender, EventArgs e) {
            listViewFiles.Items.Clear();
            this.SetControlsThreading(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesAddListToolStripMenuItem_Click(object sender, EventArgs e) {
            // ToDo check threading...

            string filedir = (String.IsNullOrEmpty(
                        Config.Settings.LastUsedInputFolder) ?
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) :
                        Config.Settings.LastUsedInputFolder);


            OpenFileDialog openerdlg = new OpenFileDialog();
            try {
                openerdlg.Title = "Add file(s)";
                openerdlg.Filter = Functions.MEDIAFILES_FILTER;
                openerdlg.FilterIndex = Config.Settings.LastUsedMediaIndex;
                openerdlg.InitialDirectory = filedir;
                openerdlg.Multiselect = true;

                DialogResult res = openerdlg.ShowDialog();


                if (res == DialogResult.OK) {
                    long total = 0;
                    foreach (String s in openerdlg.FileNames) {
                        filedir = Path.GetFullPath(s);
                        total += AddFile(s);
                    }

                }
            } finally {
                this.SetControlsThreading(true);
                updateStatusBar1();
                Config.Settings.LastUsedMediaIndex = openerdlg.FilterIndex;
                Config.Settings.LastUsedInputFolder = filedir;
                openerdlg.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesDeleteListToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (ListViewItem n in listViewFiles.SelectedItems) {
                n.Remove();
            }
            this.SetControlsThreading(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesSelectPresetListToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listViewFiles.SelectedItems.Count > 0) {
                frmPresetsEditor nform = new frmPresetsEditor(this.presetdata);

                nform.StartPosition = FormStartPosition.CenterParent;

                DialogResult res = nform.ShowDialog();
                try {
                    if (res == DialogResult.OK) {
                        if (nform.CurrentPreset != null) {
                            foreach (ListViewItem i in this.listViewFiles.SelectedItems) {
                                i.SubItems[Functions.SUBCOL_PRESETNAME].Text = nform.CurrentPreset.Name;
                                nform.CurrentPreset.LastUsed = DateTime.Now;
                                nform.CurrentPreset.UsedCount += 1;
                            }
                        }

                        if (Config.Settings.MakeBackupsXMLFiles) {
                            Functions.CreateBackupFile(Config.Settings.LastUsedPresetFile);
                        }

                        // only save the presets but do not reload the presetdata object,
                        // as the user may wants to finish this session first.
                        Functions.SerializePresetsData(Config.Settings.LastUsedPresetFile, 
                            this.presetdata);
                    }
                } finally {
                    // Save commandline descriptions...
                    nform.SaveDescriptionSettings(
                        Config.Settings.LastUsedCommandDescriptionFile);
                    nform.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesPropertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listViewFiles.SelectedItems.Count > 0) {
                Functions.ShowPropertiesWindow(
                    Path.Combine(listViewFiles.SelectedItems[0].SubItems[Functions.SUBCOL_PATH].Text,
                    listViewFiles.SelectedItems[0].SubItems[Functions.SUBCOL_FILENAME].Text));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesMoveUpToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listViewFiles.SelectedItems.Count == 1) {
                if (listViewFiles.SelectedItems[0].Index > 0) {
                    ListViewItem i1 = listViewFiles.SelectedItems[0];
                    int ind = i1.Index;
                    listViewFiles.Items.RemoveAt(ind);
                    listViewFiles.Items.Insert(ind - 1, i1);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesMoveDownToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listViewFiles.SelectedItems.Count == 1) {
                if (listViewFiles.SelectedItems[0].Index < listViewFiles.Items.Count - 1) {
                    ListViewItem i1 = listViewFiles.SelectedItems[0];
                    int ind = i1.Index;
                    listViewFiles.Items.RemoveAt(ind);
                    listViewFiles.Items.Insert(ind + 1, i1);
                }
            }
        }

        /// <summary>
        /// stops the thread...
        /// </summary>
        private void stopThread() {
            if (ffmpegconverter != null) {
                if (ffmpegconverter.CurrentThread != null && ffmpegconverter.CurrentThread.IsAlive) {
                    stopThreadEvent.Set();

                    while (ffmpegconverter.CurrentThread.IsAlive) {
                        if (WaitHandle.WaitAll(new ManualResetEvent[] { threadHasStoppedEvent }, 100, true)) {
                            break;
                        }
                        Application.DoEvents();
                    }
                }
            }

            SetControlsThreading(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadfinished"></param>
        public void SetControlsThreading(bool threadfinished) {

            // ProgressBar
            progressBarMain.Visible = !threadfinished;

            // Play...
            mediafilesPlaytoolStripButton.Enabled = threadfinished
                && !String.IsNullOrEmpty(Config.Settings.FFMPEGFilePath)
                && listViewFiles.Items.Count > 0;
            conversionStartToolStripMenuItem.Enabled =
                mediafilesPlaytoolStripButton.Enabled;

            // Stop
            mediafilesStopToolStripButton.Enabled = !threadfinished;
            conversionStopToolStripMenuItem.Enabled = !threadfinished;

            // TestRun
            mediafilesTestRunToolStripMenuItem.Enabled =
                (listViewFiles.Items.Count > 0);
            mediafilesTestRunToolStripButton.Enabled =
                mediafilesTestRunToolStripMenuItem.Enabled;

            mediafilesClearListToolStripMenuItem.Enabled = threadfinished;
            mediafilesClearListToolStripButton.Enabled = threadfinished;

            mediafilesAddListToolStripMenuItem.Enabled = threadfinished;
            mediafilesAddListToolStripButton.Enabled = threadfinished;

            mediafilesDeleteListToolStripMenuItem.Enabled = threadfinished;
            mediafilesDeleteListToolStripButton.Enabled = threadfinished;

            mediafilesMoveDownToolStripButton.Enabled = threadfinished;
            mediafilesMoveDownToolStripMenuItem.Enabled = threadfinished;

            mediafilesMoveUpToolStripButton.Enabled = threadfinished;
            mediafilesMoveUpToolStripMenuIte.Enabled = threadfinished;


            progressBarMain.Value = 0;

            if (threadfinished) {
                lblStatusBarMain.Text = "";
            }

        }

        /// <summary>
        /// Loads a pre-existing mediafile/queue into the mediafile object.
        /// </summary>
        /// <param name="afilename"></param>
        public void LoadMediaFileList(string afilename) {
            if (File.Exists(afilename)) {
                try {

                    this.mediafilelist = Functions.DeserializeMediaFileList(afilename);

                    // Add new presets, where necessary...
                    if (mediafilelist != null) {
                        foreach (MediaFile f in mediafilelist.Items) {
                            Preset internalpreset = f.Preset;

                            if (this.presetdata.FindPresetIndex(internalpreset) == -1) {
                                this.presetdata.AddPreset(internalpreset);
                            }
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshFileListView() {
            this.listViewFiles.Items.Clear();
            // sort items?
            foreach (MediaFile f in this.mediafilelist.Items) {

                if (File.Exists(f.Filename)) {

                    FileInfo finfo = new FileInfo(f.Filename);

                    ListViewItem fitem = new ListViewItem();

                    fitem.Text = Path.GetFileName(f.Filename);
                    fitem.SubItems.Add(Path.GetDirectoryName(f.Filename));
                    fitem.SubItems.Add(Functions.ConvertFileSizeToString(finfo.Length));
                    fitem.SubItems.Add(f.Preset.Name);

                    fitem.SubItems.Add((f.DateStarted != DateTime.MinValue ?
                        String.Format(Functions.TIMEFORMAT_HHMMSS,
                        f.Duration.Hours, f.Duration.Minutes,
                        f.Duration.Seconds) :
                        ""));

                    fitem.SubItems.Add((f.DateStarted != DateTime.MinValue ?
                        String.Format(Functions.TIMEFORMAT_HHMMSS, f.DateStarted.Hour,
                        f.DateStarted.Minute, f.DateStarted.Second) : ""));

                    fitem.SubItems.Add((f.DateFinished != DateTime.MinValue ?
                        String.Format(Functions.TIMEFORMAT_HHMMSS, f.DateFinished.Hour,
                        f.DateFinished.Minute, f.DateFinished.Second) : ""));

                    fitem.ImageIndex = (int)ProcessState.Unknown;

                    if (f.Duration.Ticks > 0) {
                        fitem.ImageIndex = (int)ProcessState.Success;
                    }

                    listViewFiles.Items.Add(fitem);
                }
            }

            SetControlsThreading(true);
        }

        /// <summary>
        /// Saves the current mediafile to file.
        /// </summary>
        /// <param name="afilename"></param>
        public void SaveMediaFileList(string afilename) {
            try {
                Functions.SerializeMediaFileList(afilename, this.mediafilelist);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = false;

            if (ffmpegconverter != null) {
                if (ffmpegconverter.CurrentThread.IsAlive) {

                    bool b = Config.Settings.AlwaysConfirmStopConversion;

                    DialogResult cfrm = ConfirmOKCancelDialogBox.ShowDialog(Application.ProductName, "A conversion process is still active. Do you really want to quit?", MessageBoxIcon.Question,
                        ref b);
                                        
                    if (cfrm == DialogResult.OK) {
                        stopThread();
                    } else {
                        e.Cancel = true;
                    }

                    Config.Settings.AlwaysConfirmStopConversion = b;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editSelectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (ListViewItem i in listViewFiles.Items) {
                i.Selected = true;
                Application.DoEvents();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctxListView_Opening(object sender, CancelEventArgs e) {
            testRunToolStripMenuItem.Enabled =
                (listViewFiles.Items.Count > 0) &&
                (listViewFiles.SelectedItems.Count == 1) &&
                (listViewFiles.SelectedItems[0].SubItems[Functions.SUBCOL_PRESETNAME].Text != "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesTestRunToolStripMenuItem_Click(object sender, EventArgs e) {
            MediaFileList newlist = new MediaFileList();

            foreach (ListViewItem n in listViewFiles.SelectedItems) {
                string filename = Path.Combine(n.SubItems[Functions.SUBCOL_PATH].Text, n.SubItems[Functions.SUBCOL_FILENAME].Text);
                Preset preset = presetdata.FindPreset(n.SubItems[Functions.SUBCOL_PRESETNAME].Text);

                if (preset != null) {
                    newlist.AddMediaFile(filename, preset, n.Index);
                }
            }

            frmTerminal nterm = new frmTerminal(newlist,
                Config.Settings.FFMPEGFilePath);
            nterm.Show();
            nterm.StartProcessing();
        }

        /// <summary>
        /// Cancel current process dialog. If true, the process is cancelled:
        /// otherwise the user wants to continue stuff...
        /// </summary>
        /// <returns></returns>
        private bool stopProcessYesNoDialog() {            
            bool res = false;
            if (ffmpegconverter != null) {
                if (ffmpegconverter.CurrentThread.IsAlive) {
                    if (MessageBox.Show("A conversion process is still running: Do you really want to quit the application?",
                        Application.ProductName,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation) == DialogResult.Yes) {
                        res = true;
                    } else {
                        res = false;
                    }
                }
            }

            return res;
        }


        /// <summary>
        /// Generic import method that shows the OpenFileDialog.
        /// </summary>
        /// <param name="animporter"></param>
        private void importPresets(string caption, IImporter animporter) {
            OpenFileDialog nfile = new OpenFileDialog();
            nfile.Filter = String.Format("{0}|{1}",
                Functions.MEDIAFILES_FILTER_XML, Functions.MEDIAFILES_FILTER_ALL);
            nfile.Title = caption;
            try {
                if (nfile.ShowDialog() == DialogResult.OK) {
                    // Make a backup of the file...
                    if (Config.Settings.MakeBackupsXMLFiles) {
                        Functions.CreateBackupFile(Config.Settings.LastUsedPresetFile);
                    }

                    animporter.LoadFile(nfile.FileName);

                    if (animporter.Presets.Count > 0) {
                        int oldcount = this.presetdata.Presets.Count;
                        this.presetdata.AddPresets(animporter.Presets);
                        if (this.presetdata.Presets.Count > oldcount) {
                            // actually save the presets...
                            Functions.SerializePresetsData(
                                Config.Settings.LastUsedPresetFile,
                                this.presetdata);
                            MessageBox.Show(String.Format("{0} files were imported...",
                                (this.presetdata.Presets.Count - oldcount)));
                        }

                    }
                }
            } finally {
                nfile.Dispose();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileImportWinFFToolStripMenuItem_Click(object sender, EventArgs e) {
            this.importPresets("Import WinFF file", new WinFFFile());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileImportVideoraToolStripMenuItem_Click(object sender, EventArgs e) {
            this.importPresets("Import Videora file", new VideoraFile());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e) {
            // Do some post processing like saving data...
            Config.SaveSettings(this);
            Config.Settings.Save();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileLoadMediaSetToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog opener = new OpenFileDialog();
            try {
                opener.Title = "Open existing media set file";
                opener.Filter = Functions.MEDIAFILES_FILTER_XSET + "|" +
                    Functions.MEDIAFILES_FILTER_XML + "|" +
                    Functions.MEDIAFILES_FILTER_ALL;
                opener.InitialDirectory = Config.Settings.LastUsedMediaSetFolder;

                if (opener.ShowDialog() == DialogResult.OK) {
                    LoadMediaFileList(opener.FileName);
                    RefreshFileListView();
                    Config.Settings.LastUsedMediaSetFolder = Path.GetDirectoryName(opener.FileName);
                }
            } finally {
                opener.Dispose();
            }
        }

        private void fileSaveMediaSetToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog savefile = new SaveFileDialog();
            try {
                savefile.Title = "Save media set file";
                savefile.Filter = Functions.MEDIAFILES_FILTER_XSET + "|" +
                    Functions.MEDIAFILES_FILTER_XML + "|" +
                    Functions.MEDIAFILES_FILTER_ALL;
                savefile.InitialDirectory = Config.Settings.LastUsedMediaSetFolder;

                if (savefile.ShowDialog() == DialogResult.OK) {
                    if (this.mediafilelist.Count == 0) {
                        if (this.listViewFiles.Items.Count > 0) {
                            // build the file list...
                            buildMediaFileList();
                        }
                    }
                    SaveMediaFileList(savefile.FileName);
                    Config.Settings.LastUsedMediaSetFolder = Path.GetDirectoryName(savefile.FileName);
                }
            } finally {
                savefile.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesExploreSourceFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listViewFiles.SelectedItems.Count > 0) {
                foreach (ListViewItem i in listViewFiles.SelectedItems) {
                    Functions.ShowFolderExplorer(i.SubItems[Functions.SUBCOL_PATH].Text);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediafilesExploreTargetFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listViewFiles.SelectedItems.Count > 0) {
                foreach (ListViewItem i in listViewFiles.SelectedItems) {
                    if (!String.IsNullOrEmpty(i.SubItems[Functions.SUBCOL_PRESETNAME].Text)) {
                        Preset n = presetdata.FindPreset(i.SubItems[Functions.SUBCOL_PRESETNAME].Text);
                        if (n != null) {
                            Functions.ShowFolderExplorer(n.OutputFolder);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the mediafilelist
        /// </summary>
        public MediaFileList MediaFileList {
            get {
                return this.mediafilelist;
            }
        }

        /// <summary>
        /// Gets or sets the selected items in the listview.
        /// </summary>
        public int[] SelectedIndices {
            get {
                int[] array = null;
                Array.Resize(ref array, this.listViewFiles.SelectedIndices.Count);
                
                int i = 0;
                foreach (int index in listViewFiles.SelectedIndices) {
                    array[i] = index;
                    i++;
                }

                return array;
            }
            set {
                if ((value != null) && (value.Length > 0)) {
                    foreach (int i in value) {
                        this.listViewFiles.Items[i].Selected = true;
                    }
                } else {
                    this.listViewFiles.SelectedItems.Clear();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] SelectedItems {
            get {
                string[] array = null;

                Array.Resize(ref array, this.listViewFiles.SelectedItems.Count);

                for (int i = 0; i < this.listViewFiles.SelectedItems.Count; i++) {
                    array[i] = Path.Combine(
                        this.listViewFiles.SelectedItems[i].SubItems[Functions.SUBCOL_PATH].Text,
                        this.listViewFiles.SelectedItems[i].SubItems[Functions.SUBCOL_FILENAME].Text);
                }

                return array;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void AddMediaFile(string filename) {
            AddFile(filename);

            // prepare the userinterface
            SetControlsThreading(true);
            updateStatusBar1();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetPresetNameItem(int index) {
            string preset = null;

            if (listViewFiles.Items.Count > 0) {
                ListViewItem i = listViewFiles.Items[index];
                preset = i.SubItems[Functions.SUBCOL_PRESETNAME].Text;
            }

            return preset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="presetname"></param>
        public void SetPresetNameItem(int index, string presetname) {
            if (presetdata.FindPresetIndex(presetname) > -1) {
                if (listViewFiles.Items.Count > 0 && index >= 0
                    && index < listViewFiles.Items.Count) {
                    listViewFiles.Items[index].SubItems[Functions.SUBCOL_PRESETNAME].Text = presetname;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start() {
            startThread();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop() {
            stopThread();
        }
    }
}
