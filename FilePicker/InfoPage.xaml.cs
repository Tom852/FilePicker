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

namespace FilePicker
{
    /// <summary>
    /// Interaction logic for InfoPage.xaml
    /// </summary>
    public partial class InfoPage : UserControl
    {
        public InfoPage()
        {
            InitializeComponent();

            this.InfoText.Text =
@"This help page is just a short reference for experienced users. If you are new, please visit https://github.com/Tom852/FilePicker for a full manual.

First, go to the Settings Page. On the left, you can add the folders you want to be considered. To remove any, mark them first and then press the X.
On the next column, you can add prefilters. They are connected and-wise. Filter guideline -> See below.
On the last column, you can add filter-terms and a relative prevalence of files matching your filter-condition.

When you are setup, hit the scan-button to scan your files. Rate is about 3000 files per second on a SSD.
Then, you can start opening random files according to your criteria.

Filter variables: Directory, Name, Extension, FullPath, CreatedAt, DaysSinceCreation, ModifiedAt, DaysSinceLastModified, SizeB, SizeK, SizeM, SizeG
Filter Opreators: =  !=  <  >  <=  >=  =* (Contains)  !* (Not Contains)   ^ (StartsWith)   !^ (NotStartsWith)   $ (EndsWith)   !$ (NotEndsWith)
Fitler Syntax: , (And)    | (Or)      () (Paranthesis)";

        }
    }
}
