using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Comparers;

namespace libconvendro.Persistence {
    /// <summary>
    /// A list of Presets, which is used to store presets 
    /// persistently to drive.
    /// </summary>
    public class PresetsFile {
        private List<Preset> presetslist = new List<Preset>();

        public PresetsFile() { }

        /// <summary>
        /// 
        /// </summary>
        public List<Preset> Presets {
            get { return this.presetslist; }
            set { this.presetslist = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apreset"></param>
        /// <returns></returns>
        public int AddPreset(Preset apreset) {
            int res = -1;
            try {
                presetslist.Add(apreset);
            } catch {
                res = -1;
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presets"></param>
        public void AddPresets(List<Preset> presets) {
            foreach(Preset p in presets) {
                Preset currpreset = FindPreset(p.Name);
                if (currpreset == null) {
                    // add it...
                    presetslist.Add(p);
                }
            }

            this.Sort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anindex"></param>
        public void RemovePreset(int anindex) {
            this.presetslist.RemoveAt(anindex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apreset"></param>
        public void RemovePreset(Preset apreset) {
            this.presetslist.Remove(apreset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Preset FindPreset(string name) {
            return this.presetslist.Find(delegate(Preset p) { return p.Name == name; });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apreset"></param>
        /// <returns></returns>
        public int FindPresetIndex(Preset apreset) {
            int res = -1;

            for (int i = 0; i < this.presetslist.Count; i++) {
                if (this.presetslist[i].Name == apreset.Name) {
                    res = i;
                    break;
                }
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aname"></param>
        /// <returns></returns>
        public int FindPresetIndex(string aname) {
            int res = -1;

            for (int i = 0; i < this.presetslist.Count; i++) {
                if (this.presetslist[i].Name == aname) {
                    res = i;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Sort() {
            this.presetslist.Sort(new PresetsNameSorter());
        }

        /// <summary>
        /// 
        /// </summary>
        public void Sort(bool reverse) {
            this.presetslist.Sort(new PresetsNameSorter(reverse));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(IComparer<Preset> comparer) {
            this.presetslist.Sort(comparer);
        }
    }

}
