using FilePicker;
using System.Windows;
using System.Windows.Input;

namespace FilePicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FileChooser FC { get; private set; }
        public FileRepresentation LastPlayed { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.ContentRendered += (o, a) =>
            {
                FC = new FileChooser();
                PlayBtn.Content = "Play";
            };
        }

        private void PlayBtn_OnClick(object sender, RoutedEventArgs e)
        {
            FileRepresentation f = FC.Select();
            RootFolder.Content = f.RootFolder;
            BaseFolder.Content = f.BaseFolder;
            SubFolder.Content = f.SubFolder;
            FileName.Content = f.FileName;
            Date.Content = f.DateAsString;
            LastPlayed = f;
            f.Open();
        }

        private void PlayAgain(object sender, MouseButtonEventArgs e)
        {
            LastPlayed?.Open();
        }

        private void ShowInExplorer(object sender, MouseButtonEventArgs e)
        {
            LastPlayed?.ShowInExplorer();
        }
    }
}