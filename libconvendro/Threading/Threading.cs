using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Persistence;

namespace libconvendro.Threading {
    public delegate void BooleanInvoker(bool aboolean);
    public delegate void StringInvoker(string text);
    public delegate void FloatInvoker(float floating);
    public delegate void MediaFileInvoker(MediaFile amediafile);

    public enum ProcessStage {
        Unknown = 0,
        Starting = 1,
        Processing = 2,
        Error = 3
    }
}
