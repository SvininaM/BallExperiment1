using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BallExperiment_new_
{
    class Producer
    {
        private Animator a;
        private int side;
        private CommonData d;

        public bool flag = true;
        public Producer(Animator a, CommonData d, int side)
        {
            this.a = a;
            this.d = d;
            this.side = side;
        }

        public void Start()
        {
            Ball b = new Ball(a.R, side);
            b.Start(d);
            Monitor.Enter(a.balls);
            a.balls.Add(b);
            Monitor.Exit(a.balls);
            a.Start();
        }

        public void Stop()
        {
            Monitor.Enter(a.balls);
            foreach (var b in a.balls)
            {
                b.Abort();
            }
            a.balls.Clear();
            Monitor.Exit(a.balls);
        }
    }
}
