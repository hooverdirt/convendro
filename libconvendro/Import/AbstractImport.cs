using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Persistence;

namespace libconvendro.Import {
    public interface IImporter {
        List<Preset> Presets { get; set; }
        void LoadFile(string filename);
        void SaveFile();
    }

    public class BaseImporter :IImporter {
        protected List<Preset> list = new List<Preset>();
        protected string file;

        /// <summary>
        /// 
        /// </summary>
        public virtual List<Preset> Presets {
            get {
                return list;
            }
            set {
                list = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public virtual void LoadFile(string filename) {
            file = filename;
        }

        public virtual void SaveFile() {
        }
    }
}
