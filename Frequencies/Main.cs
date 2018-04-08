using System;
using System.Windows.Forms;

namespace Frequencies {
    public static class Program {
        static MainForm form;
        static FileReader reader;
        static FileChooser fileChooser;
        static ResultPresenter presenter;
        static LoadingView loading;
        [STAThread]
        public static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            form = new MainForm();
            reader = new FileReader();
            fileChooser = new FileChooser();
            presenter = new ResultPresenter();
            loading = new LoadingView();

            form.Present(fileChooser);

            reader.finished += () => {
                var result = reader.result;
                if (result.Length == 0) {
                    form.Present(fileChooser);
                    form.Refresh();
                    MessageBox.Show("There appear to be no words that show up more than once in your file...");
                } else {

                    presenter.Consume(reader.result);
                    form.Present(presenter);
                    form.Refresh();
                }
            };
            presenter.DidClickBackButton += () => {
                form.Present(fileChooser);
                presenter.Reset();
            };
            fileChooser.didSelectFile += (file, encoding) => {
                form.Present(loading);
                form.Refresh();
                reader.Read(file, encoding);
            };

            Application.Run(form);
        }
    }
}
