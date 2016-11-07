using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace the_game
{
    public partial class Form1 : Form
    {
        private Image imgBack;
        private Image imgApple;
        private Image imgSnake;

        string soundFile = "L:\\xxx.wav";

        private Color appleColor = Color.Red;
        private Color snakeColor = Color.Lime;
        private int noOfParts = 0;
        private const int maxNoOfParts = 120;
        private const int posX = 250;
        private const int posY = 200;
        private const int dimension = 5;
        private const int dimensionApple = 6;
        private const int boundUp = 23;
        private const int boundLeft = 20;
        private const int boundRight = 313;
        private const int boundBottom = 272;
        private bool friend = false;
        private string check = "";
        private const string CODE = "friends";
        private string[] round = new string[] { "|    ", "¯   ", " ¯  ", "  ¯ ", "   ¯", "    |", "   _", "  _ ", " _  ", "_   " };

        private int[] appleGridX = new int[(boundRight/dimension)+1];
        private int[] appleGridY = new int[(boundBottom/dimension)+1];

        private Rectangle recApp = new Rectangle(0, 0, dimensionApple+2, dimensionApple);
        
        private Rectangle[] recSna = new Rectangle[maxNoOfParts];

        private Random rd = new Random();
        private bool goUp = false;
        private bool goDown = false;
        private bool goLeft = false;
        private bool goRight = false;
        private Point newLocation;
        private Point appleLocation = new Point();
        private Point previousLocation = new Point(0, 0);
        private Label[] snakeParts = new Label[maxNoOfParts];
        private Label apple = new Label();
        private Rectangle imgRec = new Rectangle(boundLeft, boundUp, boundRight, boundBottom);      
        
        public Form1()
        {
            InitializeComponent();
            imgApple = imageList1.Images[0];
            imgBack = panel1.BackgroundImage;
            imgSnake = imageList1.Images[1];

            setAppleProperties(imgApple, Color.Transparent);

            panel1.Controls.Add(apple);
            int count = 0;
            for (int i = boundLeft+10; i <= boundRight; i++)
            {
                appleGridX[count] = i;
                count++;
                i += dimension - 1;
            }
            count = 0;
            for (int i = boundUp+10; i <= boundBottom; i++)
            {
                appleGridY[count] = i;
                count++;
                i += dimension - 1;
            }
            newLocation = new Point(posX,posY);
            snakeParts[noOfParts] = new Label();
            snakeParts[noOfParts].Location = newLocation;
            snakeParts[noOfParts].Size = new Size(dimension, dimension);
            
            recSna[noOfParts].Location = newLocation;
            recSna[noOfParts].Size = new Size(dimension, dimension);

            setSnakeProperties(snakeColor, imgSnake);

            this.panel1.Controls.Add(snakeParts[noOfParts]);
            DoubleBuffered = true;

            drawApple();
        }
        private void setAppleProperties(Image appleImage,Color backColor)
        {
            apple.BackColor = backColor;
            apple.Size = new Size(dimensionApple, dimensionApple);
            apple.Image = appleImage;
        }
        private void setSnakeProperties(Color snColor,Image snaImage)
        {
            snakeParts[0].BackColor = snakeColor;
            snakeParts[0].Image = snaImage;
        }

       private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.KeyCode == Keys.Up)
            {
                if (goDown) return;
                goUp = true;
                goDown = false;
                goRight = false;
                goLeft = false;
            }
                else if (e.KeyCode == Keys.Down)
            {
                if (goUp) return;
                goUp = false;
                goDown = true;
                goRight = false;
                goLeft = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (goLeft) return;
                goUp = false;
                goDown = false;
                goRight = true;
                goLeft = false;
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (goRight) return;
                goUp = false;
                goDown = false;
                goRight = false;
                goLeft = true;
            }
            if (friend)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    checkCode(check);
                    check = "";
                }
                else
                check += e.KeyCode.ToString().ToLower();
            }

        }
        private void checkCode(string c)
        {
            if (c == CODE)
            {
                timer_Friend.Enabled = false;
                Egg g = new Egg();
                g.ShowDialog();
            }
        }

        private void timer_MoveSnake_Tick(object sender, EventArgs e)
        {
            if (goUp)
            {
                newLocation.Y -= dimension;
            }
            else if (goDown)
            {
                newLocation.Y += dimension;
            }
            else if (goRight)
            {
                newLocation.X += dimension;
            }
            else if (goLeft)
            {
                newLocation.X -= dimension;
            }

            if (goUp || goDown || goRight || goLeft)
            {
                drawSnake();
                if (!timer1.Enabled)
                {
                    timer1.Enabled = true;
                    lblInfo.Visible = false;
                    lblPause.Enabled = true;
                }
            }
            /*************************/
            if (newLocation.X < boundLeft)
            {
                newLocation.X = boundRight;
            }
            if (newLocation.X > boundRight)
            {
                newLocation.X = boundLeft;
            }
            if (newLocation.Y < boundUp)
            {
                newLocation.Y = boundBottom;
            }
            if (newLocation.Y > boundBottom)
            {
                newLocation.Y = boundUp;
            }
           
        }
        private void addSnakeParts()
        {
            noOfParts++;
            if (noOfParts < maxNoOfParts - 1)
            {
                snakeParts[noOfParts] = new Label();
                snakeParts[noOfParts].Location = newLocation;
                recSna[noOfParts].Location = newLocation;
                recSna[noOfParts].Size = new Size(dimension, dimension);
                snakeParts[noOfParts].Size = new Size(dimension, dimension);
                snakeParts[noOfParts].BackColor = snakeParts[0].BackColor;
                snakeParts[noOfParts].Image = snakeParts[0].Image;
                this.panel1.Controls.Add(snakeParts[noOfParts]);
                timer_MoveSnake.Start();
            }
        }

        private void drawSnake()
        {
           previousLocation = snakeParts[0].Location;


            if (noOfParts > 0)
            {
                for (int count = noOfParts; count > 0; count--)
                {
                    previousLocation = snakeParts[count-1].Location;
                    snakeParts[count].Location = previousLocation;
                    recSna[count].Location = previousLocation;
                }
            }

            snakeParts[0].Location = newLocation;
            for (int i = 1; i < noOfParts-1; i++)
            {
                if (recSna[i].Contains(snakeParts[0].Location))
                {
                    timer_MoveSnake.Stop();
                    timer1.Stop();
                    if (MessageBox.Show("You Lost !\nDo you want to Play again ?", "Snake in the Form", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        Application.Restart();
                    else
                        Close();
                }
            }
            Point temp = snakeParts[0].Location;
            temp.Offset(4, 4);
            if (recApp.Contains(temp))
                eatApple();

        }

        private void drawApple()
        {
            Random rd = new Random();
            do
            {
                appleLocation.X = appleGridX[rd.Next(0, appleGridX.Length - 1)];
                if (appleLocation.X > 0)
                    break;
            }
            while (true);
            do
            {
                appleLocation.Y = appleGridY[rd.Next(0, appleGridY.Length - 1)];
                if (appleLocation.Y > 0)
                    break;
            }
            while (true);
            apple.Location = appleLocation;
            recApp.Location = appleLocation;
        }
        private void eatApple()
        {
            //apple.Dispose();
            timer_MoveSnake.Stop();
            addSnakeParts();
            drawApple();
            setMarks();
            lblNoOfApples.Text = noOfParts.ToString();
        }
        private int marks = 0;
        int sec = 0, min = 0;
        string timeMin = "",timeSec="";
        private void timer1_Tick(object sender, EventArgs e)
        {
            sec++;
            if (sec >= 60)
            {
                sec = 0;
                min++;
            }
            if (sec >= 10)
                timeSec = sec.ToString();
            else
                timeSec ="0" + sec.ToString();

            if (min >= 10)
                timeMin = min.ToString();
            else
                timeMin = "0" + min.ToString();
            lblTime.Text = timeMin + ":" + timeSec;
        }
        private void setMarks()
        {
            marks += 50;
            lblMarks.Text = marks.ToString();
            if((marks%100)==0)
                setSpeed(marks);
            
        }
        private void setSpeed(int m)
        {
            if (timer_MoveSnake.Interval < 3)
            {
                return;
            }
            else if(timer_MoveSnake.Interval<11)
                timer_MoveSnake.Interval -= 1;
            else
            timer_MoveSnake.Interval -= 10;
            lblSpeed.Text =Convert.ToString(200 - timer_MoveSnake.Interval);
        }

        private void lblClose_MouseEnter(object sender, EventArgs e)
        {
            lblClose.ForeColor = Color.Red;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            lblClose.ForeColor = Color.Black;
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            if (!timer_Pause.Enabled)
            {
                timer1.Stop();
                timer_MoveSnake.Stop();
                timer_Pause.Enabled = true;
            }
            else
            {
                timer_Pause.Enabled = false;
                timer_MoveSnake.Start();
                timer1.Start();
            }
        }

        private void timer_Pause_Tick(object sender, EventArgs e)
        {
            if (lblPause.ForeColor == Color.Black)
                lblPause.ForeColor = Color.Orange;
            else
                lblPause.ForeColor =Color.Black;
        }

        private void lblPause_MouseEnter(object sender, EventArgs e)
        {
            lblPause.ForeColor = Color.Orange;
        }

        private void lblPause_MouseLeave(object sender, EventArgs e)
        {
            lblPause.ForeColor = Color.Black;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageBox.Show("Are you sure you want to exit ?", "Snake in the form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //    e.Cancel = true;
        }
        int xx = 0;
        private void timer_Friend_Tick(object sender, EventArgs e)
        {
            if (lblPause.BackColor == Color.White)
            {
                lblPause.BackColor = Color.Green;
                lblClose.BackColor = Color.Green;
            }
            else
            {
                lblPause.BackColor = Color.White;
                lblClose.BackColor = Color.White;
            }
            if (xx >= round.Length)
                xx = 0;
            lblMarks.Text = round[xx] + " " + round[xx];
            lblNoOfApples.Text = round[xx] + " " + round[xx];
            lblSpeed.Text = round[xx] + " " + round[xx];
            lblTime.Text = round[xx] + " " + round[xx];
            xx++;
        }
        string t1 = "";
        string t2 = "";
        string t3 = "";
        string t4 = "";
        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            if (!timer_Friend.Enabled)
            {
                t1 = lblTime.Text;
                t2 = lblSpeed.Text;
                t3 = lblNoOfApples.Text;
                t4 = lblMarks.Text;
                timer_Friend.Enabled = true;
                timer1.Enabled = false;
                timer_MoveSnake.Enabled = false;
                friend = true;
            }
            else
            {
                timer_Friend.Enabled = false;
                timer1.Enabled = true;
                timer_MoveSnake.Enabled = true;
                friend = false;
                lblTime.Text=t1;
                lblSpeed.Text=t2;
                lblNoOfApples.Text=t3;
                lblMarks.Text=t4;
                lblClose.BackColor = Color.White;
                lblPause.BackColor = Color.White;
                lblInfo.Visible = false;
                lblPause.Enabled = true;

            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            withoutImages();
            setSnakeProperties(snakeColor, null);
            setAppleProperties(null, appleColor);
            for(int x=0;x<=noOfParts;x++)
            {
                snakeParts[x].BackColor = snakeColor;
                snakeParts[x].Image = null;
            }
        }
        private void withoutImages()
        {
            panel1.BackgroundImage = null;
            panel1.BackColor = Color.Black;
        }

        private void withImages()
        {
            panel1.BackgroundImage = imgBack;
            panel1.BackColor = SystemColors.ActiveBorder;
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Red;
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
        }

        private void label8_MouseEnter(object sender, EventArgs e)
        {
            label8.ForeColor = Color.Red;
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            label8.ForeColor = Color.Black;
        }

        private void label7_Click_1(object sender, EventArgs e)
        {
            withImages();
            setAppleProperties(imgApple, Color.Transparent);
            setSnakeProperties(Color.Transparent, imgSnake);
            for (int x = 0; x <= noOfParts; x++)
            {
                snakeParts[x].BackColor = Color.Transparent;
                snakeParts[x].Image = imgSnake;
            }
        }

    }

}