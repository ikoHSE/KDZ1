using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
namespace Frequencies {
    public class FileChooser: View {

        private ComboBox encodingChooser = new ComboBox() {
            FlatStyle = FlatStyle.System
        };
        private Button button = new Button() {
            FlatStyle = FlatStyle.System
        };
        private OpenFileDialog fileDialog = new OpenFileDialog();

        public FileChooser() {
            controls.Add(encodingChooser);
            encodingChooser.DropDownStyle = ComboBoxStyle.DropDownList;
            encodingChooser.Items.Add("UTF-8");
            encodingChooser.Items.Add("UTF-16"); 
            encodingChooser.Items.Add("Windows-1251");
            encodingChooser.SelectedIndex = 0;
            encodingChooser.Width = 210;
            encodingChooser.Font = new Font(new FontFamily("Segoe UI"), 21, FontStyle.Regular);

            button.Font = new Font(new FontFamily("Segoe UI"), 34, FontStyle.Regular);
            controls.Add(button);
            button.Text = "Select file";
            button.AutoSize = true;

            button.Click += (_, __) => {
                var result = fileDialog.ShowDialog();
                if (fileDialog.FileName != null && result == DialogResult.OK) {
                    didSelectFile?.Invoke(fileDialog.FileName, GetEncoding());
                }
            };

            fileDialog.Filter = "Text files (*.txt)|*.txt";
            fileDialog.InitialDirectory = "c:\\" ;
            fileDialog.FilterIndex = 0;
        }

        private Encoding GetEncoding() {
            switch (encodingChooser.SelectedIndex) {
            case 0:
                return Encoding.UTF8;
            case 1:
                return Encoding.Unicode;
            case 2:
                return Encoding.GetEncoding("windows-1251");
            default:
                throw new Exception("Something has gone horribly wrong.");
            }
        }

        public override void Layout(Size rect) {
            encodingChooser.Left = rect.Width/2 - encodingChooser.Width/2;
            encodingChooser.Top = (int)(rect.Height * 0.23);

            button.Left = rect.Width/2 - button.Width/2;
            button.Top = encodingChooser.Bottom + 55;
        }

        public event Action<string, Encoding> didSelectFile;
        
    }
}
      