using System;
using System.Collections.Generic;
using System.Text;
using libconvendro.Persistence;
using System.Threading;

namespace libconvendro.Threading {

    /// <summary>
    /// Generalized ProcessConverter
    /// </summary>
    public interface IProcessConverter {
        bool Execute();
    }
}
