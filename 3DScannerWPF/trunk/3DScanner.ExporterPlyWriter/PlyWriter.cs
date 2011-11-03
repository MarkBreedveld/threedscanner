using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _3DScanner.Export;
using _3DScanner.Interoperability;
using System.IO;
using OpenTK;
using System.Threading;
using System.Collections;

namespace _3DScanner.ExporterPlyWriter
{
    public class PlyWriter : Export.Exporter
    {
        Queue notsafeBuffer = new Queue();
        Queue buffer;
        
        public override string Exporteer(Mesh mesh, string target, string[] param)
        {
            buffer = Queue.Synchronized(notsafeBuffer);
            buffer.Enqueue(String.Format("ply\n"));
	        buffer.Enqueue(String.Format("format ascii 1.0\n"));
	        buffer.Enqueue(String.Format("comment Kai Kostack Kinect 3D scanner generated\n"));
	        buffer.Enqueue(String.Format("element vertex {0:d}\n", mesh.Vertices.Count));
	        buffer.Enqueue(String.Format("property float x\n"));
	        buffer.Enqueue(String.Format("property float y\n"));
	        buffer.Enqueue(String.Format("property float z\n"));
	        buffer.Enqueue(String.Format("property uchar red\n"));
	        buffer.Enqueue(String.Format("property uchar green\n"));
	        buffer.Enqueue(String.Format("property uchar blue\n"));
	        buffer.Enqueue(String.Format("element face {0:d}\n", mesh.Faces.Count));
	        buffer.Enqueue(String.Format("property list uchar int vertex_indices\n"));
	        buffer.Enqueue(String.Format("end_header\n"));	
            double totalOperations = mesh.Faces.Count +  mesh.Vertices.Count;
            int counterProgress = 0;
            //write the vertices to the file
            IEnumerator<KeyValuePair<Vertex,long>> e = mesh.Vertices.GetEnumerator();
            while(e.MoveNext()){
                Vertex temp = e.Current.Key;
                if (float.IsNaN(temp.Position.X) || float.IsInfinity(temp.Position.X) || temp.Position.X < -100 || temp.Position.X > 100) { temp.Position.X = 0; }
                if (float.IsNaN(temp.Position.Y) || float.IsInfinity(temp.Position.Y) || temp.Position.Y < -100 || temp.Position.Y > 100) { temp.Position.Y = 0; }
                if (float.IsNaN(temp.Position.Z) || float.IsInfinity(temp.Position.Z) || temp.Position.Z < -100 || temp.Position.Z > 100) { temp.Position.Z = 0; }

                string line = String.Format("{0:g} ", temp.Position.X) +  String.Format("{0:g} ", temp.Position.Y) + String.Format("{0:g} ", temp.Position.Z) +String.Format(temp.RGBR + " ") + String.Format(temp.RGBG + " ") + String.Format(temp.RGBB + "\n");
                buffer.Enqueue(line);

                counterProgress++;
                if ((counterProgress % 1000 == 0) & (counterProgress >999))
                {
                    LOG.Instance.publishMessage("EXPORT TO " + getName() + " is for " + counterProgress + " of " + totalOperations + " done. Fase Vertices.");
                }
            }
            //Write flush buffer in the background
            Thread thread = new Thread(() => emptyBuffer(buffer.Count,target));
            thread.Start();

            //Write the faces to the file
            int counter = 0; 
            IEnumerator<Face> f = mesh.Faces.GetEnumerator();
            while (f.MoveNext())
            {
                counter++;
                if(f.Current is Quad){
                    Quad v = (Quad) f.Current;
                    buffer.Enqueue(String.Format("4 {0:d} {0:d} {0:d} {0:d}\n", mesh.Vertices[v.Point1], mesh.Vertices[v.Point2], mesh.Vertices[v.Point3], mesh.Vertices[v.Point4]));
                }else if(f.Current is Triangle){
                    Triangle t = (Triangle)f.Current;
                    buffer.Enqueue(String.Format("3 {0:d} {0:d} {0:d}\n", mesh.Vertices[t.Point1], mesh.Vertices[t.Point2], mesh.Vertices[t.Point3]));
                }else{
                    throw new ArgumentException("Unknown face detected while writing ply format. Update the ply writer or downgrade reconstructor.");
                }

                //report progress
                counterProgress++;
                if (counterProgress % 10000 == 0) {
                    
                    LOG.Instance.publishMessage("EXPORT TO " + getName() + " is for " + counterProgress + " of " + totalOperations + " done. Fase faces."); 
                }
            
            }

            // Waiting untill the last writing action is done
            thread.Join();
            //Write last part of the file
            thread = new Thread(() => emptyBuffer(buffer.Count, target));
            thread.Start();
            thread.Join();
            LOG.Instance.publishMessage("START EXPORT TO " + getName() + " DONE");
            return Exporter.EXPORT_OK;
        }

        private void emptyBuffer(int i, string target)
        {
            TextWriter tw = new StreamWriter(target,true);
            string text="";
            for (int c = 0; c < i; c++)
            {
                tw.Write(buffer.Dequeue().ToString().Replace(',','.'));
            }
            //Could happen that the file is not a multiply of 10000 P)
            if (text != "") { tw.Write(text); }
            tw.Close();
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

