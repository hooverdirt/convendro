using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using libconvendro.Persistence;

namespace libconvendro.Threading {
    public abstract class AbstractProcessConverter : IProcessConverter {

        public abstract string Executable { get; set; }

        public abstract MediaFileList MediaFileItems {
            get;
            set;
        }

        public abstract Thread CurrentThread {
            get;
        }

        public abstract bool Execute();
    }
}
