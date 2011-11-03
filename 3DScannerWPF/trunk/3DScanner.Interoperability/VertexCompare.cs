using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public class VertexCompare :IComparer<Vertex>
    {

        public int Compare(Vertex x, Vertex y)
        {
            double hx;
            hx = 37; // prime
            hx += x.Position.X * 27644437;
            hx += x.Position.Y * 1046527;
            hx += x.Position.Z * 877;
            hx += x.RGBB;
            hx += x.RGBG;
            hx += x.RGBR;
            hx *= 397;

            double hy;
            hy = 37; // prime
            hy += y.Position.X * 27644437;
            hy += y.Position.Y * 1046527;
            hy += y.Position.Z * 877;
            hy += y.RGBB;
            hy += y.RGBG;
            hy += y.RGBR;
            hy *= 397;
            if (hx > hy)
            {
                return 1;
            }
            else if (hy > hx)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
