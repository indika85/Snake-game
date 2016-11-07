using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace the_game
{
    public partial class Egg : Form
    {
        private string[] names = new string[] {"******<< Higher Diploma 2007.2 @ NIBM >>******\n","Indika Ratnayake","niratnayake@gmail.com"};

        string nn="";
        Point oriP = new Point();
        Size oriSi = new Size();
        public Egg()
        {
            InitializeComponent();
            for(int i=0;i<names.Length;i++)
                nn+="* "+names[i]+" \n----"+"\n";
            p=new Point();
            p = lblNames.Location;
            oriP = p;
            oriSi = lblNames.Size;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        Random rd = new Random();
        private void timer_Images_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image=imageList1.Images[rd.Next(0,imageList1.Images.Count-1)];
        }
        int index = 0;
        Point p;

        private void timer_Words_Tick(object sender, EventArgs e)
        {
            if (index >= nn.Length - 1)
            {
                index = 0;
                //timer_Words.Enabled = false;
                lblNames.Location=oriP;
                p = oriP;
                lblNames.Size = oriSi;
                lblNames.Text = "";
            }
            lblNames.Text += nn[index].ToString();
            index++;
            if ((index > 140&&index<nn.Length-40))
            {
                    lblNames.Height += 1;
                    p.Offset(0, -1);
                    lblNames.Location = p;
            }

        }
    }

}