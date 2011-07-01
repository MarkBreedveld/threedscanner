using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _3DScanner.Interoperability;

namespace _3DScanner.ScannerUI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ScannerController : UserControl
    {
        public ScannerController()
        {
            InitializeComponent();
        }

        private void TakePerspectiveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Scanner.BeeldVerwerker.Instance.TakePicture();
            }
            catch (Exception ex)
            {
                LOG.Instance.publishMessage("ERROR : " + ex.Message + " " + ex.InnerException);
            }
        }

    }
}
