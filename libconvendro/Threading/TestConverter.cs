using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using libconvendro.Persistence;
using libconvendro.Forms;

namespace libconvendro.Threading {
    /// <summary>
    /// </summary>
    public class TestConverter : BaseProcessConverter {
        private ManualResetEvent mnstopevent, mnhasstoppedevent;
        private ProcessStage processstage = ProcessStage.Unknown;

        public TestConverter(ManualResetEvent stopevent, ManualResetEvent hasstoppedevent) {
            this.mnstopevent = stopevent;
            this.mnhasstoppedevent = hasstoppedevent;

        }

        protected override void execthread() {
            foreach (MediaFile i in this.MediaFileItems.Items) {
                if (mnstopevent.WaitOne(0, true)) {                    
                    mnhasstoppedevent.Set();
                    return;
                }

                SynchTitle(i.Preset.Name);

                Process nprocess = new Process();
                try {
                    nprocess.StartInfo.FileName = this.Executable;
                    nprocess.StartInfo.Arguments = i.BuildCommandLine();
                    nprocess.EnableRaisingEvents = false;
                    nprocess.StartInfo.UseShellExecute = false;
                    nprocess.StartInfo.CreateNoWindow = true;
                    nprocess.StartInfo.RedirectStandardOutput = true;
                    nprocess.StartInfo.RedirectStandardError = true;
                    nprocess.Start();
                    StreamReader d = nprocess.StandardError;
                    do {
                        string s = d.ReadLine();
                        SynchOutputwindow(s);
                        if (s.Contains("Duration: ")) {
                            processstage = ProcessStage.Starting;
                        } else {
                            if (s.Contains("frame=")) {
                                processstage = ProcessStage.Processing;
                            } else {
                                processstage = ProcessStage.Error;
                            }
                        }

                        if (mnstopevent.WaitOne(0, true)) {
                            nprocess.Kill();
                            mnhasstoppedevent.Set();
                            return;
                        }

                    } while (!d.EndOfStream);
                    nprocess.WaitForExit();
                } finally {
                    nprocess.Close();
                }
            }
            SynchControls();
        }

        protected virtual void SynchTitle(string s) {
            if (this.Form.InvokeRequired) {
                this.Form.Invoke(new StringInvoker(SynchTitle), new object[] { s });
            } else {
                this.Form.Text = string.Format("{0} - [{1}]", Functions.FORM_TERMINAL_TITLE, s);
            }
        }

        protected virtual void SynchControls() {
            if ((this.Form as frmTerminal).InvokeRequired) {
                this.Form.Invoke(new MethodInvoker(SynchControls));
            } else {
                (this.Form as frmTerminal).SetThreadingControls(false);
            }
        }

        protected virtual void SynchOutputwindow(string s) {
            if ((this.Form as frmTerminal).Terminal.InvokeRequired) {
                (this.Form as frmTerminal).Terminal.Invoke(new StringInvoker(SynchOutputwindow), new object[] { s });
            } else {
                (this.Form as frmTerminal).Terminal.Text += s + Environment.NewLine;
                (this.Form as frmTerminal).Terminal.SelectionStart =
                    (this.Form as frmTerminal).Terminal.Text.Length - 1;
                (this.Form as frmTerminal).Terminal.ScrollToCaret();
            }
        }

    }
}
