using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DScanner.Interoperability
{
    public class LOG
    {
        
        LinkedList<ILOGListener> listeners = new LinkedList<ILOGListener>();
        LinkedList<string> buffer = new LinkedList<string>();
        
        LOG()
        {
        }

        public void add(ILOGListener l)
        {
            lock (this)
            {
                listeners.AddFirst(l);
                foreach (string m in buffer)
                {
                    l.publishMessage(m);
                }
            }
        }

        public void remove(ILOGListener l)
        {
            lock (listeners)
            {
                listeners.Remove(l);
            }
        }

        public void publishMessage(string m)
        {
            lock (buffer)
            {
                buffer.AddFirst(m);
            }
            lock (listeners)
            {
                foreach (ILOGListener l in listeners)
                {
                    l.publishMessage(m);
                }
            }
            Console.WriteLine(m);
        }

        public void clear()
        {
            lock (this)
            {
                buffer.Clear();
                foreach (ILOGListener l in listeners)
                {
                    l.clear();
                }
            }
        }

        public static LOG Instance
        {
            get
            {
                lock (Nested.instance)
                {
                    return Nested.instance;
                }
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly LOG instance = new LOG();
        }

    }
}
