using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallClass
{
    public partial class FrmBallClass : Form
    {

        // global variables
        Timer draw;
        BouncingBall ballMaster;
        BouncingBall[] balls = new BouncingBall[50];

        public FrmBallClass()
        {
            InitializeComponent();
        }

        private void FrmBallClass_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.Paint += FrmBallClass_Paint;
            Random rnd = new Random();

            ballMaster = new BouncingBall(this,
                                          ClientRectangle.Width / 2,
                                          ClientRectangle.Height / 2,
                                          5,
                                          5,
                                          200,
                                          Brushes.Black);

            for (int i = 0; i < balls.Length; i++)
            {
                balls[i] = new BouncingBall(this,
                                          ClientRectangle.Width / 2,
                                          ClientRectangle.Height / 2,
                                          rnd.Next(1, 10),
                                          rnd.Next(1, 10),
                                          rnd.Next(50, 200),
                                          new SolidBrush(
                                              Color.FromArgb(rnd.Next(0, 256),
                                              rnd.Next(0, 256),
                                              rnd.Next(0, 256))));
            }



            draw = new Timer();

            draw.Interval = 10;
            draw.Tick += Draw_Tick;
            draw.Enabled = true;
        }

        private void Draw_Tick(object sender, EventArgs e)
        {
            ballMaster.Update();
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].Update();
            }
            this.Refresh();
        }

        private void FrmBallClass_Paint(object sender, PaintEventArgs e)
        {
            ballMaster.Display(e.Graphics);
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].Display(e.Graphics);
            }

        }
    }
}


public class BouncingBall
{
    private int x, y, dx, dy, size;
    private Brush ballColor;
    private Form frm;

    public int X
    {
        get { return x; }
    }
    public int Y
    {
        get { return y; }
    }
    public int Size
    {
        get { return size; }
    }

    public BouncingBall(Form frm, int x, int y, int dx, int dy, int size, Brush ballColor)
    {
        this.frm = frm;
        this.x = x;
        this.y = y;
        this.dx = dx;
        this.dy = dy;
        this.size = size;
        this.ballColor = ballColor;
    }

    // displays movement
    public void Display(Graphics g)
    {
        g.FillEllipse(ballColor, x, y, size, size);
    }

    // update the status of the ball
    public void Update()
    {
        Move();
        // edge detection
        checkCollisionWithWalls();
    }

    private void checkCollisionWithWalls()
    {
        if (isCollingWithHorizontalWalls()) dy *= -1;
        if (isCollingWithVerticalWalls()) dx *= -1;
    }

    private bool isCollingWithVerticalWalls()
    {
        if (x < 0 || x + size > frm.ClientRectangle.Width) return true;
        return false;
    }

    private bool isCollingWithHorizontalWalls()
    {
        if (y < 0 || y + size > frm.ClientRectangle.Height) return true;
        return false;
    }

    // makes ball move in memory
    public void Move()
    {
        x += dx;
        y += dy;
    }

}