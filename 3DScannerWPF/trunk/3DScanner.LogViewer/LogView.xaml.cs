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
namespace _3DScanner.LogViewer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LogView : UserControl,ILOGListener
    {

        private delegate void UpdateLog(string message);

        public LogView()
        {
            InitializeComponent();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            LOG.Instance.clear();
        }

        public void clear()
        {
            Thread t = new Thread(new ThreadStart(
                delegate
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(clearLog));
                }
                ));
            t.Start();
        }

        private void clearLog()
        {
            this.logTextBlock.Text = "";
        }

        public void publishMessage(string Message)
        {
            Thread t = new Thread(new ThreadStart(
                delegate
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<string>(SetValue), Message);
                }
                ));
            t.Start();
        }

        private void SetValue(string txt)
        {
            logTextBlock.Text = txt + "\r\n" + logTextBlock.Text; 
        }
    }
}
