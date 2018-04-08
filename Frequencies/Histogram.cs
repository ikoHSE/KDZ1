using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace Frequencies {
    public class Histogram: PictureBox {
        public Histogram() {
            Paint += PaintRects;
        }

        private KeyValuePair<string, int>[] _values;
        private ListView.SelectedIndexCollection _selectedIndices;
        private Color _mainColor = Color.FromArgb(230, 230, 230); //Color.LightGray;
        private Color _selectedColor = Color.FromArgb(200, 200, 200); //Color.DimGray;
        private int _count = 0;

        public int Count {
            get => _count;
            set {
                _count = value;
                Invalidate();
            }
        }

        public Color MainColor {
            get => _mainColor;
            set {
                _mainColor = value;
                Invalidate();
            }
        }

        public Color SelectedColor {
            get => _selectedColor;
            set {
                _selectedColor = value;
                Invalidate();
            }
        }

        public ListView.SelectedIndexCollection SelectedIndices {
            get => _selectedIndices;
            set {
                _selectedIndices = value;
                Invalidate();
            }
        }

        public KeyValuePair<string, int>[] values {
            get => _values;
            set {
                _values = value;
                Invalidate();
            }
        }


        private static int minPadding = 8;
        private static int minWidth = 10;
        private Font wordFont = new Font(new FontFamily("Segoe UI"), 21, FontStyle.Bold);
        private Font numFont = new Font(new FontFamily("Segoe UI"), 8, FontStyle.Bold);
        private static Color wordColor = Color.Black;
        private static Color numColor = Color.Black;
        private static int numberPadding = 1;
        private int heightBuffer = 21;
        private void PaintRects(object _, PaintEventArgs e) {
            if (values.Length > 0) {
                var g = e.Graphics;
                var mainBrush = new SolidBrush(MainColor);
                var selectedBrush = new SolidBrush(SelectedColor);
                var wordBrush = new SolidBrush(wordColor);
                var numBrush = new SolidBrush(numColor);
                var min = Math.Min(Count, values.Length);
                var k = (double)(Height - heightBuffer) / (double)values.First().Value;
                var currentX = minPadding;
                var numberWidth = ResultPresenter.WidthFor(values.First().Value, numFont, g);
                var i = 0;
                foreach (var el in values.Take(Count)) {

                    g.ResetTransform();

                    var word = el.Key;
                    var descent = wordFont.FontFamily.GetCellDescent(wordFont.Style);
                    var lineHeight = wordFont.FontFamily.GetLineSpacing(wordFont.Style);
                    var actualDescent = wordFont.Height * descent / lineHeight;
                    var actualWordWidth = wordFont.Height - actualDescent;

                    var barHeight = (int) (el.Value * k);

                    var actualWidth = Math.Max(actualWordWidth + numberPadding, minWidth) + numberWidth;
                    var rect = new Rectangle(new Point(currentX, Height - barHeight), new Size(actualWidth, barHeight));
                    if (SelectedIndices == null) {
                        g.FillRectangle(mainBrush, rect);
                    } else {
                        g.FillRectangle(SelectedIndices.Contains(i) ? selectedBrush : mainBrush, rect);
                    }


                    g.TranslateTransform(currentX, Height - barHeight);

                    var numString = $"{el.Value}";
                    var numSize = g.MeasureString(numString, numFont);
                    g.DrawString(numString, numFont, numBrush, new PointF(actualWordWidth + numberPadding + (numberWidth - numSize.Width), -numSize.Height));

                    var wordSize = g.MeasureString(el.Key, wordFont);
                    var wordHeight = wordSize.Width;
                    var actualWordHeight = Math.Max(wordHeight, barHeight);

                    g.RotateTransform(90);
                    g.DrawString(el.Key, wordFont, wordBrush, new PointF(barHeight - actualWordHeight, -actualWordWidth));

                    currentX += actualWidth + minPadding;
                    i += 1;
                }
                Width = currentX;
            }
        }

    }
}
