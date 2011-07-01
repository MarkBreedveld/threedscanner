using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public class Triangle : Face
    {
        Vertex p2;

        public Vertex Point2
        {
            get { return p2; }
            set { p2 = value; }
        }
        Vertex p3;

        public Vertex Point3
        {
            get { return p3; }
            set { p3 = value; }
        }

        public override string ToPlyString(IList<Vertex> v)
        {
            throw new NotImplementedException();
        }
    }
}
