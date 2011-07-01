using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public interface IMeshListener
    {
        void updateMeshes(IList<Mesh> mesh);

        void clear();
    }
}
