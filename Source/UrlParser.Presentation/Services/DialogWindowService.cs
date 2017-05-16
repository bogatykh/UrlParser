using Microsoft.Win32;

namespace UrlParser.Presentation.Services
{
    public class DialogWindowService : IDialogWindowService
    {
        public IFileDialog CreateOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "*.txt*";
            openFileDialog.Filter = "Text File (.txt)|*.txt";
            openFileDialog.Title = "Load file";
            return new FileDialogResult(openFileDialog);
        }

        private class FileDialogResult : IFileDialog
        {
            private readonly FileDialog m_fileDialog;

            internal FileDialogResult(FileDialog fileDialog)
            {
                m_fileDialog = fileDialog;
            }

            public bool? ShowDialog()
            {
                return m_fileDialog.ShowDialog();
            }

            public string FileName
            {
                get { return m_fileDialog.FileName; }
            }
        }
    }
}
