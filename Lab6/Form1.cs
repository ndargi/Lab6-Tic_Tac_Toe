using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        //dimensions
        private const float clientSize = 100;
        private const float lineLength = 80;
        private const float block = lineLength / 3;
        private const float offset = 10;
        private const float delta = 5;
        private bool gameover = false;
        public enum CellSelection { N, O, X };
        private CellSelection[,] grid;
        private float scale; //current scale factor
        private bool firstmove = true;//True if the user will go first otherwise false;
        private GameEngine Engine = new GameEngine();
        public Form1()
        {
            InitializeComponent();
            ResizeRedraw = true;
            grid = Engine.enginegrid;//Initialize the grid using the game engine

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            grid = Engine.enginegrid;//get the status of the board from the game engine
            base.OnPaint(e);
            Graphics g = e.Graphics;
            ApplyTransform(g);
            //draw board
            g.DrawLine(Pens.Black, block, 0, block, lineLength);
            g.DrawLine(Pens.Black, 2 * block, 0, 2 * block, lineLength);
            g.DrawLine(Pens.Black, 0, block, lineLength, block);
            g.DrawLine(Pens.Black, 0, 2 * block, lineLength, 2 * block);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (grid[i, j] == CellSelection.O)
                    {
                        DrawO(i, j, g);
                    }
                    else if (grid[i, j] == CellSelection.X)
                    {
                        DrawX(i, j, g);
                    }
                }
            }
        }
        private void ApplyTransform(Graphics g)
        {
            scale = Math.Min(ClientRectangle.Width / clientSize, ClientRectangle.Height / clientSize);
            if (scale == 0f) return;
            g.ScaleTransform(scale, scale);
            g.TranslateTransform(offset, offset);
        }
        private void DrawX(int i, int j, Graphics g)
        {
            g.DrawLine(Pens.Black, i * block + delta, j * block + delta, (i * block) + block - delta, (j * block) + block - delta);
            g.DrawLine(Pens.Black, (i * block) + block - delta, j * block + delta, (i * block) + delta, (j * block) + block - delta);
        }
        private void DrawO(int i, int j, Graphics g)
        {
            g.DrawEllipse(Pens.Black, i * block + delta, j * block + delta, block - 2 * delta, block - 2 * delta);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!gameover)//Nothing will happen if the game is over
            {
                if (firstmove)
                {
                    computerStartsToolStripMenuItem.Enabled = false;

                }
                Graphics g = CreateGraphics();
                ApplyTransform(g);
                PointF[] p = { new Point(e.X, e.Y) };
                g.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, p);
                if (p[0].X < 0 || p[0].Y < 0) return;
                int i = (int)(p[0].X / block);
                int j = (int)(p[0].Y / block);
                if (i > 2 || j > 2) { return; }
                if (e.Button == MouseButtons.Left)//Only enter if it is a left click
                {
                    if (Engine.LegalMove(i, j))//Will check if move is legal, and if it is it will add the move to the board
                    {

                        AIMove();//Legal move has happened, let the AI move
                    }
                    else
                    {
                        MessageBox.Show("Illegal Move");
                    }
                }
                Invalidate();
            }            
        }

        private void computerStartsToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            computerStartsToolStripMenuItem.Enabled = false;
            firstmove = false;
            Engine.nextentry = CellSelection.O;//Change the next move to O since computer is starting;
            AIMove();
            Invalidate();
        }
        private void AIMove()//Will be called when it is time for the AI to perform a move
        {
            Invalidate();
            string status = Engine.ComputerMove();//Calls the method for the AI to make its move, logic and board manipulation done inside GameEngine
            if (status == "w")//Below is how the form knows what the game engine means/if no moves should be allowed
            {
                MessageBox.Show("Congratulations, You Win!");
                gameover = true;
            }
            else if (status == "l")
            {
                MessageBox.Show("You Lose!");
                gameover = true;
            }
            else if (status== "t")
            {
                MessageBox.Show("You Tied!");
                gameover = true;
            }
            Invalidate();
        }
        private void newToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            Engine = new GameEngine();
            firstmove = true;
            gameover = false;
            computerStartsToolStripMenuItem.Enabled = true;
            Invalidate();
        }
    }
}
