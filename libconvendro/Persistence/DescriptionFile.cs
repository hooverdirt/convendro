using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace libconvendro.Persistence {
    /// <summary>
    /// Description file to store simple name/description items,
    /// for example for commandline commands
    /// </summary>
    public class DescriptionFile {
        private List<DescriptionItem> list = new List<DescriptionItem>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adescriptionitem"></param>
        /// <returns></returns>
        public int AddDescription(DescriptionItem adescriptionitem) {
            int i = -1;

            try {
                list.Add(adescriptionitem);
                i = list.Count - 1;
            } catch {
                i = -1;
            }

            return i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aname"></param>
        /// <param name="adescription"></param>
        /// <returns></returns>
        public int AddDescription(string aname, string adescription) {
            int i = -1;

            try {
                list.Add(new DescriptionItem(aname, adescription));
                i = list.Count - 1;
            } catch {
                i = -1;
            }

            return i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anitem"></param>
        /// <returns></returns>
        public bool RemoveDescription(DescriptionItem anitem) {
            bool b = false;

            if (anitem != null) {
                try {
                    list.Remove(anitem);
                    b = true;
                } catch {
                    b = false;
                }
            }

            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveDescription(string name) {
            bool b = false;

            try {
                int i = list.FindIndex(delegate(DescriptionItem s) { return s.Name == name; });

                if (i > -1) {
                    list.RemoveAt(i);
                }

            } catch {
                b = false;
            }

            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool RemoveDescription(int i) {
            bool b = false;

            try {
                list.RemoveAt(i);
            } catch {
                b = false;
            }
            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear() {
            list.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Assign(IList alist) {
            if (alist != null) {
                for (int i = 0; i < alist.Count; i++) {
                    string s = (string)alist[i];

                    if (String.IsNullOrEmpty(s)) {
                        this.AddDescription(s, s);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<DescriptionItem> Items {
            get { return this.list; }
            set { this.list = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count {
            get { return list.Count; }
        }
    }
}
