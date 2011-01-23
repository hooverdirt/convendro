using System;
using System.Collections.Generic;
using System.Text;

namespace libconvendro.Persistence {
    /// <summary>
    /// MediaFileList.
    /// </summary>
    public class MediaFileList {
        List<MediaFile> items = new List<MediaFile>();

        /// <summary>
        /// 
        /// </summary>
        public List<MediaFile> Items {
            get { return items; }
            set { items = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anidex"></param>
        /// <returns></returns>
        public MediaFile this[int anidex] {
            get { return items[anidex]; }
            set { items[anidex] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count {
            get { return items.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afilename"></param>
        /// <param name="apreset"></param>
        /// <returns></returns>
        public MediaFile AddMediaFile(string afilename, Preset apreset) {
            MediaFile f = null;

            if (!String.IsNullOrEmpty(afilename) && apreset != null) {
                f = new MediaFile(afilename, apreset);
                items.Add(f);
            }

            return f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afilename"></param>
        /// <param name="apreset"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public MediaFile AddMediaFile(string afilename, Preset apreset,
            int order) {

            MediaFile f = null;

            if (!String.IsNullOrEmpty(afilename) && apreset != null) {
                f = new MediaFile(afilename, apreset, order);
                items.Add(f);
            }

            return f;
        }

        public void Clear() {
            this.items.Clear();
        }
    }
}
