using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Persistence;

namespace libconvendro.Comparers {
    /// <summary>
    /// MediaFileOrderSorter.
    /// </summary>
    public class MediaFileOrderSorter : BaseMediaFileSorter {
        public override int Compare(MediaFile x, MediaFile y) {
            int res = 0;

            if (x != null && y != null) {
                if (Reverse) {
                    res = y.Order.CompareTo(x.Order);
                } else {
                    res = x.Order.CompareTo(y.Order);
                }
            }

            return res;
        }
    }
}
