using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public class Mesh
    {
        private DateTime _CreateDate = DateTime.Now;

        public DateTime CreateDate
        {
            get { return _CreateDate; }
            internal set{ _CreateDate = value; }
        }
        public Mesh(IList<Face> f,IList<Vertex> v){
            faces = f;
            long i = 0;
            Vertex last;
            vertices = new SortedList<Vertex, long>(new VertexCompare());
            foreach(Vertex vertex in v){
                //Remove double vertices
                if (!vertices.ContainsKey(vertex))
                {
                    vertices.Add(vertex, i);
                    i++;
                    last = vertex;
                }
            }
        }

        IList<Face> faces;
        public IList<Face> Faces
        {
            get { return faces; }
            set { faces = value; }
        }

        SortedList<Vertex, long> vertices;
        public SortedList<Vertex, long> Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        public override string ToString()
        {
            return "Mesh created on " + _CreateDate.ToShortTimeString();
        }
    }
}
