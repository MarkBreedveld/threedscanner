using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public interface IProcessor
    {
        void push(Perspective p);
    }
}
