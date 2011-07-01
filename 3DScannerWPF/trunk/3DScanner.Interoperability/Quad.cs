using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public class Quad : Face
    {
        Vertex p2;

        public Vertex Point2
        {
            get { return p2; }
            set { p2 = value; }
        }

        private Vertex p3;
        public Vertex Point3
        {
            get { return p3; }
            set { p3 = value; }
        }

        private Vertex p4;
        public Vertex Point4
        {
            get { return p4; }
            set { p4 = value; }
        }

        /*public override string ToPlyString(IList<Vertex> v){
            return 
        }*/

        public override string ToPlyString(IList<Vertex> v)
        {
            throw new NotImplementedException();
        }
    }
}
