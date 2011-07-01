using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DScanner.Interoperability;

namespace _3DScanner.Export
{
    public abstract class Exporter
    {

        public static readonly string EXPORT_OK = "Export Succesvol";

        public abstract string Exporteer(Mesh mesh, string target, string[] param);

        public abstract string getName();

        public abstract string getVendor();

        public abstract int getVersion();

        public abstract string getExtension();

        public abstract string getSummary();

        public abstract string[] getParams();

    }
}
