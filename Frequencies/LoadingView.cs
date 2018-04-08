using System.Windows.Forms;
using System.Drawing;
namespace Frequencies {
    public class LoadingView: View {

        private Label label = new Label();

        public LoadingView() {
            label.Font = new Font("Segoe UI", 55, FontStyle.Regular);
            controls.Add(label);
            label.ForeColor = Color.LightGray;
            label.AutoSize = true;
            label.Text = "Loading...";
        }

        public override void Layout(System.Drawing.Size rect) {
            label.Left = rect.Width/2 - label.Width/2;
            label.Top = rect.Height/2 - label.Height/2 - (int)((double)(rect.Height) * 0.07);
        }
    }
}
