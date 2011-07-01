using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public interface IOpslag
    {
        void PushPerspective(IProcessor i, Perspective perspectief);

        void RegisterPerspective(Perspective p);

        void GetPerspective(int i);

        void RegistrerProcessor(IProcessor processor);

        void RegistrerListener(IListener listener);

        int Count();

    }
}
