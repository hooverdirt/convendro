using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Plugins;
using System.Windows.Forms;

namespace MediaInfoPlugin {
    public class MediaInfoPlugin : BaseConvendroPlugin {
        private void setupInternals() {
            this.Author = "Arthur Hoogervorst";
            this.CopyrightInformation = "Copyrights 2009 Arthur Hoogervorst";
            this.Description = "Mediafile Library plugin";
            this.Caption = "Mediafile properties...";
            this.Version = new Version("1.0.0.0");
            this.ShortcutKeys = Keys.ShiftKey | Keys.Alt | Keys.M;
            this.MenuBitmap = Properties.Resources.drive_cd;
            // override default Guid in Baseclass.
            this.Guid = new Guid("51BFE6FD-39F3-4A3F-ABF6-438E00FD6B4F");
        }

        /// <summary>
        /// 
        /// </summary>
        public MediaInfoPlugin() {
            this.setupInternals();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Execute() {
            bool b = true;
            string[] array = this.Host.SelectedItems;

            if (array.Length > 0) {
                MediaInfo newmedia = new MediaInfo();
                try {
                    newmedia.Open(array[0]);
                    newmedia.Inform();
                    newmedia.Option("Complete");
                    MessageBox.Show(newmedia.Inform());
                } finally {
                    newmedia.Close();
                }
            }

            return b;
        }
    }
}
