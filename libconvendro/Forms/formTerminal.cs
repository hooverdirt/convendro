using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using libconvendro.Threading;
using System.Threading;
using libconvendro.Persistence;
using libconvendro;

namespace libconvendro.Forms {
    public partial class frmTerminal : Form {
        private TestConverter convertthread = null;
        private string executable;
        private MediaFileList mediafilelist = null;
        private ManualResetEvent stopthreadevent = new ManualResetEvent(false);
        private ManualResetEvent threadhasstoppedevent = new ManualResetEvent(false);       

        public frmTerminal() {
            InitializeComponent();
        }

        public frmTerminal(MediaFileList afilelist, string anexecutable) : this() {
            this.executable = anexecutable;
            if (afilelist != null) {
                mediafilelist = afilelist;
            }
        }

        private void formTerminal_Load(object sender, EventArgs e) {
            this.edTerminalLog.Text = "";
        }
       
        public RichTextBox Terminal {
            get { return edTerminalLog; }
            set { edTerminalLog = value; }
        }        

        public void StartProcessing() {
            this.edTerminalLog.Clear();
            stopthreadevent.Reset();
            threadhasstoppedevent.Reset();

            this.convertthread = new TestConverter(stopthreadevent, threadhasstoppedevent);
            this.convertthread.Executable = this.executable;
            this.convertthread.MediaFileItems = this.mediafilelist;
            this.convertthread.Form = this;
            this.convertthread.Execute();
            this.SetThreadingControls(true);
        }

        public void StopProcessing() {
            if (convertthread != null) {
                if (convertthread.CurrentThread != null && convertthread.CurrentThread.IsAlive) {
                    stopthreadevent.Set();

                    while (convertthread.CurrentThread.IsAlive) {
                        if (WaitHandle.WaitAll(new ManualResetEvent[] { threadhasstoppedevent }, 100, true)) {
                            break;
                        }
                        Application.DoEvents();
                    }
                }
            }
            SetThreadingControls(false);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void formTerminal_FormClosing(object sender, FormClosingEventArgs e) {
            StopProcessing();
            e.Cancel = false;
        }

        private void frmTerminal_FormClosed(object sender, FormClosedEventArgs e) {
            this.Dispose();
        }

        public void SetThreadingControls(bool isworking) {
            startToolStripMenuItem.Enabled = !isworking;
            stopToolStripMenuItem.Enabled = isworking;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e) {
            StopProcessing();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e) {
            StartProcessing();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            this.edTerminalLog.SelectAll();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
            this.edTerminalLog.Copy();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog nsaver = new SaveFileDialog();
            try {
                nsaver.Filter = String.Format("{0}|{1}", Functions.MEDIAFILES_FILTER_TXT,
                    Functions.MEDIAFILES_FILTER_ALL);
                if (nsaver.ShowDialog() == DialogResult.OK) {                    
                    edTerminalLog.SaveFile(nsaver.FileName, RichTextBoxStreamType.PlainText);
                }
            } finally {
                nsaver.Dispose();
            }
        }
    }
}
