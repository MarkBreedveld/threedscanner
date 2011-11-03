using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using System.Runtime.InteropServices;

namespace _3DScanner.Interoperability
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    { // mimic InterleavedArrayFormat.T2fN3fV3f
        public Vector2 TexCoord;
        public Vector3 Normal;
        public Vector3 Position;
        public UInt16 RGBR;
        public UInt16 RGBB;
        public UInt16 RGBG;

        public override int GetHashCode()
        {
            unchecked
            {
                double result = 37; // prime
                result *= 397; // also prime (see note)
                result += Position.X;
                result += Position.Y;
                result += Position.Z;
                result += RGBB;
                result += RGBG;
                result += RGBR;
                result *= 100000000000000;
                result *= 397;
                
                return (int)result;
            }
        }

        public override bool Equals(object obj)
        {
            if(obj is Vertex){
                Vertex y = (Vertex)obj;
                if ((Position.X == y.Position.X) & (Position.Y == y.Position.Y) & (Position.Z == y.Position.Z))
                {
                    if ((this.RGBB == y.RGBB) & (this.RGBG == y.RGBR) & (y.RGBR == this.RGBR))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
