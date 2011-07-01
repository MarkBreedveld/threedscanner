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
using Config;
using _3DScanner.LiveOverview;
using System.Configuration;
using System.Collections.Generic;
using WPF.Themes;
using _3DScanner.Interoperability;

namespace _3DScannerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IList<KinectRawViewer.RawKinectViewer> viewers = new List<KinectRawViewer.RawKinectViewer>();

        public MainWindow()
        {
            InitializeComponent();
        }

        void verifySettings()
        {
            /*IList<string> keys = new List<string>();
            keys.Add("KinectConfigFile");
            System.Collections.IEnumerator e = keys.GetEnumerator();
            
            while (e.MoveNext())
            {
                if (!ConfigurationSettings.AppSettings.AllKeys.Contains(e.Current))
                {
                    if (e.Current.Equals("KinectConfigFile"))
                    {
                        ConfigurationSettings.AppSettings.Add("KinectConfigFile", "C:\\Program Files (x86)\\OpenNI\\Data\\SamplesConfig.xml");
                    }
                }   
            }*/
        }
        
        private void themes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string theme = e.AddedItems[0].ToString();

                // Window Level
                // this.ApplyTheme(theme);

                // Application Level
                // Application.Current.ApplyTheme(theme);
            }
        }    

        private void LiveMultiViewer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            themes.ItemsSource = ThemeManager.GetThemes();
            LiveMultiViewer.ResetViewers(Config.InitConfig.Instance.CONFIG);
            BeeldOpslag.Instance.RegistrerListener(this.prespectivesViewer1);

            BeeldOpslag.Instance.registerIMeshListener(meshView1);
            //Initialize log viewer
            LOG.Instance.add(LogView2);
        }
    }
}
