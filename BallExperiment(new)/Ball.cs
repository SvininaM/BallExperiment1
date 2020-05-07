using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BallExperiment_new_
{
    class Ball
    {
        private int side;
        private int width;
        private int height;
        public int X { get; private set; }
        public int Y { get; private set; }
        public Color FgColor { get; private set; }
        public Color BgColor { get; private set; }
        public int BallD { get; private set; }
        private int dx, dy;
        private Thread t;
        public CommonData d;
        private bool stop = false;

        public bool IsAlive
        {
            get { return t != null && t.IsAlive; }
        }
        public Ball(Rectangle r, int side)
        {

            this.side = side;
            width = r.Width;
            height = r.Height;
            Random rand = new Random();
            BallD = rand.Next(15, 30);
            if (side == 0)// красный
            {
                FgColor = Color.Red;
                BgColor = Color.FromArgb(rand.Next(25, 255), 255,
                    rand.Next(75),
                    rand.Next(75)
                );
                X = 0;
                Y = height / 2 + rand.Next(-15, 15);
                dx = rand.Next(5, 7);
                dy = 0;
            }
            if (side == 1)//зеленый
            {
                FgColor = Color.Green;
                BgColor = Color.FromArgb(rand.Next(25, 255),
                    rand.Next(75),
                    255,
                    rand.Next(75)
                );
                X = width / 2 + rand.Next(-15, 15);
                Y = 0;
                dx = 0;
                dy = rand.Next(3, 6);
            }
            if (side == 2)//синий
            {
                FgColor = Color.Blue;
                BgColor = Color.FromArgb(rand.Next(25, 255),
                    rand.Next(75),
                    rand.Next(75),
                    255
                );
                X = width - BallD;
                Y = height / 2 + rand.Next(-15, 15);
                dx = rand.Next(-7, -5);
                dy = 0;
            }

        }
        public void Start(CommonData d)
        {
            this.d = d;
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Move);
                t = new Thread(th);
                t.Start();
            }
        }

        private void Move()
        {
            while (!stop)
            {
                var spd = (side + 1) * 30;
                Thread.Sleep(spd);
                X += dx;
                Y += dy;
                if (new Rectangle(width / 2 - 30, height / 2 - 30, 60, 60).Contains(X, Y))
                {
                    d.Add(this, side);
                    Stop();
                }
            }
        }

        private void Stop()
        {
            stop = true;
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
