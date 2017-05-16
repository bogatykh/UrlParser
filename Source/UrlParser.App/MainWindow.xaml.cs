using System.Windows;
using UrlParser.Presentation.Services;
using UrlParser.Presentation.ViewModels;

namespace UrlParser.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _model;

        public MainWindow()
        {
             InitializeComponent();

            DataContext = _model = new MainViewModel(new DialogWindowService());
        }
    }
}
