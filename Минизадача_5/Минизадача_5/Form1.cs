﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Минизадача_1
{
    public partial class Form1 : Form
    {
        bool ClickCheck;
        bool InFigureCheck;
        List<MyFigure> a = new List<MyFigure>();

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (MyFigure f in a)
            {
                f.Draw(g);
            }
            if(a.Count >= 3)//Минизадача 4
            {
                for (int i = 0; i < a.Count; i++)
                    a[i].Inside = false;
                for (int i = 0; i < a.Count - 1; i++)
                {
                    for(int j = i + 1; j < a.Count; j++)
                    {
                        bool above = true;
                        bool below =true;
                        for(int k = 0; k < a.Count; k++)
                        {
                            if(k != i && k != i)
                            {
                                if ((a[k].X - a[i].X) * (a[j].Y - a[i].Y) < (a[k].Y - a[i].Y) * (a[j].X - a[i].X)) below = false;//формулу нашел в интернете)
                                if ((a[k].X - a[i].X) * (a[j].Y - a[i].Y) > (a[k].Y - a[i].Y) * (a[j].X - a[i].X)) above = false;//и без понятия как она работает)
                            }
                        }
                        if(above || below)
                        {
                            a[i].Inside = true;
                            a[j].Inside = true;
                            e.Graphics.DrawLine(new Pen(Color.Black), a[i].X, a[i].Y, a[j].X, a[j].Y);
                        }
                    }
                }
                for (int i = 0; i < a.Count; i++)
                {
                    if (!a[i].Inside) 
                    { 
                        a.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            ClickCheck = false;
        }

        private void кругToolStripMenuItem_Click(object sender, EventArgs e)
        {
            квадратToolStripMenuItem.Checked = false;
            треуголькникToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = true;
        }

        private void квадратToolStripMenuItem_Click(object sender, EventArgs e)
        {
            квадратToolStripMenuItem.Checked = true;
            треуголькникToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
        }

        private void треуголькникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            квадратToolStripMenuItem.Checked = false;
            треуголькникToolStripMenuItem.Checked = true;
            кругToolStripMenuItem.Checked = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < a.Count; i++)
                {
                    if (a[i].Check(e.X, e.Y))
                    {
                        a[i].delX = e.X - a[i].X;
                        a[i].delY = e.Y - a[i].Y;
                        a[i].CheckDrag = true;
                        ClickCheck = true;
                    }
                }
                if (a.Count >= 3) 
                {
                    InFigureCheck = true;
                    for (int s = 0; s < a.Count - 1; s++)
                    {
                        for (int j = s + 1; j < a.Count; j++)
                        {
                            bool above = true;
                            bool below = true;
                            for (int k = 0; k < a.Count; k++)
                            {
                                if (k != s && k != s)
                                {
                                    if ((a[k].X - a[s].X) * (a[j].Y - a[s].Y) < (a[k].Y - a[s].Y) * (a[j].X - a[s].X)) below = false;
                                    if ((a[k].X - a[s].X) * (a[j].Y - a[s].Y) > (a[k].Y - a[s].Y) * (a[j].X - a[s].X)) above = false;
                                }
                            }
                            if (above)
                            {
                                if ((e.X - a[s].X) * (a[j].Y - a[s].Y) > (e.Y - a[s].Y) * (a[j].X - a[s].X)) InFigureCheck = false;
                            }
                            if (below)
                            {
                                if ((e.X - a[s].X) * (a[j].Y - a[s].Y) < (e.Y - a[s].Y) * (a[j].X - a[s].X)) InFigureCheck = false;
                            }
                        }
                    }
                    if (InFigureCheck)
                    {
                        for(int i = 0; i < a.Count; i++)
                        {
                            a[i].delX = e.X - a[i].X;
                            a[i].delY = e.Y - a[i].Y;
                            a[i].CheckDrag = true;
                        }
                    }
                }
                if (!ClickCheck && !InFigureCheck)//не нажали ни на один из существующих объектов
                {
                    if (кругToolStripMenuItem.Checked) a.Add(new Circle(e.X, e.Y));
                    if (квадратToolStripMenuItem.Checked) a.Add(new Sqare(e.X, e.Y));
                    if (треуголькникToolStripMenuItem.Checked) a.Add(new Triangle(e.X, e.Y));
                    if(a.Count > 2) ReCheck();
                    Invalidate();
                }
            }
            ClickCheck = false;
            if (MouseButtons.Right == e.Button)
            {
                for (int i = 0; i < a.Count; i++)
                {
                    if (a[i].Check(e.X, e.Y)) 
                    { 
                        a.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            for(int i = 0; i< a.Count; i++) a[i].CheckDrag = false;
            if(a.Count > 2) ReCheck();
            Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].CheckDrag)
                {
                    a[i].X = e.X - a[i].delX;
                    a[i].Y = e.Y - a[i].delY;
                }
            }
            Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;//изображение не мигает
        }

        private void ReCheck()
        {
            for (int i = 0; i < a.Count; i++)
                a[i].Inside = false;
            for (int i = 0; i < a.Count - 1; i++)
            {
                for (int j = i + 1; j < a.Count; j++)
                {
                    bool above = true;
                    bool below = true;
                    for (int k = 0; k < a.Count; k++)
                    {
                        if (k != i && k != i)
                        {
                            if ((a[k].X - a[i].X) * (a[j].Y - a[i].Y) < (a[k].Y - a[i].Y) * (a[j].X - a[i].X)) below = false;
                            if ((a[k].X - a[i].X) * (a[j].Y - a[i].Y) > (a[k].Y - a[i].Y) * (a[j].X - a[i].X)) above = false;
                        }
                    }
                    if (above || below)
                    {
                        a[i].Inside = true;
                        a[j].Inside = true;
                    }
                }
            }
            for (int i = 0; i < a.Count; i++)
            {
                if (!a[i].Inside)
                {
                    a.RemoveAt(i);
                    i--;
                }
            }
        }

        private void выборЦветаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            MyFigure.color = colorDialog1.Color;
            Invalidate();
        }
    }

    abstract class MyFigure
    {
        protected int x;
        protected int y;
        static protected int R;// радиус
        static protected Color C;

        protected int delx;
        protected int dely;
        protected bool checkDrag;

        protected bool inside;

        protected MyFigure() { }
        protected MyFigure(int x, int y)
        {
            this.x = x;
            this.y = y;
            R = 50;//можно менять
            C = Color.Green;//можно менять
            checkDrag = false;
            inside = false;
        }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        public int delX { get { return delx; } set { delx = value; } }//?
        public int delY { get { return dely; } set { dely = value; } }//?
        public bool CheckDrag { get { return checkDrag; } set { checkDrag = value; } }

        public bool Inside { get { return inside; } set { inside = value; } }

        public static Color color{ get { return C; } set { C = value; } }

        public abstract void Draw(Graphics g);
        public abstract bool Check(int x, int y);
    }
        class Circle : MyFigure
        {
            public Circle() : base() { }
            public Circle(int x, int y) : base(x, y) { }
            public override void Draw(Graphics g)
            {
                g.FillEllipse(new SolidBrush(C), x - R, y - R, 2*R, 2*R);
            }
            public override bool Check(int x, int y)
            {
            if (Math.Sqrt(Math.Pow(x - this.x, 2) + Math.Pow(y - this.y, 2)) <= R) return true; 
            else return false;
            }
        }
        class Sqare : MyFigure
        {
            public Sqare() : base() { }
            public Sqare(int x, int y) : base(x, y) { }
            public override void Draw(Graphics g)
            {
                g.FillRectangle(new SolidBrush(C), x - R, y - R, 2 * R, 2 * R);
            }
            public override bool Check(int x, int y)
            {
                if (this.x - R <= x && this.x + R >= x && this.y - R <= y && this.y + R >= y) return true;
                else return false;
            }
        }
        class Triangle : MyFigure
        {
            public Triangle() : base() { }
            public Triangle(int x, int y) : base(x, y) { }
            public override void Draw(Graphics g)
            {
                Point[] p = { new Point(x, y - R), new Point(x - R, y + R), new Point(x + R, y + R) };
                g.FillPolygon(new SolidBrush(C), p);
            }
            public override bool Check(int x, int y)
            {
                if (this.x - R <= x && this.x + R >= x && this.y - R <= y && this.y + R >= y) return true;
                else return false;
            }
        }
}
