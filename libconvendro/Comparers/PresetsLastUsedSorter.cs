using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Persistence;

namespace libconvendro.Comparers {
    public class PresetsLastUsedSorter : BasePresetsSorter {
        public PresetsLastUsedSorter() {
            this.Reverse = false;
        }

        public override int Compare(Preset x, Preset y) {
            int i = 0;

            if (x != null && y != null) {
                if (!this.Reverse) {
                    i = x.LastUsed.CompareTo(y.LastUsed);
                } else {
                    i = y.LastUsed.CompareTo(x.LastUsed);
                }
            }

            return i;
        }
    }
}
