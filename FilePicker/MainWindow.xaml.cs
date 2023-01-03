using FilePicker;
using FilePicker.DataAccess;
using FilePicker.PlayWindow;
using FilePicker.Scanner;
using FilePicker.Settings;
using FilePicker.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FilePicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsModel Settings { get; set; }
        private SqLiteService sqLiteService { get; set; }

        private IEnumerable<FileRepresentation> data { get; set; }
        public Playlist PlayList { get; set;  } // keepinbg this reference here since if user switches tabs we need to know the last playlist. wenn man data und settings hält könnt man playlist auch glei.
        public ApplicationStatus ApplicationStatus { get; }
        public FileCounter FileCounter { get; } = new FileCounter();
        public StatusChecker StatusChecker { get; } = new StatusChecker();
        private FilterValidator Validator { get; } = new FilterValidator();

        public event EventHandler<FileCountResult> OnScanFinished;

        public MainWindow()
        {
            InitializeComponent();

            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;

            Settings = SettingsPersistence.LoadFromAppdata();

            this.sqLiteService = new SqLiteService();
            this.data = sqLiteService.ReadData();

            this.ApplicationStatus = new ApplicationStatus();
            this.DataContext = this.ApplicationStatus;


            this.ApplicationStatus.Status = StatusChecker.GetStatusAfterStartup(this.data, this.Settings);

            if (this.ApplicationStatus.CanPlay)
            {
                InitPlayList();
                this.contentControl.Content = new MainControl(this.PlayList);
            }
            else
            {
                SettingsView(null, null);
            }



        }

        private void InitPlayList()
        {
            if (!this.ApplicationStatus.RequiresNewPlaylist) // das iwie komisch... ich würd bei jedem scan einfach ne neue list ewrwarten...
            {
                return;
            }

            this.ApplicationStatus.RequiresNewPlaylist = false;

            this.PlayList = new Playlist(new FileChooser(this.data, this.Settings));
        }

        private void DefaultView(object sender, RoutedEventArgs e)
        {
            InitPlayList();
            this.contentControl.Content = new MainControl(this.PlayList);

        }

        private void SettingsView(object sender, RoutedEventArgs e)
        {
            var fileCountResult = this.FileCounter.CountFiles(this.data.AsQueryable(), this.Settings);

            var s = new SettingsControl(this.Settings, fileCountResult, this.ApplicationStatus);
            s.Unloaded += (a, b) =>
            {
                SettingsPersistence.StoreToAppdata(Settings);
                OnScanFinished -= (_, fcr) => s.SetAdditionalInfoTexts(fcr);
            };
            this.contentControl.Content = s;
            OnScanFinished += (_, fcr) => s.SetAdditionalInfoTexts(fcr);
        }

        private async void Scan(object sender, RoutedEventArgs e)
        {
            // button should be greyed out but check for safety.
            this.Settings = SettingsPersistence.LoadFromAppdata();
            var validatorResult = Validator.Validate(this.Settings);
            if (validatorResult.HasMainFilterError || validatorResult.HasPrevalenceFilterError)
            {
                MessageBox.Show("Invalid Filters - Can't Scan - Scan Button should be greyed out at this point.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var w = new ProgressBarBlocker();
            var scnaner = new FileScanner();
            scnaner.OnProgress += (a, progress)
                => Dispatcher.Invoke(() => w.ProgressBar.Value = progress); // tut noch nicht so ganz (aber halb immerhin)

            var cts = new CancellationTokenSource();
            var filesTask = Task.Run(() => scnaner.Scan(Settings.Folders, cts.Token));
            w.Show();
            w.Closing += (a, b) => cts.Cancel();

            IEnumerable<FileRepresentation> files = null;

            this.IsEnabled = false;
            try
            {
                files = await filesTask;
            }
            catch (TaskCanceledException) { }
            catch (OperationCanceledException) { }

            this.IsEnabled = true;
            w.Close();

            if (files != null)
            {
                this.sqLiteService.RebuildDataBaseByData(files);
                this.data = files;
                var fcr = this.FileCounter.CountFiles(files.AsQueryable(), this.Settings);
                this.ApplicationStatus.Status = this.StatusChecker.GetStatusAfterFileScan(fcr);
                OnScanFinished?.Invoke(this, fcr);
            }
        }

        private void HelpView(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new InfoPage();
        }

        void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception occurred: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            this.ApplicationStatus.Status = ApplicationStatusEnum.Error;
            e.Handled = true;
        }
    }
}