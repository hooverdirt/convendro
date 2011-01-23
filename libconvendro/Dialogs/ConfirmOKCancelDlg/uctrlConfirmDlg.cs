using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace libconvendro.Dialogs.Confirm {
    public partial class uctrlConfirmDlg : UserControl {
        public uctrlConfirmDlg() {
            InitializeComponent();
        }

        public Button OKButton {
            get { return this.btnOK; }
            set { this.btnOK = value; }
        }

        public Button CancelButton {
            get { return this.btnCancel; }
            set { this.btnCancel = value; }
        }

        public PictureBox Picture {
            get { return this.imgIcon; }
            set { this.imgIcon = value; }
        }

        public Label Labeltext {
            get { return this.lblMessage; }
            set { this.lblMessage = value; }
        }

        public CheckBox ConfirmCheckBox {
            get { return this.chkDontAskAgain; }
            set { this.chkDontAskAgain = value; }
        }
    }
}
