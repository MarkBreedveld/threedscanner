using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DScanner.Export;
using _3DScanner.Interoperability;
using System.IO;
using OpenTK;
namespace _3DScanner.ExporterPlyWriter
{
    public class PlyWriter : Export.Exporter
    {

        public override string Exporteer(Mesh mesh, string target, string[] param)
        {

            TextWriter tw = new StreamWriter(target);
            tw.Write("ply\n");
	        tw.Write("format ascii 1.0\n");
	        tw.Write("comment Kai Kostack Kinect 3D scanner generated\n");
	        tw.Write("element vertex {0:d}\n", mesh.Vertices.Count);
	        tw.Write("property float x\n");
	        tw.Write("property float y\n");
	        tw.Write("property float z\n");
	        tw.Write("property uchar red\n");
	        tw.Write("property uchar green\n");
	        tw.Write("property uchar blue\n");
	        tw.Write("element face {0:d}\n", mesh.Faces.Count);
	        tw.Write("property list uchar int vertex_indices\n");
	        tw.Write("end_header\n");	

            //write the vertices to the file
            IEnumerator<Vertex> e = mesh.Vertices.GetEnumerator();
            while(e.MoveNext()){
                Vertex temp = e.Current;
                if (float.IsNaN(temp.Position.X) || float.IsInfinity(temp.Position.X) || temp.Position.X < -100 || temp.Position.X > 100) { temp.Position.X = 0; }
                if (float.IsNaN(temp.Position.Y) || float.IsInfinity(temp.Position.Y) || temp.Position.Y < -100 || temp.Position.Y > 100) { temp.Position.Y = 0; }
                if (float.IsNaN(temp.Position.Z) || float.IsInfinity(temp.Position.Z) || temp.Position.Z < -100 || temp.Position.Z > 100) { temp.Position.Z = 0; }

                tw.Write("{0:g} ", temp.Position.X);
                tw.Write("{0:g} ", temp.Position.Y);
                tw.Write("{0:g} ", temp.Position.Z);
                            
                tw.Write(temp.RGBR + " ");
                tw.Write(temp.RGBG + " ");
                tw.Write(temp.RGBB + "\n");
            }

            //Write the faces to the file
            int counter = 0; 
            IEnumerator<Face> f = mesh.Faces.GetEnumerator();
            while (f.MoveNext())
            {
                counter++;
                if(f.Current is Quad){
                    Quad v = (Quad) f.Current;
                    tw.Write("4 {0:d} {0:d} {0:d} {0:d}\n",mesh.Vertices.IndexOf(v.Point1),mesh.Vertices.IndexOf(v.Point2),mesh.Vertices.IndexOf(v.Point3),mesh.Vertices.IndexOf(v.Point4));
                }else if(f.Current is Triangle){
                    Triangle t = (Triangle)f.Current;
                    tw.Write("3 {0:d} {0:d} {0:d}\n",mesh.Vertices.IndexOf(t.Point1),mesh.Vertices.IndexOf(t.Point2),mesh.Vertices.IndexOf(t.Point3));
                }else{
                    throw new ArgumentException("Unknown face detected while writing ply format. Update the ply writer or downgrade reconstructor.");
                }
            }
	        tw.Close();
            return Exporter.EXPORT_OK;
        }
       
        public override string getName()
        {
            return "Stanford PLY";
        }
        
        public override string getVendor()
        {
            return "Stanford's implementation by Kai";
        }
        
        public override int getVersion()
        {
            return 1;
        }
        
        public override string getExtension()
        {
            return "PLY";
        }
        
        public override string getSummary()
        {
            return "This version is the ascii based ply format, with vertices, faces and color.";
        }
        
        public override string[] getParams()
        {
            return null;
        }

        public override string ToString()
        {
            return getName();
        }
    }
}

