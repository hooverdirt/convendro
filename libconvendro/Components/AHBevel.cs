using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace libconvendro.Components {
    /// <summary>
    /// Bevel style
    /// </summary>
    public enum BevelStyle {
        Lowered, Raised
    }

    /// <summary>
    /// Bevel Shape
    /// </summary>
    public enum BevelShape {
        Box,
        Frame,
        TopLine,
        BottomLine,
        RightLine,
        LeftLine,
        LeftRightLine,
        TopBottomLine
    }

    /// <summary>
    /// Plain bevel control (not a container).
    /// </summary>
    public class AHBevel : Control {
        BevelStyle style = BevelStyle.Lowered;
        BevelShape shape = BevelShape.Box;

        public AHBevel() {

        }

        public BevelShape Shape {
            get { return this.shape; }
            set {
                setShape(value);
            }
        }

        public BevelStyle Style {
            get { return this.style; }
            set { setStyle(value); }
        }


        private void BevelRectangle(Rectangle rect, Color col1, Color col2, Graphics graphics) {
            Pen n = new Pen(new SolidBrush(col1));
            Pen p = new Pen(new SolidBrush(col2));
            try {
                graphics.DrawLines(n,
                    new Point[]{
                        new Point(rect.Left, rect.Bottom),
                        new Point(rect.Left, rect.Top),
                        new Point(rect.Right, rect.Top)});
                graphics.DrawLines(p,
                    new Point[] {
                        new Point(rect.Right, rect.Top),
                        new Point(rect.Right, rect.Bottom),
                        new Point(rect.Left, rect.Bottom)
                    });
            } finally {
                n.Dispose();
                p.Dispose();
            }
        }

        private void BevelLine(Color col, int x1, int y1, int x2, int y2, Graphics graphics) {
            Pen n = new Pen(new SolidBrush(col));
            try {
                graphics.DrawLine(n, new Point(x1, y1), new Point(x2, y2));
            } finally {
                n.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e) {

            Color col1 = new Color();
            Color col2 = new Color();
            base.OnPaint(e);
            if (style == BevelStyle.Lowered) {
                col1 = SystemColors.ControlDark;
                col2 = SystemColors.ControlLightLight;
            } else {
                col1 = SystemColors.ControlLightLight;
                col2 = SystemColors.ControlDark;
            }

            switch (shape) {
                case BevelShape.Box:
                    BevelRectangle(new Rectangle(0, 0, this.ClientRectangle.Width - 1,
                        this.ClientRectangle.Height - 1), col1, col2, e.Graphics);
                    break;
                case BevelShape.Frame:
                    Color temp = col1;
                    col1 = col2;
                    BevelRectangle(new Rectangle(1, 1, this.ClientRectangle.Width - 2,
                        this.ClientRectangle.Height - 2), col1, col1, e.Graphics);
                    col2 = temp;
                    col1 = temp;
                    BevelRectangle(new Rectangle(0, 0, this.ClientRectangle.Width - 2,
                        this.ClientRectangle.Height - 2), col1, col2, e.Graphics);
                    break;
                case BevelShape.TopLine:
                    BevelLine(col1, 0, 0, this.ClientRectangle.Width, 0, e.Graphics);
                    BevelLine(col2, 0, 1, this.ClientRectangle.Width, 1, e.Graphics);
                    break;
                case BevelShape.BottomLine:
                    BevelLine(col1, 0, this.ClientRectangle.Height - 2,
                        this.ClientRectangle.Width, this.ClientRectangle.Height - 2,
                        e.Graphics);
                    BevelLine(col2, 0, this.ClientRectangle.Height - 1,
                        this.ClientRectangle.Width, this.ClientRectangle.Height - 1,
                        e.Graphics);
                    break;
                case BevelShape.LeftLine:
                    BevelLine(col1, 0, 0, 0, this.ClientRectangle.Height, e.Graphics);
                    BevelLine(col2, 1, 0, 1, this.ClientRectangle.Height, e.Graphics);
                    break;
                case BevelShape.RightLine:
                    BevelLine(col1, this.ClientRectangle.Width - 2, 0,
                        this.ClientRectangle.Width - 2, this.ClientRectangle.Height,
                        e.Graphics);
                    BevelLine(col2, this.ClientRectangle.Width - 1, 0,
                        this.ClientRectangle.Width - 1, this.ClientRectangle.Height,
                        e.Graphics);
                    break;
                case BevelShape.LeftRightLine:
                    BevelLine(col1, 0, 0, 0, this.ClientRectangle.Height, e.Graphics);
                    BevelLine(col2, 1, 0, 1, this.ClientRectangle.Height, e.Graphics);
                    BevelLine(col1, this.ClientRectangle.Width - 2, 0,
                        this.ClientRectangle.Width - 2, this.ClientRectangle.Height,
                        e.Graphics);
                    BevelLine(col2, this.ClientRectangle.Width - 1, 0,
                        this.ClientRectangle.Width - 1, this.ClientRectangle.Height,
                        e.Graphics);
                    break;
                case BevelShape.TopBottomLine:
                    BevelLine(col1, 0, 0, this.ClientRectangle.Width, 0, e.Graphics);
                    BevelLine(col2, 0, 1, this.ClientRectangle.Width, 1, e.Graphics);
                    BevelLine(col1, 0, this.ClientRectangle.Height - 2,
                        this.ClientRectangle.Width, this.ClientRectangle.Height - 2,
                        e.Graphics);
                    BevelLine(col2, 0, this.ClientRectangle.Height - 1,
                        this.ClientRectangle.Width, this.ClientRectangle.Height - 2,
                        e.Graphics);
                    break;



            }
        }

        private void setStyle(BevelStyle astyle) {
            if (astyle != this.style) {
                this.style = astyle;
                this.Invalidate();
            }
        }

        private void setShape(BevelShape ashape) {
            if (ashape != this.shape) {
                this.shape = ashape;
                this.Invalidate();
            }
        }


    }
}
