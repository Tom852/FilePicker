using FilePicker;
using FilePicker.DataAccess;
using FilePicker.Scanner;
using FilePicker.Settings;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        public MainWindow()
        {
            InitializeComponent();
            Settings = SettingsPersistence.Load();

            this.sqLiteService = new SqLiteService();
            this.data = sqLiteService.ReadData();
            this.contentControl.Content = new MainControl(this.data);
        }

        private void DefaultView(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new MainControl(this.data);

        }

        private void SettingsView(object sender, RoutedEventArgs e)
        {
            var s = new SettingsControl(Settings);
            s.Unloaded += (a, b) =>
            {
                SettingsPersistence.Store(Settings);
            };
            this.contentControl.Content = s;
        }

        private async void Scan(object sender, RoutedEventArgs e)
        {
            var w = new ProgressBarBlocker();
            var scnaner = new FileScanner();
            scnaner.OnProgress += (a, progress)
                => Dispatcher.Invoke(() => w.ProgressBar.Value = progress); // tut noch nicht so ganz

            var cts = new CancellationTokenSource();
            var filesTask = Task.Run( () => scnaner.Scan(Settings.Folders, cts.Token));
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
            }
        }

        private void HelpView(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new InfoPage();
        }
    }
}