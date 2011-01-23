namespace convendro {
    partial class frmPresetsEditor {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPresetsEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboPresetCategory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboPresetname = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.datagridArguments = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctxArguments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteCommandlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboFileExtension = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFolderExplore = new System.Windows.Forms.Button();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.copyCommandLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridArguments)).BeginInit();
            this.ctxArguments.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnNew);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboPresetCategory);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboPresetname);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Preset:";
            // 
            // btnRemove
            // 
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.Location = new System.Drawing.Point(310, 17);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(25, 25);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(281, 17);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(25, 25);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNew
            // 
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.Location = new System.Drawing.Point(252, 17);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(25, 25);
            this.btnNew.TabIndex = 1;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cate&gory:";
            // 
            // cboPresetCategory
            // 
            this.cboPresetCategory.FormattingEnabled = true;
            this.cboPresetCategory.Location = new System.Drawing.Point(66, 50);
            this.cboPresetCategory.Name = "cboPresetCategory";
            this.cboPresetCategory.Size = new System.Drawing.Size(273, 21);
            this.cboPresetCategory.Sorted = true;
            this.cboPresetCategory.TabIndex = 5;
            this.cboPresetCategory.TextUpdate += new System.EventHandler(this.cboPresetCategory_TextUpdate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Name:";
            // 
            // cboPresetname
            // 
            this.cboPresetname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPresetname.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPresetname.FormattingEnabled = true;
            this.cboPresetname.Location = new System.Drawing.Point(66, 19);
            this.cboPresetname.Name = "cboPresetname";
            this.cboPresetname.Size = new System.Drawing.Size(180, 21);
            this.cboPresetname.Sorted = true;
            this.cboPresetname.TabIndex = 0;
            this.cboPresetname.SelectionChangeCommitted += new System.EventHandler(this.cboPresetname_SelectionChangeCommitted);
            this.cboPresetname.SelectedIndexChanged += new System.EventHandler(this.cboPresetname_SelectedIndexChanged);
            this.cboPresetname.TextUpdate += new System.EventHandler(this.cboPresetname_TextUpdate);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.datagridArguments);
            this.groupBox2.Location = new System.Drawing.Point(12, 253);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 129);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "&Arguments:";
            // 
            // datagridArguments
            // 
            this.datagridArguments.BackgroundColor = System.Drawing.SystemColors.Control;
            this.datagridArguments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.datagridArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridArguments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colValue});
            this.datagridArguments.ContextMenuStrip = this.ctxArguments;
            this.datagridArguments.Dock = System.Windows.Forms.DockStyle.Left;
            this.datagridArguments.Location = new System.Drawing.Point(3, 16);
            this.datagridArguments.Name = "datagridArguments";
            this.datagridArguments.Size = new System.Drawing.Size(336, 110);
            this.datagridArguments.TabIndex = 0;
            this.datagridArguments.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.datagridArguments_CellValidating);
            this.datagridArguments.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.datagridArguments_EditingControlShowing);
            this.datagridArguments.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.datagridArguments_DataError);
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            // 
            // colValue
            // 
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.Width = 150;
            // 
            // ctxArguments
            // 
            this.ctxArguments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteRowToolStripMenuItem,
            this.copyCommandLineToolStripMenuItem,
            this.pasteCommandlineToolStripMenuItem});
            this.ctxArguments.Name = "ctxArguments";
            this.ctxArguments.Size = new System.Drawing.Size(182, 92);
            // 
            // deleteRowToolStripMenuItem
            // 
            this.deleteRowToolStripMenuItem.Name = "deleteRowToolStripMenuItem";
            this.deleteRowToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.deleteRowToolStripMenuItem.Text = "Delete Row";
            this.deleteRowToolStripMenuItem.Click += new System.EventHandler(this.deleteRowToolStripMenuItem_Click);
            // 
            // pasteCommandlineToolStripMenuItem
            // 
            this.pasteCommandlineToolStripMenuItem.Name = "pasteCommandlineToolStripMenuItem";
            this.pasteCommandlineToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.pasteCommandlineToolStripMenuItem.Text = "Paste Commandline";
            this.pasteCommandlineToolStripMenuItem.Click += new System.EventHandler(this.pasteCommandlineToolStripMenuItem_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(99, 400);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(201, 400);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtDescription);
            this.groupBox3.Location = new System.Drawing.Point(12, 98);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(353, 67);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "&Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(3, 16);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(347, 48);
            this.txtDescription.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cboFileExtension);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.btnFolderExplore);
            this.groupBox4.Controls.Add(this.txtDirectory);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(12, 171);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(353, 76);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Output:";
            // 
            // cboFileExtension
            // 
            this.cboFileExtension.FormattingEnabled = true;
            this.cboFileExtension.Items.AddRange(new object[] {
            "mp4",
            "m4v",
            "mpg",
            "mpeg2"});
            this.cboFileExtension.Location = new System.Drawing.Point(70, 45);
            this.cboFileExtension.Name = "cboFileExtension";
            this.cboFileExtension.Size = new System.Drawing.Size(121, 21);
            this.cboFileExtension.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Extension:";
            // 
            // btnFolderExplore
            // 
            this.btnFolderExplore.Image = global::convendro.Properties.Resources.folder_explore;
            this.btnFolderExplore.Location = new System.Drawing.Point(314, 10);
            this.btnFolderExplore.Name = "btnFolderExplore";
            this.btnFolderExplore.Size = new System.Drawing.Size(25, 25);
            this.btnFolderExplore.TabIndex = 5;
            this.btnFolderExplore.UseVisualStyleBackColor = true;
            this.btnFolderExplore.Click += new System.EventHandler(this.btnFolderExplore_Click);
            // 
            // txtDirectory
            // 
            this.txtDirectory.Location = new System.Drawing.Point(70, 13);
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.ReadOnly = true;
            this.txtDirectory.Size = new System.Drawing.Size(238, 20);
            this.txtDirectory.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Directory:";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Value";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // copyCommandLineToolStripMenuItem
            // 
            this.copyCommandLineToolStripMenuItem.Name = "copyCommandLineToolStripMenuItem";
            this.copyCommandLineToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.copyCommandLineToolStripMenuItem.Text = "Copy Commandline";
            this.copyCommandLineToolStripMenuItem.Click += new System.EventHandler(this.copyCommandLineToolStripMenuItem_Click);
            // 
            // frmPresetsEditor
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(380, 435);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPresetsEditor";
            this.ShowInTaskbar = false;
            this.Text = "Edit presets";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridArguments)).EndInit();
            this.ctxArguments.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPresetname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboPresetCategory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView datagridArguments;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridViewComboBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.ContextMenuStrip ctxArguments;
        private System.Windows.Forms.ToolStripMenuItem pasteCommandlineToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnFolderExplore;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboFileExtension;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem deleteRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyCommandLineToolStripMenuItem;
    }
}