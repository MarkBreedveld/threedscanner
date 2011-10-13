using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenNI;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using _3DScanner.Interoperability;
using Config;
//Het engelse gedeelte is code van kai
//Maar omdat de code kris kras door de applicatie verdwijnt
//Zal niet meer te herkennen zijn wat van hem is of nog maar te zeggen dat het van hem is
//Toch ter ere van zijn werk noteren we zijn naam Kai.
namespace _3DScanner.Scanner
{
    public class BeeldVerwerker
    {
        private static bool configurated;
        public static bool Configurated
        {
            get { return BeeldVerwerker.configurated; }
        }
        
        private static int counter = 0;

        private Context context;
        /// <summary>
        /// Image camera source.
        /// </summary>
        private WriteableBitmap _imageBitmap;

        BeeldVerwerker()
        {
            
        }

        public void SetupScanner(string Config){
            lock (this)
            {
                try
                {
                    this.context = new Context(Config);
                    this.GDepth = context.FindExistingNode(NodeType.Depth) as DepthGenerator;
                    this.GImage = context.FindExistingNode(NodeType.Image) as ImageGenerator;
                    if (this.GDepth == null)
                    {
                        throw new Exception("Viewer must have a depth node!");
                    }
                    if (this.GImage == null)
                    {
                        throw new Exception("3D Scanner must have a image node!");
                    }
                    MapOutputMode mapMode = this.GDepth.MapOutputMode;
                    configurated = true;
                }
                catch (Exception e)
                {
                    LOG.Instance.publishMessage("ERROR : " + e.Message + " " + e.InnerException);
                    configurated = false;
                }
            }
        }

        public static BeeldVerwerker Instance
        {
            get
            {
                if (!configurated)
                {
                    Nested.instance.SetupScanner(InitConfig.Instance.CONFIG);
                    if (!configurated) { throw new GeneralException(" Scanner kan niet worden geconfigureerd met standaard configuratie."); }
                }
                return Nested.instance;
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly BeeldVerwerker instance = new BeeldVerwerker();
        }

        private unsafe void ReaderThread()
        {
            lock(this){
                //Het engelse gedeelte is code van kai
                //Maar omdat de code kris kras door de applicatie verdwijnt
                //Zal niet meer te herkennen zijn wat van hem is of nog maar te zeggen dat het van hem is
                //Toch ter ere van zijn werk noteren we zijn naam Kai.

                //TextWriter tw = new StreamWriter("d:\\temp\\depthdump.txt");
                int[] depth;
                UInt16[] rgb;
                try { this.context.WaitAndUpdateAll(); }catch (Exception) { }
                Perspective p = new Perspective(DateTime.Now);
                GImage.GetMetaData(p.ImgMD );
                GDepth.GetMetaData(p.DepthMD);

                BeeldOpslag.Instance.RegisterPerspective(p);
                LOG.Instance.publishMessage("Start take picture thread ID " + Thread.CurrentThread.ManagedThreadId +" DONE");
            }
        }

        public void TakePicture()
        {
            
            Thread t = new Thread(ReaderThread);
            t.Start();
            LOG.Instance.publishMessage("Start take picture thread...ID " + t.ManagedThreadId);
        }

        public bool shouldRun { get; set; }

        public Thread readerThread { get; set; }

        public DepthGenerator GDepth { get; set; }

        public ImageGenerator GImage { get; set; }
    }
}
