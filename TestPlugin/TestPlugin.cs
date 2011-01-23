using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Plugins;
using System.Windows.Forms;
using System.IO;

namespace TestPlugin {
    public class TestPlugin : BaseConvendroPlugin  {
        /// <summary>
        /// 
        /// </summary>
        private void setupInternals() {
            this.Author = "Arthur Hoogervorst";
            this.CopyrightInformation = "Copyrights 2009 Arthur Hoogervorst";
            this.Description = "A test plugin for convendro";
            this.Caption = "Do a test...";
            this.Version = new Version("1.0.0.0");
            this.MenuBitmap = Properties.Resources.address_book_new;
            // override default Guid in Baseclass.
            this.Guid = new Guid("01716dc3-2d75-416c-8a37-ff32ad4011f3");
        }

        /// <summary>
        /// 
        /// </summary>
        public TestPlugin() {
            this.setupInternals();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize() {           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Execute() {

            string filename = "noname unknown haha...";

            if (this.Host != null) {

                string[] array = this.Host.SelectedItems;

                if (array != null & array.Length > 0) {
                    // only get the first item...
                    filename = array[0];
                }
            }

            return (MessageBox.Show("Hello World " + 
                this.Author + " | " + this.Description + " | " +
                this.Version.ToString() + " | selected file :" + filename) == DialogResult.OK);
        }
    }
}
