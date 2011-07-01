using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public abstract class Face
    {
        Vertex point1;

        public Vertex Point1
        {
            get { return point1; }
            set { point1 = value; }
        }

        public override string ToString()
        {
            return point1.ToString();
        }

        public abstract string ToPlyString(IList<Vertex> v);
    }
}
