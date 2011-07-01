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
            vertices = v;
        }

        IList<Face> faces;
        public IList<Face> Faces
        {
            get { return faces; }
            set { faces = value; }
        }

        IList<Vertex> vertices;
        public IList<Vertex> Vertices
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
