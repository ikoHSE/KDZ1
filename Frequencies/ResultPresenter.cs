using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
namespace Frequencies {
    public class ResultPresenter: View {

        private static Font numberFont = new Font("Segoe UI", 13, FontStyle.Regular);
        private static Color numberColor = Color.Black;
        private static Font wordFont = new Font("Segoe UI", 13, FontStyle.Bold);
        private static Color wordColor = Color.Black;
        private static int paddingBetweenElements = 8;
        private static int minListWidth = 144;
        private static int backButtonWidth = 233;
        private static int backButtonHeight = 50;
        private static Font backButtonFont = new Font("Segoe UI", 21, FontStyle.Regular);
        private static int maxNumberOfElement = 50; 
        private static Font numberLabelFont = new Font("Segoe UI", 34, FontStyle.Bold);

        Histogram hist = new Histogram();
        ListView list = new ListView() {
            View = System.Windows.Forms.View.Details,
            HeaderStyle = ColumnHeaderStyle.None,
            AutoArrange = false
        };
        Panel scrollView = new Panel() {
            AutoScroll = true
        };
        Button backButton = new Button() {
            Text = "  ⟨ Back", 
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = backButtonFont
        };
        TrackBar track = new TrackBar() {
            Minimum = 1
        };
        Label numberLabel = new Label() {
            Font = numberLabelFont,
            TextAlign = ContentAlignment.BottomLeft,
            AutoSize = true
        };

        private void updateNumberLabelText() {
            numberLabel.Text = track.Value == 1 ?
                $"Top word" :
                $"Top {track.Value} words";
        }

        public void Reset() {
            track.Value = 1;
        }

        public ResultPresenter() {
            backButton.Click += (_, __) => {
                DidClickBackButton?.Invoke();
            };
            track.ValueChanged += (_, __) => {
                updateNumberLabelText();
                hist.Count = track.Value;
            };
            list.SelectedIndexChanged += (_, __) => {
                hist.SelectedIndices = list.SelectedIndices;
            };
            controls.Add(list);
            controls.Add(scrollView);
            scrollView.Controls.Add(hist);
            controls.Add(backButton);
            controls.Add(track);
            controls.Add(numberLabel);
        }

        public override void Layout(System.Drawing.Size rect) {
            backButton.FlatStyle = FlatStyle.System;
            backButton.Top = paddingBetweenElements;
            backButton.Left = paddingBetweenElements;
            backButton.Width = backButtonWidth;
            backButton.Height = backButtonHeight;

            numberLabel.Left = backButton.Right + paddingBetweenElements;
            numberLabel.Top = backButton.Bottom - numberLabel.Height;

            list.Width = Math.Max((rect.Width - paddingBetweenElements * 3) / 5, minListWidth);
            list.Height = rect.Height - 2*paddingBetweenElements;
            list.Left = rect.Width - paddingBetweenElements - list.Width;
            list.Top = paddingBetweenElements;
            list.FullRowSelect = true;
            list.Columns[2].Width = list.Width - list.Columns[1].Width - SystemInformation.VerticalScrollBarWidth - list.Columns[0].Width - 4;

            track.Width = rect.Width - 3*paddingBetweenElements - list.Width;
            track.Left = paddingBetweenElements;
            track.Top = rect.Height - paddingBetweenElements - track.Height;

            scrollView.Height = rect.Height - paddingBetweenElements*4 - track.Height - backButton.Height;
            scrollView.Width = rect.Width - 3 * paddingBetweenElements - list.Width;
            scrollView.Top = paddingBetweenElements*2 + backButton.Height;
            scrollView.Left = paddingBetweenElements;
            hist.Height = scrollView.Height - SystemInformation.HorizontalScrollBarHeight;
            hist.Invalidate();
        }

        public void Consume(KeyValuePair<string, int>[] values) {
            list.Clear();

            var maxInt = values.First().Value;
            var numWidth = WidthFor(maxInt, numberFont, list.CreateGraphics());

            list.Columns.Add("", 0, HorizontalAlignment.Right);
            list.Columns.Add("", numWidth, HorizontalAlignment.Right);
            list.Columns.Add("", 0, HorizontalAlignment.Left);

            var itemsToAdd = new ListViewItem[values.Count()];
            var i = 0;

            foreach (var val in values) {
                var item = new ListViewItem("");
                item.UseItemStyleForSubItems = false;

                var numItem = new ListViewItem.ListViewSubItem(item, $"{val.Value}");
                numItem.ForeColor = numberColor;
                numItem.Font = numberFont;

                item.SubItems.Add(numItem);

                var wordItem = new ListViewItem.ListViewSubItem(item, $"{val.Key}");
                wordItem.Font = wordFont;
                wordItem.ForeColor = wordColor;

                item.SubItems.Add(wordItem);

                itemsToAdd[i] = item;

                i += 1;
            }


            list.Items.AddRange(itemsToAdd);

            hist.values = values;

            track.Maximum = Math.Min(maxNumberOfElement, values.Length);

            updateNumberLabelText();
            hist.Count = track.Value;
        }

        public static int WidthFor(int number, Font font, Graphics g) {
            var count = $"{number}".Length;
            var st = g.MeasureString(new String(Enumerable.Repeat('8', count + 1).ToArray()), font);
            return (int) (st.Width);
        }

        public event Action DidClickBackButton;
    }
}
