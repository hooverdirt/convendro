using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using libconvendro.Persistence;
using libconvendro.Plugins;

namespace libconvendro.Threading {
    /// <summary>
    /// 
    /// </summary>
    public class FFMPEGConverter : BaseProcessConverter{
        private float fileduration = 0.00F;
        private DateTime currentdate;
        private ManualResetEvent stopThread;
        private ManualResetEvent threadHasStopped;       

        /// <summary>
        /// 
        /// </summary>
        public void Clear() {
            this.MediaFileItems.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public FFMPEGConverter() {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopthread"></param>
        /// <param name="signalthreadstopped"></param>
        public FFMPEGConverter(ManualResetEvent stopthread, 
            ManualResetEvent signalthreadstopped) : this() {
            stopThread = stopthread;
            threadHasStopped = signalthreadstopped;
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void execthread() {
            foreach (MediaFile m in this.MediaFileItems.Items) {

                if (stopThread.WaitOne(0, true)) {
                    threadHasStopped.Set();
                    this.synchUpdateControls();
                    return;
                }

                // ignore anything that has a date in it...
                if (m.DateFinished != DateTime.MinValue) {
                    continue;
                }

                this.synchPrepareListViewItem(m);

                Process nprocess = new Process();
                try {
                    float current = 0.00F;
                    this.fileduration = 0.00F;
                    currentdate = DateTime.Now;

                    m.DateStarted = currentdate;

                    nprocess.StartInfo.FileName = this.Executable;
                    nprocess.StartInfo.Arguments = m.BuildCommandLine();

                    nprocess.EnableRaisingEvents = false;
                    nprocess.StartInfo.UseShellExecute = false;
                    nprocess.StartInfo.CreateNoWindow = true;
                    nprocess.StartInfo.RedirectStandardOutput = true;
                    nprocess.StartInfo.RedirectStandardError = true;
                    nprocess.Start();
                    StreamReader d = nprocess.StandardError;
                    do {
                        string s = d.ReadLine();
                        if (s.Contains("Duration: ")) {
                            string stime = Functions.ExtractDuration(s);
                            fileduration = Functions.TotalStringToSeconds(stime);
                            synchTotalFloat();

                        } else {
                            // Sound files don't have frame rates, so double check for 'size'
                            // this may need to be cleaned up.
                            if (s.Contains("frame=") || s.Contains("size=")) {
                                string currents = Functions.ExtractTime(s);
                                current = Functions.CurrentStringToSeconds(currents);
                                synchCurrentFloat(current);
                            }
                        }

                        if (stopThread.WaitOne(0, true)) {
                            nprocess.Kill();
                            threadHasStopped.Set();
                            this.synchUpdateControls();
                            return;
                        }

                    } while (!d.EndOfStream);
                    nprocess.WaitForExit();
                } catch {
                    // Signal the process to kill itself:
                    // Should probably start setting the state of the process,
                    // so user knows something went wrong.
                    nprocess.Kill();
                } finally {
                    nprocess.Close();
                }

                m.DateFinished = DateTime.Now;
                this.synchListViewItem(m);
            } // End Foreach

            /// update controls
            threadHasStopped.Set();
            this.synchUpdateControls();
        }

        /// <summary>
        /// 
        /// </summary>
        private void synchUpdateControls() {
            if (this.Form != null) {
                if (this.Form.InvokeRequired) {
                    this.Form.Invoke(new MethodInvoker(synchUpdateControls));
                } else {
                    (this.Form as IThreadingHost).SetControlsThreading(true);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amediafile"></param>
        private void synchPrepareListViewItem(MediaFile amediafile) {
            if (this.Form != null) {
                if ((this.Form as IThreadingHost).FileListView.InvokeRequired) {
                    (this.Form as IThreadingHost).FileListView.Invoke(new MediaFileInvoker(synchPrepareListViewItem),
                        new object[] { amediafile });
                } else {
                    if (amediafile != null) {
                        // for now assume working process state
                        (this.Form as IThreadingHost).FileListView.Items[amediafile.Order].ImageIndex = (int)ProcessState.Working;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amediafile"></param>
        /// <param name="atimespan"></param>
        private void synchListViewItem(MediaFile amediafile) {
            if (this.Form != null) {
                if ((this.Form as IThreadingHost).FileListView.InvokeRequired) {
                    (this.Form as IThreadingHost).FileListView.Invoke(new MediaFileInvoker(synchListViewItem), new object[] { amediafile });
                } else {
                    if (amediafile != null) {
                        TimeSpan atimespan = amediafile.DateFinished.Subtract(amediafile.DateStarted);
                        (this.Form as IThreadingHost).FileListView.Items[amediafile.Order].SubItems[Functions.SUBCOL_DURATION].Text =
                            String.Format(Functions.TIMEFORMAT_HHMMSS,
                            atimespan.Hours, atimespan.Minutes, atimespan.Seconds);
                        (this.Form as IThreadingHost).FileListView.Items[amediafile.Order].SubItems[Functions.SUBCOL_STARTED].Text =
                            String.Format(Functions.TIMEFORMAT_HHMMSS,
                            amediafile.DateStarted.Hour, amediafile.DateStarted.Minute, amediafile.DateStarted.Second);
                        (this.Form as IThreadingHost).FileListView.Items[amediafile.Order].SubItems[Functions.SUBCOL_FINISHED].Text =
                            String.Format(Functions.TIMEFORMAT_HHMMSS,
                            amediafile.DateFinished.Hour, amediafile.DateFinished.Minute, amediafile.DateFinished.Second);

                        (this.Form as IThreadingHost).FileListView.Items[amediafile.Order].ImageIndex = (int)ProcessState.Success;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private void synchTextStatusBar(string text) {
            if (this.Form != null) {
                if ((this.Form as IThreadingHost).Statusbar.InvokeRequired) {
                    (this.Form as IThreadingHost).Statusbar.Invoke(new StringInvoker(synchTextStatusBar), new object[] { text });
                } else {
                    if (!(this.Form as IThreadingHost).Statusbar.IsDisposed) {
                        if ((this.Form as IThreadingHost).Statusbar.Items[2] != null) {
                            ((this.Form as IThreadingHost).Statusbar.Items[2] as ToolStripLabel).Text = text;
                        }
                    }
                }
            }
        }

        private void synchTextOutput(string text) {
            if (this.Form != null) {
                if ((this.Form as IThreadingHost).LogBox.InvokeRequired) {
                    (this.Form as IThreadingHost).LogBox.Invoke(new StringInvoker(synchTextOutput), new object[] { text });
                } else {
                    (this.Form as IThreadingHost).LogBox.Text = text;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floating"></param>
        private void synchTotalFloat() {
            if (this.Form != null) {
                if ((this.Form as IThreadingHost).Statusbar.InvokeRequired) {
                    (this.Form as IThreadingHost).Statusbar.Invoke(new MethodInvoker(synchTotalFloat));
                } else {
                    ((this.Form as IThreadingHost).Statusbar.Items[1] as ToolStripProgressBar).Minimum = 0;
                    ((this.Form as IThreadingHost).Statusbar.Items[1] as ToolStripProgressBar).Maximum = 100;
                    ((this.Form as IThreadingHost).Statusbar.Items[1] as ToolStripProgressBar).Value = 0;
                }
            }
        }

        /// <summary>
        /// Sets the status...
        /// </summary>
        /// <param name="avalue"></param>
        private void synchCurrentFloat(float avalue) {
            if (this.Form != null) {
                if ((this.Form as IThreadingHost).Statusbar.InvokeRequired) {
                    (this.Form as IThreadingHost).Statusbar.Invoke(new FloatInvoker(synchCurrentFloat),
                        new object[] { avalue });
                } else {
                    int v = ((this.Form as IThreadingHost).Statusbar.Items[1] as ToolStripProgressBar).Value;

                    float vv = (avalue / this.fileduration);

                    int stat = (int)(vv * 100F);

                    if (stat > 100) {
                        stat = 100;
                    }

                    TimeSpan pdelta = DateTime.Now.Subtract(currentdate);

                    Double seconds = (pdelta.TotalSeconds * fileduration) / avalue;

                    TimeSpan pdelta2 = new TimeSpan(0, 0, (int)seconds);
                    DateTime finishtime = currentdate.Add(pdelta2);

                    ((this.Form as IThreadingHost).Statusbar.Items[1] as ToolStripProgressBar).Value = stat;
                    ((this.Form as IThreadingHost).Statusbar.Items[2] as ToolStripLabel).Text = String.Format(
                        "Started: {0}," + "Est. finish: {1}" + ", Duration: {2}",
                        currentdate.ToShortTimeString(),
                        finishtime.ToShortTimeString(),
                        String.Format("{0}h {1}m {2}s", pdelta2.Hours, pdelta2.Minutes, pdelta2.Seconds)
                        );
                }
            }
        }

    }
}
