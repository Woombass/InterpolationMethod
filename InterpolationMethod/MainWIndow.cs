using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace InterpolationMethod
{
    public partial class MainWIndow : Form
    {

        public List<double> GetNumbers(string nums)
        {
            nums = nums.Replace('.', ',');
            var InputNum = nums.TrimEnd(' ').Split(new[] { ' ' });

            List<double> ListOfNum = new List<double>();


            for (int i = 0; i < InputNum.Length; i++)
            {
                var temp = Math.Round(double.Parse(InputNum[i]), 3);
                ListOfNum.Add(temp);
            }


            return ListOfNum;


        }
        private void DrawY(List<double> listOfY, List<double> step)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            zedGraphControl1.Text = "График";

            zedGraphControl1.MouseMove += ZedGraphControl1_MouseMove;

            pane.XAxis.Title.Text = "t";
            pane.XAxis.Title.FontSpec.IsUnderline = true;
            pane.XAxis.Title.FontSpec.FontColor = Color.OrangeRed;
            pane.YAxis.Title.Text = "Y(t)";
            pane.YAxis.Title.FontSpec.IsUnderline = true;
            pane.YAxis.Title.FontSpec.FontColor = Color.Aqua;
            pane.CurveList.Clear();
            PointPairList list = new PointPairList();
            for (int i = 0; i < listOfY.Count; i++)
            {
                list.Add(step[i], listOfY[i]);
            }


            LineItem myCurve = pane.AddCurve("Y", list, Color.Red, SymbolType.Circle);
            myCurve.Symbol.Size = 6;
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 1F;
            myCurve.Line.Width = 1;

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();



        }

        private void ZedGraphControl1_MouseMove(object sender, MouseEventArgs e)
        {
            double x, y;

            zedGraphControl1.GraphPane.ReverseTransform(e.Location, out x, out y);

            string text = $"X: {Math.Round(x, 4)}; Y: {Math.Round(y,4)}";
            label9.Text = text;
        }

        delegate void DoWork (double a, double b, double c, double d, double x);
        DoWork dd;

        public MainWIndow()
        {
            InitializeComponent();
            button2.Visible = false;
            groupBox2.Visible = false;
            groupBox1.Visible = false;
            label3.Text = "Ya";
            label4.Text = "Yb";
            label5.Text = "Ta";
            label6.Text = "Tb";
            label9.Text = "";
            this.Height = 455;

            ChainEditor editor = new ChainEditor();
            editor.Show();

        }

        ControllerGraphic controller;
        private void button1_Click(object sender, EventArgs e)
        {
            this.Height = 700;
            groupBox2.Visible = true;
            groupBox1.Visible = true;
            var points = GetNumbers(textBox1.Text);
            var step = GetNumbers(textBox2.Text);

            controller = new ControllerGraphic(points, step);
            dd += controller.Calculate;
            controller.Normalize();
            points = controller.Graph.NormPoints;
            DrawY(points, step);
            button2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Ya = double.Parse(textBox3.Text);
            var Yb = double.Parse( textBox4.Text);
            var Ta = double.Parse(textBox5.Text);
            var Tb = double.Parse(textBox6.Text);
            var k = double.Parse(textBox7.Text);
            dd(Ta, Tb, Ya, Yb, k);
            label7.Text =  "T: " + Convert.ToString( Math.Round(controller.Graph.T, 4) );
            label8.Text = "Tau: " + Convert.ToString(Math.Round(controller.Graph.Tau, 4));
            label10.Text = "K: " + Convert.ToString(Math.Round(controller.Graph.K, 4));
        }
    }
}
