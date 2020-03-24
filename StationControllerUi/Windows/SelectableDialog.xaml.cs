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
using System.Windows.Shapes;

namespace StationControllerUi.Windows
{
    /// <summary>
    /// Interaction logic for SelectableDialog.xaml
    /// </summary>
    public partial class SelectableDialog : Window
    {
        public SelectableDialog(string ipAddresses)
        {
            InitializeComponent();
            tbIps.Text = ipAddresses;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
