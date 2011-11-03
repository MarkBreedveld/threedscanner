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
using System.Threading;
using System.Windows.Threading;

namespace _3DScanner.MeshViewer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MeshView : UserControl,IMeshListener
    {
        IList<Mesh> meshes = new List<Mesh>();
        Thread t;
        public MeshView()
        {
            InitializeComponent();
            MeshGrid.ItemsSource = meshes;
        }

        private void UpdateSource()
        {
            lock (meshes)
            {
                MeshGrid.ItemsSource = meshes;
            }
        }

        public void updateMeshes(IList<Mesh> mesh)
        {
            lock (meshes)
            {
                meshes = new List<Mesh>(mesh);
                Thread t = new Thread(new ThreadStart(
                    delegate
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(UpdateSource));
                    }
                    ));
                t.Start();
            }
        }

        public void clear()
        {
            Thread t = new Thread(new ThreadStart(
                delegate
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(clearListSource));
                }
                ));
            t.Start();
        }

        private void clearListSource()
        {
            lock (meshes)
            {
                this.meshes.Clear();
                MeshGrid.ItemsSource = meshes;
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            string filename;
            string extension;
            lock (this)
            {
                if (ExportComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show(" You need to select a exportformat.");
                }
                else
                {
                    filename = TargetTextBox.Text + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                    extension = ((Export.Exporter)ExportComboBox.SelectedItem).getExtension();
                    Export.Exporter exporter = (Export.Exporter)ExportComboBox.SelectedItem;
                    LOG.Instance.publishMessage("START EXPORT TO " + ExportComboBox.SelectedItem.ToString());
                    LOG.Instance.publishMessage("This might take while, please wait till the ready message appears here.");
                    foreach (Mesh m in this.MeshGrid.SelectedItems)
                    {
                        //Execute exporting into seperate thread
                        t = new Thread(() => Config.InitConfig.Instance.Export.Exporteer(exporter, m, filename + "." + extension, null));
                        t.Start();
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TargetTextBox.Text = Config.InitConfig.Instance.DefaultTarget;
            ExportComboBox.ItemsSource = Config.InitConfig.Instance.Export.GetExporters;
        }
    }
}
