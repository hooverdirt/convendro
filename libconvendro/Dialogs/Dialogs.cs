using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using libconvendro.Dialogs.Confirm;


namespace libconvendro.Dialogs {

    public enum DialogType {
        OKCancel = 0,
        YesNo = 1
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ConfirmOKCancelDialogBox {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anicon"></param>
        /// <returns></returns>
        private static Bitmap iconEnumerationToBitmap(MessageBoxIcon anicon) {
            Bitmap res = null;
            switch (anicon) {
                case MessageBoxIcon.Asterisk:
                    res = SystemIcons.Asterisk.ToBitmap();
                    break;
                case MessageBoxIcon.Error:
                    res = SystemIcons.Error.ToBitmap();
                    break;
                case MessageBoxIcon.Exclamation:
                    res = SystemIcons.Exclamation.ToBitmap();
                    break;
                case MessageBoxIcon.None:
                    break;
                case MessageBoxIcon.Question:
                    res = SystemIcons.Question.ToBitmap();
                    break;
                default :
                    res = SystemIcons.Information.ToBitmap();
                    break;
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aform"></param>
        private static void prepareForm(Form aform, uctrlConfirmDlg confirmPanel,
            string caption, string labeltext,
            MessageBoxIcon anicon, bool showcheckbox) {
            confirmPanel.Labeltext.Text = labeltext;
            aform.Text = caption;
            aform.Controls.Add(confirmPanel);
            aform.ClientSize = confirmPanel.ClientSize;
            aform.AcceptButton = confirmPanel.OKButton;
            aform.CancelButton = confirmPanel.CancelButton;
            aform.FormBorderStyle = FormBorderStyle.FixedDialog;
            aform.MaximizeBox = false;
            aform.MinimizeBox = false;
            aform.ShowIcon = false;
            aform.ControlBox = false;
            aform.ShowInTaskbar = false;
            aform.WindowState = FormWindowState.Normal;
            aform.StartPosition = FormStartPosition.CenterParent;

            if (!showcheckbox) {
                confirmPanel.ConfirmCheckBox.Visible = false;
                aform.Height -= (confirmPanel.ConfirmCheckBox.Height -
                    (aform.Height - aform.ClientSize.Height));
            }

            confirmPanel.Picture.Image = iconEnumerationToBitmap(anicon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acaption"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DialogResult ShowDialog(string acaption, string text) {
            DialogResult res = DialogResult.Cancel;
            Form embeddedform = new Form();
            uctrlConfirmDlg confirmPanel = new uctrlConfirmDlg();
            try {
                prepareForm(embeddedform, confirmPanel, acaption, text,
                    MessageBoxIcon.Information, false);
                res = embeddedform.ShowDialog();

            } finally {
                embeddedform.Dispose();
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acaption"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult ShowDialog(string acaption, string text, MessageBoxIcon icon) {
            DialogResult res = DialogResult.Cancel;
            Form embeddedform = new Form();
            uctrlConfirmDlg confirmPanel = new uctrlConfirmDlg();
            try {
                prepareForm(embeddedform, confirmPanel, acaption, text, icon, false);
                res = embeddedform.ShowDialog();

            } finally {
                embeddedform.Dispose();
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acaption"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <param name="checkboxset"></param>
        /// <returns></returns>
        public static DialogResult ShowDialog(string acaption, string text, MessageBoxIcon icon,
            ref bool checkboxset
            ) {

            DialogResult res = DialogResult.OK;

            // Don't bother showing it if it's NOT set...
            if (checkboxset) {
                Form embeddedform = new Form();
                uctrlConfirmDlg confirmPanel = new uctrlConfirmDlg();
                try {
                    prepareForm(embeddedform, confirmPanel, acaption, text, icon, true);
                    res = embeddedform.ShowDialog();

                } finally {
                    checkboxset = !confirmPanel.ConfirmCheckBox.Checked;
                    embeddedform.Dispose();
                }
            }

            return res;
        }

        public static DialogResult ShowDialog(string acaption, string text, MessageBoxIcon icon,
            ref bool checkboxset, DialogType dlgtype
            ) {

            DialogResult res = DialogResult.OK;

            // Don't bother showing it if it's NOT set...
            if (checkboxset) {
                Form embeddedform = new Form();
                uctrlConfirmDlg confirmPanel = new uctrlConfirmDlg();
                try {
                    prepareForm(embeddedform, confirmPanel, acaption, text, icon, true);

                    if (dlgtype == DialogType.YesNo) {
                        confirmPanel.OKButton.Text = "Yes";
                        confirmPanel.OKButton.DialogResult = DialogResult.Yes;
                        confirmPanel.CancelButton.Text = "No";
                        confirmPanel.CancelButton.DialogResult = DialogResult.No;                        
                    }
                    res = embeddedform.ShowDialog();

                } finally {
                    checkboxset = !confirmPanel.ConfirmCheckBox.Checked;
                    embeddedform.Dispose();
                }
            }

            return res;
        }

    }

}
