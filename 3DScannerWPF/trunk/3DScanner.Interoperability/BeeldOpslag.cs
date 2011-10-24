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
    public sealed class BeeldOpslag : IOpslag
    {
        private IList<Perspective> Perspectieven = new List<Perspective>();
        private LinkedList<IProcessor> Verwerkers = new LinkedList<IProcessor>();
        private LinkedList<IListener> luisteraars = new LinkedList<IListener>();
        private LinkedList<IMeshListener> MeshLuisteraars = new LinkedList<IMeshListener>();

        public void RegisterPerspective(Perspective p)
        {
            lock (Perspectieven)
            {
                Perspectieven.Add(p);
            }
            lock (luisteraars)
            {
                IEnumerator<IListener> e = luisteraars.GetEnumerator();
                while (e.MoveNext())
                {
                    e.Current.UpdatePerspectives(Perspectieven);
                }
            }
        }

        public void GetPerspective(int i)
        {
            lock (Perspectieven)
            {
                Perspectieven.ElementAt(i);
            }
        }

        public int Count()
        {
            lock (Perspectieven)
            {
                return Perspectieven.Count;
            }
        }

        BeeldOpslag()
        {
        }

        public static BeeldOpslag Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly BeeldOpslag instance = new BeeldOpslag();
        }

        public void PushPerspective(IProcessor i,Perspective Perspective)
        {
            lock (Verwerkers)
            {
                lock(Verwerkers.Find(i).Next.Value){
                    Verwerkers.Find(i).Next.Value.push(Perspective);
                }
            }
        }

        public void RegistrerProcessor(IProcessor verwerker)
        {
            lock (Verwerkers)
            {
                Verwerkers.AddFirst(verwerker);
            }
        }

        public void RegistrerListener(IListener i)
        {
            if (this.luisteraars.Contains(i)){ throw new ArgumentException(" IListener is al geregistreerd."); }
            this.luisteraars.AddFirst(i);
        }

        public void registerMesh(Mesh m, Perspective p)
        {
            lock (Perspectieven)
            {
                lock (p)
                {
                    lock (m)
                    {
                        p.Mesh = m;
                    }
                }
                foreach (IMeshListener l in MeshLuisteraars)
                {
                    l.updateMeshes(Perspectieven.Where(f => f.Mesh != null).Select(t => t.Mesh).ToList<Mesh>());
                }
            }
        }

        public void registerIMeshListener(IMeshListener l)
        {
            lock (l)
            {
                lock (MeshLuisteraars)
                {
                    MeshLuisteraars.AddFirst(l);
                }
            }
        }
    }
}
