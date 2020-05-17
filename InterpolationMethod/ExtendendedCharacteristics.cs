using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace InterpolationMethod
{
    public partial class ExtendendedCharacteristics : Form
    {
        public ExtendendedCharacteristics()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
            zedGraphControl1.MouseMove += ZedGraphControl1OnMouseMove;
            zedGraphControl1.MouseClick += ZedGraphControl1OnMouseClick;
            groupBox1.Enabled = false;
            groupBox1.Visible = false;
            groupBox2.Enabled = false;
            groupBox2.Visible = false;
            
        }

        private void ZedGraphControl1OnMouseClick(object sender, MouseEventArgs e)
        {
            double Kp, Kp_Ti, Ti;
            
            zedGraphControl1.GraphPane.ReverseTransform(e.Location,out Kp,out Kp_Ti);
            if (Program.admin == true)
            {
                Ti = Kp / Kp_Ti;
                Kp = Math.Round(Kp, 3);
                Ti = Math.Round(Ti, 3);
                textBox1.Text = Convert.ToString(Kp);
                textBox2.Text = Convert.ToString(Ti);
                groupBox1.Visible = true;
                groupBox1.Enabled = true;
            }
            else
            {
                textBox4.Text = Convert.ToString(Math.Round(Kp, 3));
                textBox3.Text = Convert.ToString(Math.Round(Kp_Ti, 4));
                groupBox2.Visible = true;
                groupBox2.Enabled = true;
            }


        }

        private void ZedGraphControl1OnMouseMove(object sender, MouseEventArgs e)
        {
                        double x, y;
            
                        zedGraphControl1.GraphPane.ReverseTransform(e.Location, out x, out y);
            
                        string text = $"Kp: {Math.Round(x, 4)}; Kp/Ti: {Math.Round(y,4)}";
                        label1.Text = text;
        }
    }
}
