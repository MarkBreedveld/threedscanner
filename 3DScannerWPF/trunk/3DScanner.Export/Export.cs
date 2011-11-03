using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DScanner.Interoperability;

namespace _3DScanner.Export
{
    public class Export 
    {
        private LinkedList<Exporter> Exporters = new LinkedList<Exporter>();

        public Exporter[] GetExporters
        {
            get { return Exporters.ToArray(); }
        }

        /// <summary>
        /// Adds a exporter into the application, but throws a ArgumentNullException when the argument is null
        /// </summary>
        /// <param name="exporter">Should be an exporter with the IExporter interface</param>
        public void Registrer(Exporter exporter)
        {
            if (exporter == null) { throw new ArgumentNullException(); }
            this.Exporters.AddFirst(exporter);
        }

        /// <summary>
        /// Exports the current mesh to the given format en target with some optional parameters depeding the exporter
        /// </summary>
        /// <param name="exporter">The exporter who has to export the format</param>
        /// <param name="mesh">The model/mesh to export</param>
        /// <param name="target">File location of the mesh</param>
        /// <param name="param">Optional params such as colors, but that depens on the exporter</param>
        /// <returns> Return</returns>
        public string Exporteer(Exporter exporter, Mesh mesh, string target, string[] param)
        {
            if (exporter == null) { throw new ArgumentNullException(); }
            if (mesh == null) { throw new ArgumentNullException(); }

            IEnumerator<Exporter> exporters = this.Exporters.GetEnumerator();
            while (exporters.MoveNext())
            {
                if (exporter.GetType() == exporters.Current.GetType())
                {
                    string result;
                    try
                    {
                        result = exporters.Current.Exporteer(mesh, target, param);
                    }
                    catch (Exception e)
                    {
                        result = e.Message + " : inner -> " + e.InnerException;
                    }
                    LOG.Instance.publishMessage(result);
                    return result;
                }
            }
            throw new ArgumentException("The given export format was not registrert or could not befound by this factory. Please register the format.");
        }
    }
}
