using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using libconvendro.Persistence;
using libconvendro;
using convendro.Classes;

namespace convendro {
    internal enum ScreenState {
        New,
        Editing
    }
    public partial class frmPresetsEditor : Form {
        private PresetsFile presetfile = null;
        private DescriptionFile descriptionfile = new DescriptionFile();
        private bool modified = false;
        private Preset currentpreset = null;
        private ToolTip tooltip = null;

        /// <summary>
        /// 
        /// </summary>
        public frmPresetsEditor() {
            InitializeComponent();
            tooltip = new ToolTip();
            tooltip.SetToolTip(btnNew, "New Preset");
            tooltip.SetToolTip(btnAdd, "Save Preset");
            tooltip.SetToolTip(btnRemove, "Remove Preset");
            tooltip.SetToolTip(btnFolderExplore, "Select Foldername");
            LoadDescriptionSettings(Config.Settings.LastUsedCommandDescriptionFile);
        }

        public frmPresetsEditor(PresetsFile afile) : this() {           
            presetfile = afile;
            this.RefreshData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acategory"></param>
        private void addPresetCategory(string acategory) {
            if (cboPresetCategory.Items.IndexOf(acategory) == -1) {
                cboPresetCategory.Items.Add(acategory);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void loadPresetsData() {
            cboPresetCategory.Items.Clear();
            cboPresetname.Items.Clear();
            
            if (presetfile != null) {
                foreach (Preset p in presetfile.Presets) {
                    cboPresetname.Items.Add(p.Name);
                    // no duplicates... slow...        
                    addPresetCategory(p.Category);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearScreen() {
            cboPresetCategory.Text = "";
            cboPresetname.Text = "";
            txtDescription.Text = "";
            txtDirectory.Text = "";
            cboFileExtension.Text = "";
            datagridArguments.Rows.Clear();
            this.modified = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dontclearkey"></param>
        public void ClearScreen(bool dontclearkey) {
            if (!dontclearkey) {
                cboPresetname.Text = "";
            }
            cboPresetCategory.Text = "";
            txtDescription.Text = "";
            txtDirectory.Text = "";
            cboFileExtension.Text = "";
            datagridArguments.Rows.Clear();
            this.modified = false;

        }

        /// <summary>
        /// Need to be refactored (see the secondary
        /// overload...)
        /// </summary>
        /// <param name="anindex"></param>
        private void showPreset(int anindex) {
            if (anindex > -1) {
                Preset p = presetfile.Presets[anindex];

                if (p != null) {
                    cboPresetname.Text = p.Name;
                    cboPresetCategory.Text = p.Category;
                    txtDescription.Text = p.Description;

                    if (Directory.Exists(p.OutputFolder)) {                        
                        txtDirectory.Text = p.OutputFolder;
                    } else {
                        txtDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    }

                    cboFileExtension.Text = p.Extension;
                    
                    foreach (CommandOption co in p.CommandLineOptions.Items) {
                        int i = datagridArguments.Rows.Add();

                        if (colName.Items.IndexOf(co.Name) == -1) {
                            colName.Items.Add(co.Name);
                        }

                        datagridArguments.Rows[i].Cells[0].Value = co.Name;
                        datagridArguments.Rows[i].Cells[1].Value = co.Value;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        private void showPreset(string name) {
            Preset p = presetfile.FindPreset(name);
            // set currentpreset...

            if (p != null) {
                cboPresetCategory.Text = p.Category;
                cboPresetname.Text = p.Name;
                txtDescription.Text = p.Description;

                if (Directory.Exists(p.OutputFolder)) {
                    txtDirectory.Text = p.OutputFolder;
                } else {
                    txtDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                cboFileExtension.Text = p.Extension;


                foreach (CommandOption co in p.CommandLineOptions.Items) {
                    int i = datagridArguments.Rows.Add();

                    if (colName.Items.IndexOf(co.Name) == -1) {
                        colName.Items.Add(co.Name);
                    }

                    datagridArguments.Rows[i].Cells[0].Value = co.Name;
                    datagridArguments.Rows[i].Cells[1].Value = co.Value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshData() {
            loadPresetsData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presetdata"></param>
        public void AddPreset(Preset presetdata) {
            presetfile.AddPreset(presetdata);
            presetfile.Sort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presetdata"></param>
        public void RemovePreset(Preset presetdata) {
            presetfile.RemovePreset(presetdata);
        }

        /// <summary>
        /// 
        /// </summary>
        public PresetsFile Presets {
            get { return presetfile; }
            set { 
                presetfile = value;
                this.loadPresetsData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Preset CurrentPreset {
            get { return this.currentpreset; }
            set { this.currentpreset = value; }
        }


        
        private void cboPresetname_TextUpdate(object sender, EventArgs e) {
            this.modified = true;

        }

        private void cboPresetCategory_TextUpdate(object sender, EventArgs e) {
            this.modified = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e) {
            this.ClearScreen();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Preset buildPreset() {
            Preset pre = null;

            if (this.cboPresetname.Text != "") {                
                pre = new Preset();
                pre.Name = this.cboPresetname.Text;
                pre.Category = this.cboPresetCategory.Text;
                pre.Description = this.txtDescription.Text;
                pre.Extension = this.cboFileExtension.Text;

                if (this.txtDirectory.Text != "") {
                    if (this.txtDirectory.Text[this.txtDirectory.Text.Length -1] != '\\') {
                        pre.OutputFolder = this.txtDirectory.Text + "\\";
                    } else {
                        pre.OutputFolder = this.txtDirectory.Text;
                    }
                } else {
                    pre.OutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";
                }
                
                CommandLineOptions preoptions = new CommandLineOptions();

                // get the commandlineoptions...
                foreach (DataGridViewRow r in this.datagridArguments.Rows) {

                    string stt = "";

                    if (r.Cells[1].Value != null) {
                        stt = r.Cells[1].Value.ToString();
                    }

                    if (r.Cells[0].Value != null) {
                        CommandOption opt = new CommandOption(                        
                            r.Cells[0].Value.ToString(), stt);
                        preoptions.Add(opt);
                    }
                }

                pre.CommandLineOptions = preoptions;
            }

            return pre;
        }

        /// <summary>
        /// 
        /// </summary>
        private void cloneCurrentPreset(){
            this.currentpreset = this.buildPreset();

            if (this.currentpreset != null) {
                int i = this.presetfile.FindPresetIndex(this.currentpreset.Name);
                
                if (i > -1) {
                    this.currentpreset.Name = String.Format("Copy of {0}", this.currentpreset.Name);
                }

                this.presetfile.AddPreset(this.currentpreset);
                this.RefreshData();
            }
        }

        /// <summary>
        /// Saves the current (edited) preset
        /// </summary>
        private void saveCurrentPreset() {
            this.currentpreset = this.buildPreset();

            if (this.currentpreset != null) {
                int i = this.presetfile.FindPresetIndex(this.currentpreset);

                if (i == -1) {
                    this.presetfile.AddPreset(this.currentpreset);
                    this.RefreshData();
                } else {
                    this.presetfile.Presets[i] = this.currentpreset;
                }

                this.presetfile.Sort();

                this.ClearScreen();
                this.showPreset(this.currentpreset.Name);
            } else {
                this.ClearScreen();
                this.showPreset(0);
            }

        }


        /// <summary>
        /// Adds a new preset or clones the current one (SHIFT key).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e) {
            int state = Convert.ToInt32(Functions.GetAsyncKeyState(
                Keys.ShiftKey).ToString());

            if (state == Functions.KEY_PRESSED) {
                this.cloneCurrentPreset();
            } else {
                this.saveCurrentPreset();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e) {
            if (currentpreset != null) {
                this.presetfile.RemovePreset(currentpreset);
                this.RefreshData();
                this.ClearScreen();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagridArguments_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            if (this.datagridArguments.CurrentCellAddress.X == colName.DisplayIndex) {
                ComboBox cb = e.Control as ComboBox;

                if (cb != null) {
                    cb.DropDownStyle = ComboBoxStyle.DropDown;

                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagridArguments_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
            if (e.ColumnIndex == colName.DisplayIndex) {
                
                string s = (string)e.FormattedValue;

                if (!String.IsNullOrEmpty(s)) {
                    if (!this.colName.Items.Contains(e.FormattedValue)) {

                        this.colName.Items.Add(e.FormattedValue);

                        datagridArguments.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.FormattedValue;
                    }
                }                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboPresetname_SelectionChangeCommitted(object sender, EventArgs e) {
            if (this.cboPresetname.SelectedIndex > -1) {
                this.ClearScreen();
                this.showPreset(this.cboPresetname.SelectedIndex);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void LoadDescriptionSettings(string filename) {
            colName.Items.Clear();
            if (File.Exists(filename)) {
                descriptionfile = Functions.DeserializeDescriptionFile(filename);
                if (descriptionfile != null) {
                    foreach (DescriptionItem item in descriptionfile.Items) {
                        colName.Sorted = true;
                        colName.Items.Add(item.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the current descriptions of the commandline arguments
        /// </summary>
        /// <param name="filename"></param>
        public void SaveDescriptionSettings(string filename) {
            descriptionfile.Assign(this.colName.Items);
            Functions.SerializeDescriptionFile(filename, descriptionfile);
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pasteCommandlineToolStripMenuItem_Click(object sender, EventArgs e) {
            // this is going to be a motherload of a string...
            string b = Clipboard.GetText();

            if (!String.IsNullOrEmpty(b)) {
                string[] a = b.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

                if (a.Length > 0) {
                    datagridArguments.Rows.Clear();

                    foreach (string l in a) {
                        string d = l.Trim();
                        if (d.Contains(" ")) {
                            string[] t = d.Split(new char[]{' '});
                            if (t.Length == 2) {
                                int irow = datagridArguments.Rows.Add();

                                if (colName.Items.IndexOf(t[0]) == -1) {
                                    colName.Items.Add(t[0]);
                                }

                                datagridArguments.Rows[irow].Cells[0].Value = t[0];
                                datagridArguments.Rows[irow].Cells[1].Value = t[1];                                
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFolderExplore_Click(object sender, EventArgs e) {
            FolderBrowserDialog fold = new FolderBrowserDialog();
            try {
                if (fold.ShowDialog() == DialogResult.OK) {
                    txtDirectory.Text = fold.SelectedPath;
                }
            } finally {
                fold.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagridArguments_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            string s = datagridArguments[e.ColumnIndex, e.RowIndex].Value.ToString();

            this.colName.Items.Add(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.datagridArguments.SelectedRows.Count > 0) {
                foreach(DataGridViewRow r in this.datagridArguments.SelectedRows) {
                    this.datagridArguments.Rows.Remove(r);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboPresetname_SelectedIndexChanged(object sender, EventArgs e) {
            this.currentpreset = presetfile.FindPreset(this.cboPresetname.Text);
            if (this.currentpreset != null) {
                this.ClearScreen(true);
                this.showPreset(this.cboPresetname.Text);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void setModifiedControls() {
            btnAdd.Enabled = this.modified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyCommandLineToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.currentpreset != null) {
                string s = this.currentpreset.CommandLineOptions.BuildCommandLine();
                if (!String.IsNullOrEmpty(s)) {
                    Clipboard.SetText(s);
                }
            }
        }

    }
}
