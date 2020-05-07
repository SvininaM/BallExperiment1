using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BallExperiment_new_
{
    class Consumer
    {
        private Thread t = null;
        private Animator a;
        private CommonData d;
        private bool stop = false;

        public Consumer(Animator a, CommonData d)
        {
            this.a = a;
            this.d = d;
        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(CreateRing);
                t = new Thread(th);
                t.Start();
            }

        }

        private void CreateRing()
        {
            while (!stop)
            {
                int red = 0; int green = 0; int blue = 0;
                Ball[] res = new Ball[3];
                res = d.GetRes();
                for (int i = 0; i < 3; i++)
                {
                    red += res[i].BgColor.R;
                    green += res[i].BgColor.G;
                    blue += res[i].BgColor.B;
                }
                var resClr = Color.FromArgb(red/3, green/3, blue/3);
                Ring r = new Ring(a.R, resClr);
                Monitor.Enter(a.rings);
                a.rings.Add(r);
                r.Start();
                Monitor.Exit(a.rings);
                Monitor.Enter(a.balls);
                foreach (var i in res)
                {
                    i.Abort();
                }
                Monitor.Exit(a.balls);
            }
        }
        public void Stop()
        {
            stop = true;
            Monitor.Enter(a.rings);
            foreach (var r in a.rings)
            {
                r.Abort();
            }
            a.rings.Clear();
            Monitor.Exit(a.rings);
            t.Abort();
            t.Join();
        }

    }
}
