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
using xn;
using KinectRawViewer;
using _3DScanner.Interoperability;

namespace _3DScanner.LiveOverview
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LiveMultiViewer : UserControl
    {

        IList<string> configs = new List<string>();

        public LiveMultiViewer()
        {
            InitializeComponent();
        }

        public void ResetViewers(string CONFIG)
        {
            LOG.Instance.publishMessage("Init liveviewers...");
            configs = new List<string>();
            configs.Add(CONFIG);
            try
            {
                RawKinectViewer r = new RawKinectViewer();
                r.StartViewer(CONFIG);
                viewers.Children.Add(r);
                RawKinectViewer r1 = new RawKinectViewer();
                r.StartViewer(CONFIG);
                viewers.Children.Add(r1);
            }
            catch  (Exception e) {
                LOG.Instance.publishMessage("ERROR : " + e.Message);
                Label l = new Label();
                l.Content = "Configuratie file kon niet worden gevonden.";
                l.Height = 20;
                viewers.Children.Add(l);
            }
            LOG.Instance.publishMessage("Init liveviewers DONE");
        }

        public void ResetViewers(IList<string> CONFIGURATIONS)
        {
            configs = CONFIGURATIONS;
            viewers.Children.Clear();
            ListViewItem l  = new ListViewItem();
            l.Content = "Werkt nog niet";
            LOG.Instance.publishMessage("Werkt nog niet");
            viewers.Children.Add(l);
            /*
            IList<Generator> devices = new List<Generator>();
            IEnumerator<string> configs = CONFIGURATIONS.GetEnumerator();
            while (configs.MoveNext())
            {
                try
                {
                    xn.Context c = new Context(configs.Current);
                    IEnumerator<NodeInfo> nil = c.EnumerateExistingNodes().GetEnumerator();
                    while (nil.MoveNext())
                    {
                        DepthGenerator dg = nil.Current.GetInstance() as DepthGenerator;
                        if (dg != null)
                        {
                            devices.Add(dg);
                        }
                        ImageGenerator ig = nil.Current.GetInstance() as ImageGenerator;
                        if (ig != null)
                        {
                            devices.Add(ig);
                        }
                    }
                    break;
                }catch(Exception){
                    devices.Clear();
                }
            }
            
            for(int i = 0 ; i < devices.Count ; i++){
                RawKinectViewer r =new RawKinectViewer(devices.ElementAt(i)
                KinectViewListView.Items.Add
                deviceit.Current
            }
            */
        }

        
        private void load1Cam()
        {
            try
            {
                xn.Context c = new Context();
            }
            catch (Exception)
            {

            }
        }

        private void AddKinectButton_Click(object sender, RoutedEventArgs e)
        {
            ResetViewers(configs);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
