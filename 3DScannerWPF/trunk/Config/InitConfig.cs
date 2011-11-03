using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DScanner.Export;
using _3DScanner.ExporterPlyWriter;
namespace Config
{
    public class InitConfig
    {

        private string _CONFIG = @"C:\\Program Files\\OpenNI\\Data\\SamplesConfig.xml";
        public string CONFIG
        {
            get { lock (_CONFIG) { return _CONFIG; } }
            internal set { lock (_CONFIG) { _CONFIG = value; } }
        }

        private Export _Export;
        public Export Export
        {
            get {
                if (_Export == null)
                {
                    _Export = new Export();
                    LoadExporters();
                }
                return _Export; 
            }
            internal set { _Export = value; }
        }

        private string _DefaultTarget = @"C:\\temp\\";
        public string DefaultTarget
        {
            get { return _DefaultTarget; }
            internal set { _DefaultTarget = value; }
        }

        void LoadExporters(){
            _Export.Registrer(new PlyWriter());
        }


        InitConfig()
        {
        }

        /*void LoadDefaultConfig()
        {
            CONFIG = @"C:\\Program Files (x86)\\OpenNI\\Data\\SamplesConfig.xml";
        }*/

        public static InitConfig Instance
        {
            get
            {
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

            internal static readonly InitConfig instance = new InitConfig();
        }
    }
}

