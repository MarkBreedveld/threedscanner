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
    }
}
