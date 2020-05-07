using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BallExperiment_new_
{
    class Ring
    {
        public Color Clr { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        private int maxRadius = 700;
        /*{
            get
            {
                return (Animator.R.Width > Animator.R.Height)
                ? Animator.R.Width : Animator.R.Height;
            }
        }*/
        public bool IsAlive { get { return t != null && t.IsAlive; } }
        public int Radius { get; private set; }
        private bool stop;
        private Thread t = null;

        public Ring(Rectangle r, Color clr)
        {
            Clr = clr;
            Radius = 0;
            X = r.Width / 2;
            Y = r.Height / 2;
        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Enc);
                t = new Thread(th);
                t.Start();
            }
        }

        private void Enc()
        {
            while (!stop && maxRadius > Radius)
            {
                X -= 1;
                Y -= 1;
                Radius += 1;
                Thread.Sleep(5);
                int alpha = Math.Abs((int)((1.0 - (float)Radius / maxRadius) * 255)) % 255;
                Clr = Color.FromArgb(
                    (int)(alpha),
                    Clr.R,
                    Clr.G,
                    Clr.B
                );
            }
        }

        public void Abort()
        {
            try
            {
                stop = true;
                t.Abort();
                t.Join();
            }
            catch (Exception e) { }
        }
    }
}
