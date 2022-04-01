using FilePicker.DataAccess;
using FilePicker.Scanner;
using FilePicker.Settings;
using System;
using System.Collections.Generic;
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
using Gridify;
namespace FilePicker
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        private readonly IQueryable<FileRepresentation> preFilteredData;
        private readonly SettingsModel settings;

        public MainControl(IEnumerable<FileRepresentation> data)
        {
            InitializeComponent();
            this.settings = SettingsPersistence.Load();
            this.preFilteredData = data.AsQueryable();
            foreach (var filter in this.settings.MainFilters)
            {
                try
                {
                    this.preFilteredData = this.preFilteredData.ApplyFiltering(filter.FilterExpression);
                }
                catch (Exception e)
                {
                    var error = e.ToString();
                }
            }
        }

        private void PlayBtn_OnClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<FileRepresentation> filesToPickFrom;
            if (this.settings.Prevalences.Any())
            {
                int maxValPrev = this.settings.Prevalences.Sum(p => p.RelativePrevalence);
                int edgeValue = new Random().Next(maxValPrev);
                int current = 0;
                var targetFilter = this.settings.Prevalences.SkipWhile(item =>
                {
                    current += item.RelativePrevalence;
                    return !(current > edgeValue);
                }).First();
                try
                {
                    filesToPickFrom = this.preFilteredData.ApplyFiltering(targetFilter.FilterExpression);
                }
                catch
                {
                    filesToPickFrom = this.preFilteredData;
                }
            }
            else
            {
                filesToPickFrom = this.preFilteredData;
            }

            if (filesToPickFrom.Count() == 0)
            {
                // todo würd hjier eher ne exception udn das im UI schon anzzeigen
                return;
            }

            int targetIndex = new Random().Next(filesToPickFrom.Count());
            var targetFile = filesToPickFrom.ElementAt(targetIndex);
            FileHandler.Open(targetFile);

            this.BaseFolder.Content = targetFile.Directory;
            this.File.Content = targetFile.Name;
            this.Date.Content = targetFile.ModifiedAt;

        }

        private void PlayAgain(object sender, MouseButtonEventArgs e)
        {
            //LastPlayed?.Open();
        }

        private void ShowInExplorer(object sender, MouseButtonEventArgs e)
        {
            //LastPlayed?.ShowInExplorer();
        }
    }
}
