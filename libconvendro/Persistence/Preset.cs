using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace libconvendro.Persistence {
    /// <summary>
    /// A preset setting that can contain a list of commandline options.
    /// </summary>
    public class Preset {
        private string name;
        private string category;
        private string description;
        private CommandLineOptions commandlineoptions;
        private string foldername;
        private string extension;
        private int timesused;
        private DateTime dateused;

        private void init() {
            name = null;
            category = null;
            description = null;
            extension = null;
            timesused = 0;
            dateused = DateTime.MinValue;
            foldername = Path.GetDirectoryName(Assembly.GetCallingAssembly().GetName().CodeBase);
            commandlineoptions = new CommandLineOptions();
        }

        public Preset() {
            init();
        }

        public int UsedCount {
            get { return timesused; }
            set { timesused = value; }
        }

        public DateTime LastUsed {
            get { return this.dateused; }
            set { this.dateused = value; }
        }

        public Preset(string aname, string acategory)
            : this() {
            this.name = aname;
            this.category = acategory;
        }

        public Preset(string aname, string acategory, string adescription)
            : this(aname, acategory) {
            this.description = adescription;
        }

        public Preset(string aname, string acategory, string adescription, string afoldername)
            : this(aname, acategory, adescription) {
            this.foldername = afoldername;
        }

        public Preset(string aname, string acategory, string adescription, string afoldername, string anextension) :
            this(aname, acategory, adescription, afoldername) {
            this.extension = anextension;
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Category {
            get { return this.category; }
            set { this.category = value; }
        }

        public string Description {
            get { return this.description; }
            set { this.description = value; }
        }

        public string OutputFolder {
            get { return this.foldername; }
            set { this.foldername = value; }
        }

        public string Extension {
            get { return this.extension; }
            set { this.extension = value; }
        }

        public CommandLineOptions CommandLineOptions {
            get { return this.commandlineoptions; }
            set { this.commandlineoptions = value; }
        }
    }
}
