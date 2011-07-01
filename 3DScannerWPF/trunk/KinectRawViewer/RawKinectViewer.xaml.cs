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
using System.ComponentModel;
using xn;

namespace KinectRawViewer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// code geript van http://www.studentguru.gr/blogs/vangos/archive/2011/01/28/kinect-and-wpf-getting-the-raw-and-depth-image-using-openni.aspx
    /// Published 28 Ιανουαρίου 2011 09:00 μμ by Vangos 
    /// </summary>
    public partial class RawKinectViewer : UserControl
    {
        Nui.NuiSensor _sensor;
        BackgroundWorker _worker = new BackgroundWorker();

        public RawKinectViewer()
        {
            InitializeComponent();
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        public void StartViewer(string CONFIG)
        {
            

            _sensor = new Nui.NuiSensor(CONFIG);

            _worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
        }

        public void StartViewer(Context c, ImageGenerator ig, DepthGenerator dg)
        {
            _sensor = new Nui.NuiSensor();
            //_sensor.
            //TODO
        }

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)delegate
            {
                imgRaw.Source = _sensor.RawImageSource;
                imgDepth.Source = _sensor.DepthImageSource;
            });
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!_worker.IsBusy)
            {
                _worker.RunWorkerAsync();
            }
        }

        void Dispatcher_ShutdownStarted(object sender, EventArgs e )
        {
            if (_sensor != null){
                _sensor.Dispose();
            }
        }

    }
}
