namespace UrlParser.Presentation
{
    public interface IFileDialog
    {
        bool? ShowDialog();
        string FileName { get; }
    }
}
