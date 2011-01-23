using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace convendro {
    public partial class frmAbout : Form {
        public frmAbout() {
            InitializeComponent();
            SetVersion();
        }

        public void SetVersion() {
            Version n = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = n.ToString();
        }
    }
}
