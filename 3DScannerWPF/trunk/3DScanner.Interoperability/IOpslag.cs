using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * Opslag means storage!
 * So this is the storage interface which has currently one implementation 
 * The beeldopslag which translated means image storage.
 * Storing the perspectives in a thread save way
 */
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
