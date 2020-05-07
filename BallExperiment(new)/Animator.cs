using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BallExperiment_new_
{
    class Animator
    {
        public Graphics G { get; private set; }
        public Rectangle R { get; private set; }
        private bool stop = false;
        Thread t = null;
        private BufferedGraphics bg;
        private Graphics bgg;
        public List<Ball> balls = new List<Ball>();
        public List<Ring> rings = new List<Ring>();

        public Animator(Graphics g, Rectangle r)
        {
            G = g;
            R = r;
            bg = BufferedGraphicsManager.Current.Allocate(G,
                new Rectangle(0, 0, R.Width, R.Height)
            );
        }

        public void Animate()
        {
            while (!stop)
            {
                bgg = bg.Graphics;
                bgg.Clear(Color.White);
                Monitor.Enter(rings);               
                int cntR = rings.Count;
                for (int i = 0; i < cntR; i++)
                {
                    if (!rings[i].IsAlive)
                    {
                        rings.Remove(rings[i]);
                        i--;
                        cntR--;
                    }
                }
                for (int i = 0; i < cntR; i++)
                {
                    Brush br = new SolidBrush(rings[i].Clr);
                    bgg.FillEllipse(br, rings[i].X, rings[i].Y, 2 * rings[i].Radius, 2 * rings[i].Radius);
                }
                Monitor.Exit(rings);

                Monitor.Enter(balls);
                    int cntB = balls.Count;
                    for (int j = 0; j < cntB; j++)
                    {
                        if (!balls[j].IsAlive)
                        {
                            balls.Remove(balls[j]);
                            j--;
                            cntB--;
                            if (j < 0) j = 0;
                        }
                    }
                    foreach (var b in balls)
                    {
                        Brush br = new SolidBrush(b.BgColor);
                        bgg.FillEllipse(br, b.X, b.Y, b.BallD, b.BallD);
                        Pen p = new Pen(b.FgColor, 2);
                        bgg.DrawEllipse(p, b.X, b.Y, b.BallD, b.BallD);
                    }
                    Monitor.Exit(balls);
                
                try
                {
                    bg.Render();
                }
                catch (Exception e) { }
                Thread.Sleep(30);

            }

        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Animate);
                t = new Thread(th);
                t.Start();
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
