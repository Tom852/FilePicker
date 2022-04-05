using FilePicker.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FilePicker.Util;
using Gridify;
using FilePicker.Scanner;
using FilePicker.Status;

namespace FilePicker.Settings
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsModel Ctx { get; set; }
        public ApplicationStatus AppStatus { get; set; }

        private FilterValidator filterValidator { get; } = new FilterValidator();

        public SettingsControl(SettingsModel settings, FileCountResult fileCountResult, ApplicationStatus appStatus)
        {
            InitializeComponent();
            this.Ctx = settings;
            DataContext = this.Ctx;
            this.AppStatus = appStatus;
            SetAdditionalInfoTexts(fileCountResult);
        }



        private void RemoveFolders_Click(object sender, RoutedEventArgs e)
        {
            var badboys = this.Folders.SelectedItems.ConvertAndCastToGenericList<string>();
            foreach (var item in badboys)
            {
                this.Ctx.Folders.Remove(item);
            }

            this.AppStatus.RequiresNewPlaylist = true;

            if (!this.Ctx.Folders.Any())
            {
                this.AppStatus.Status = ApplicationStatusEnum.NoFolders;
            }
            else
            {
                this.AppStatus.Status = ApplicationStatusEnum.FilterChanged;
            }
        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Select a folder";
                dialog.ShowNewFolderButton = false;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                if (Ctx.Folders.Contains(dialog.SelectedPath))
                {
                    MessageBox.Show("Folder already added!");
                    return;
                }
                Ctx.Folders.Add(dialog.SelectedPath);
                SettingsPersistence.Store(this.Ctx);
                this.AppStatus.Status = ApplicationStatusEnum.FilterChanged;
                this.AppStatus.RequiresNewPlaylist = true;
            }


        }

        private void AddMainFilter_Click(object sender, RoutedEventArgs e)
        {
            Ctx.MainFilters.Add(new MainFilter());
        }

        private void XBtnMainFilters_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = MainFiltersListView.Items.IndexOf(item);
            Ctx.MainFilters.RemoveAt(index);
            SettingsPersistence.Store(this.Ctx);
            this.AppStatus.Status = ApplicationStatusEnum.FilterChanged;
            this.AppStatus.RequiresNewPlaylist = true;
        }

        private void AddPrevalenceFilter_Click(object sender, RoutedEventArgs e)
        {
            Ctx.Prevalences.Add(new PrevalenceFilter() { RelativePrevalence = 1 });
        }

        private void XBtnPrevalenceFilters_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            int index = PrevalencesFiltersListView.Items.IndexOf(item);
            Ctx.Prevalences.RemoveAt(index);
            SettingsPersistence.Store(this.Ctx);
            this.AppStatus.Status = ApplicationStatusEnum.FilterChanged;
            this.AppStatus.RequiresNewPlaylist = true;
        }

        private void OnPrevalenceTextChange(object sender, TextChangedEventArgs args)
        {
            var sendingElement = (sender as FrameworkElement).DataContext;
            int index = PrevalencesFiltersListView.Items.IndexOf(sendingElement);
            var item = this.Ctx.Prevalences[index];

            var filterTerm = (sender as TextBox).Text;
            var isValid = this.filterValidator.IsValid(filterTerm);

            if (isValid)
            {
                item.AdditionalText = "Rescan required";
                SettingsPersistence.Store(this.Ctx); // da event noch nicht durch ist hier noch das alte zeug drin.
                this.AppStatus.Status = ApplicationStatusEnum.FilterChanged;
            }
            else
            {
                item.AdditionalText = "Filter invalid";
                this.AppStatus.Status = ApplicationStatusEnum.InvalidFilters;
            }
            this.AppStatus.RequiresNewPlaylist = true;

        }

        private void OnMainFilterTextChange(object sender, TextChangedEventArgs args)
        {
            var sendingElement = (sender as FrameworkElement).DataContext;
            int index = MainFiltersListView.Items.IndexOf(sendingElement);
            var item = this.Ctx.MainFilters[index];

            var filterTerm = (sender as TextBox).Text;
            var isValid = this.filterValidator.IsValid(filterTerm);

            if (isValid)
            {
                item.AdditionalText = "Rescan required";
                SettingsPersistence.Store(this.Ctx); // da event noch nicht durch ist hier noch das alte zeug drin.
                this.AppStatus.Status = ApplicationStatusEnum.FilterChanged;
            }
            else
            {
                item.AdditionalText = "Filter invalid";
                this.AppStatus.Status = ApplicationStatusEnum.InvalidFilters;
            }
            this.AppStatus.RequiresNewPlaylist = true;
        }

        private void OnLoseFocus(object sender, RoutedEventArgs e)
        {
            SettingsPersistence.Store(this.Ctx);
        }

        private void PrevalenceTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsDigit(e.Key))
            {
                e.Handled = true;
            }

            this.AppStatus.RequiresNewPlaylist = true;
        }

        private bool IsDigit(Key key)
        {
            // wtF????
            return key == Key.D1 || key == Key.D2 || key == Key.D3 || key == Key.D4 || key == Key.D5 || key == Key.D6 || key == Key.D7 || key == Key.D8 || key == Key.D9 || key == Key.D0 ||
                key == Key.NumPad0 || key == Key.NumPad1 || key == Key.NumPad2 || key == Key.NumPad3 || key == Key.NumPad4 || key == Key.NumPad5 || key == Key.NumPad6 || key == Key.NumPad7 || key == Key.NumPad8 || key == Key.NumPad9;
        }

        public void SetAdditionalInfoTexts(FileCountResult fileCountResult)
        {
            if (fileCountResult == null)
            {
                return;
            }

            TotalFileCountBeforeFilter.Content = $"{fileCountResult.FileCountBeforeMainFilters} files found"; 
            TotalFileCountAfterFilter.Content = $"{fileCountResult.FileCountAfterMainFilters} files remain after filtering";
            
            for (int i = 0; i < Ctx.MainFilters.Count; i++)
            {
                var count = fileCountResult.FileCountsForEachMainFilter.ElementAtOrDefault(i);

                if (count == -1)
                {
                    Ctx.MainFilters[i].AdditionalText = $"Invalid Filter";
                }
                else
                {
                    Ctx.MainFilters[i].AdditionalText = $"{count} files match";
                }
            }

            for (int i = 0; i < Ctx.Prevalences.Count; i++)
            {
                var count = fileCountResult.FileCountsForEachPrevalencePool.ElementAtOrDefault(i);

                if (count == -1)
                {
                    Ctx.Prevalences[i].AdditionalText = $"Invalid Filter";
                }
                else
                {
                    Ctx.Prevalences[i].AdditionalText = $"{count} files in pool";
                }
            }
        }
    }
}
