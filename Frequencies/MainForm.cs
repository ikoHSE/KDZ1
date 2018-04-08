using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace Frequencies {
    public class MainForm: Form {

        public MainForm(): base() {
            Text = "Frequencies";
            MinimumSize = new System.Drawing.Size(750, 500);
            DoubleBuffered = true;
            Layout += (_, __) => {
                currentView?.Layout(ClientSize);
            };
            BackColor = Color.White;
        }

        private View currentView;

        public void Present(View view) {
            currentView = view;
            Controls.Clear();
            Controls.AddRange(view.controlsArray);
            Invalidate();
            Refresh();
        }
    }

    abstract public class View {
        protected List<Control> controls = new List<Control>();
        public Control[] controlsArray {
            get {
                return controls.ToArray();
            }
        }

        abstract public void Layout(Size rect);
    }
}


