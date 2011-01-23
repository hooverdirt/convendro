using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace libconvendro.Persistence {
    // Single file with preset set/unset (for threading).
    public class MediaFile {
        private string filename;
        private string outputfilename;
        private Preset currentpreset;
        private int order = 0;
        private DateTime started = DateTime.MinValue;
        private DateTime finished = DateTime.MinValue;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MediaFile() {
            currentpreset = new Preset();
        }

        /// <summary>
        /// Secondary constructor.
        /// </summary>
        /// <param name="afilename"></param>
        /// <param name="apreset"></param>
        public MediaFile(string afilename, Preset apreset) {
            this.filename = afilename;
            this.currentpreset = apreset;
        }

        /// <summary>
        /// Third constructor.
        /// </summary>
        /// <param name="afilename"></param>
        /// <param name="apreset"></param>
        public MediaFile(string afilename, Preset apreset, int anorder) {
            this.filename = afilename;
            this.currentpreset = apreset;
            this.order = anorder;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Filename {
            get { return filename; }
            set { filename = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Order {
            get { return this.order; }
            set { this.order = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Preset Preset {
            get { return currentpreset; }
            set {
                currentpreset = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateStarted {
            get { return started; }
            set { started = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateFinished {
            get { return finished; }
            set { finished = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Duration {
            get {
                TimeSpan res = new TimeSpan();

                if (started != DateTime.MinValue && finished != DateTime.MinValue) {
                    res = finished.Subtract(started);
                }

                return res;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string BuildCommandLine() {
            string res = null;

            if (this.Preset.CommandLineOptions.Count > 0) {

                // Use default directory...
                if (!Directory.Exists(this.Preset.OutputFolder) ){
                    this.Preset.OutputFolder = 
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                string outputfile = Path.Combine(this.Preset.OutputFolder,
                        Path.GetFileNameWithoutExtension(this.filename)
                        + "." + this.Preset.Extension);

                res = "-i " + "\"" + this.filename + "\"" + " " +
                    this.Preset.CommandLineOptions.BuildCommandLine() + " " +
                    "\"" + outputfile + "\"";
            }

            return res;
        }
    }

}
