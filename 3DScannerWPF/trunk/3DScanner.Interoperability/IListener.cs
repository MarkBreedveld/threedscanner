using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public interface IListener
    {
        //Er is een nieuw perspectief toegevoegd
        void UpdatePerspectives(IList<Perspective> perspectief);
    }
}
