using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallExperiment_new_
{
    public partial class Form1 : Form
    {
        private Animator a;
        private Producer[] p;
        private Consumer c;
        private Thread t = null;
        private bool stop = true;
        public Form1()
        {
            InitializeComponent();

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            
            CommonData d = new CommonData();
            a = new Animator(panel1.CreateGraphics(), panel1.ClientRectangle);
            p = new Producer[3];
            for (int i = 0; i < 3; i++)
                p[i] = new Producer(a, d, i);
            c = new Consumer(a,d);
            Start();
        }

        private void Start()
        {
            if (t == null || !t.IsAlive)
            {
                ThreadStart th = new ThreadStart(Move);
                t = new Thread(th);
                t.Start();
            }
        }

        private void Move()
        {
            while (stop)
            {
                c.Start();
                for (int i =0; i<3;i++)
                {
                    p[i].Start();
                }
                Random rand = new Random();
                Thread.Sleep(rand.Next(800,1500));
            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stop = false;
            for (int i = 0; i < 3; i++)
                p[i].Stop();
            a.Abort();
            c.Stop();
            t.Abort();
            t.Join();

        }
    }
}
