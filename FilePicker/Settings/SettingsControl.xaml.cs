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

namespace FilePicker.Settings
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsModel Ctx { get; set; }

        public SettingsControl(SettingsModel settings)
        {
            InitializeComponent();
            this.Ctx = settings;
            DataContext = this.Ctx;
        }


        private void RemoveFolders_Click(object sender, RoutedEventArgs e)
        {
            var badboys = this.Folders.SelectedItems.ConvertAndCastToGenericList<string>();
            foreach (var item in badboys)
            {
                this.Ctx.Folders.Remove(item);
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
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsDigit(e.Key))
            {
                e.Handled = true;
            }
        }

        private bool IsDigit(Key key)
        {
            // wtF????
            return key == Key.D1 || key == Key.D2 || key == Key.D3 || key == Key.D4 || key == Key.D5 || key == Key.D6 || key == Key.D7 || key == Key.D8 || key == Key.D9 || key == Key.D0 ||
                key == Key.NumPad0 || key == Key.NumPad1 || key == Key.NumPad2 || key == Key.NumPad3 || key == Key.NumPad4 || key == Key.NumPad5 || key == Key.NumPad6 || key == Key.NumPad7 || key == Key.NumPad8 || key == Key.NumPad9;
        }
    }
}
