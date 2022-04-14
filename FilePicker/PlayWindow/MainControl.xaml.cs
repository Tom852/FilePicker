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


namespace FilePicker.PlayWindow
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public Playlist PlayList { get; }

        public MainControl(Playlist playlist)
        {
            this.PlayList = playlist;
            this.DataContext = this.PlayList;

            InitializeComponent();
         }


        private void PlayBtn_OnClick(object sender, RoutedEventArgs e)
        {
            
            FileHandler.Open(this.PlayList.Current);
            this.PlayList.RotateForward();
        }

        private void BackBtn_OnClick(object sender, RoutedEventArgs e)
        {

            this.PlayList.RotateBackward();
        }

        private void FwdBtn_OnClick(object sender, RoutedEventArgs e)
        {
            this.PlayList.RotateForward();
        }

        private void PlayAgain(object sender, MouseButtonEventArgs e)
        {
            var tag = ((FrameworkElement)sender).Tag.ToString();
            PerformActionOnTaggedElement(tag, FileHandler.Open);
        }

        private void ShowInExplorer(object sender, MouseButtonEventArgs e)
        {
            var tag = ((FrameworkElement)sender).Tag.ToString();
            PerformActionOnTaggedElement(tag, FileHandler.ShowInExplorer);
        }

        private void PerformActionOnTaggedElement(string tag, Action<FileRepresentation> action)
        {
            switch (tag)
            {
                case "PrevPrev":
                    action(PlayList.PrevPrev);
                    return;
                case "Prev":
                    action(PlayList.Prev);
                    return;
                case "Current":
                    action(PlayList.Current);
                    return;
                case "Next":
                    action(PlayList.Next);
                    return;
                case "NextNext":
                    action(PlayList.NextNext);
                    return;
                default:
                    throw new ApplicationException("Invalid Tag");
            }
        }

        
    }
}
