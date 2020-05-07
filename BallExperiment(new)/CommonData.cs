using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BallExperiment_new_
{
    class CommonData
    {
        private Queue<Ball>[] vals = {                  
            new Queue<Ball>(), 
            new Queue<Ball>(), 
            new Queue<Ball>() 
            };
        public void Add(Ball ball, int index)
        {
            var q = vals;
            Monitor.Enter(q);
            try
            {
                vals[index].Enqueue(ball);
                Monitor.PulseAll(q);
                while (true)
                {
                    Monitor.Wait(q);
                }
            }
            catch (Exception e) { }
            finally { Monitor.Exit(q); }
        }
        public Ball[] GetRes()
        {
            Ball[] res = new Ball[3];
            var q = vals;
            Monitor.Enter(q);
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    while (q[i].Count == 0)
                    {
                        Monitor.Wait(q);
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    res[i] = vals[i].Dequeue();
                }
                Monitor.PulseAll(q);
            }
            catch (Exception e) { }
            finally { Monitor.Exit(q); }
            return res;
        }
    }
}
