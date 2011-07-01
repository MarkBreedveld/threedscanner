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
using System.Windows.Threading;

namespace _3DScanner.PerspectiveViewer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PrespectivesViewer : UserControl, IListener
    {

        IList<Perspective> perspectives = new List<Perspective>();

        public PrespectivesViewer()
        {
            InitializeComponent();
            PerspectiveGrid.ItemsSource = perspectives;
        }

        public void UpdatePerspectives(IList<Perspective> perspectief)
        {
            lock (perspectives)
            {
                perspectives = new List<Perspective>(perspectief);
            }

            PerspectiveGrid.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(update));
        }

        void update()
        {
            PerspectiveGrid.ItemsSource = perspectives;
        }

        void i_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO create mesh...
            throw new NotImplementedException();
        }

        void i_Selected(object sender, RoutedEventArgs e)
        {
            ListViewItem lvi = sender as ListViewItem;
            if (lvi != null)
            {
                Perspective p = (Perspective)lvi.Tag;
                ColorImage.Source = p.RawImageSource;
                DepthImage.Source = p.DepthBitmap;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            lock(this){
                foreach (Perspective p in PerspectiveGrid.SelectedItems)
                {
                    IList<Perspective> l = new List<Perspective>();
                    l.Add(p);
                    _3DScanner.GLMeshConstructor.SinglePerspectiveReconstructor.PerspectiefNaarEasyMesh(l);
                }
            }
        }
            
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sorry, This function is not yet implemented. But this project is still under active development. Please join us!");
            LOG.Instance.publishMessage("Sorry, This function is not yet implemented. But this project is still under active development. Please join us!");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PerspectiveGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lock(this){
                foreach (Perspective p in PerspectiveGrid.SelectedItems)
                {
                    ColorImage.Source = p.RawImageSource;
                    DepthImage.Source = p.DepthBitmap;
                }
            }
        }

        private void ColorImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}
