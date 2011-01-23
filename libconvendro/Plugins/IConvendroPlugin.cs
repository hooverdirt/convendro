using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace libconvendro.Plugins {

    public interface IConvendroPlugin {
        string Name { get;}

        string Description { get; set; }

        string Caption { get; set; }

        string Author { get; set; }

        string CopyrightInformation { get; set;}

        Bitmap MenuBitmap { get; set; }

        Version Version { get; set; }

        Guid Guid { get; set; }

        /// <summary>
        /// Shows the plugin main dialog (if provided)
        /// </summary>
        bool Execute();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Config();

        /// <summary>
        /// 
        /// </summary>
        IConvendroHost Host { get; set; }
    }
}
