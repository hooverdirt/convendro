using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Persistence;

namespace libconvendro.Comparers {
    public class PresetsUsedCountSorter : BasePresetsSorter {
        public PresetsUsedCountSorter() {
            this.Reverse = false;
        }

        public override int Compare(Preset x, Preset y) {
            int i = 0;

            if (x != null && y != null) {
                if (!this.Reverse) {
                    i = x.UsedCount.CompareTo(y.UsedCount);
                } else {
                    i = y.UsedCount.CompareTo(x.UsedCount);
                }
            }

            return i;
        }
    }
}
