using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Persistence;

namespace libconvendro.Comparers {
    public abstract class BaseMediaFileSorter : IComparer<MediaFile> {

        private bool reverse = false;

        public bool Reverse {
            get { return this.reverse; }
            set { this.reverse = value; }
        }
        
        public abstract int Compare(MediaFile x, MediaFile y);
    }
}
