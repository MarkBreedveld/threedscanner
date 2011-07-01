using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public interface ILOGListener
    {
        void clear();

        void publishMessage(string Message);
    }
}
