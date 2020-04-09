using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace InterpolationMethod
{
    public class ControllerExtendedCharacteristics
    {
        public const double m = 0.221;
        public ZedGraphControl zedGraph;
        public List<Chain> Chains = new List<Chain>();
        public void CalculateExtendedCharacteristics(double first,double last,double step)
        {
            foreach (var chain in Chains)
            {
                switch (chain.Type)
                {
                    case Chain.ChainType.Integrating:
                        CalculateIntegrating(chain,first,last,step);
                        break;
                    case Chain.ChainType.Latency:
                        CalculateLatency(chain,first,last,step);
                        break;
                    case Chain.ChainType.Proportional:
                        CalculateProportional(chain,first,last,step);
                        break;
                }
            }

        }
        private void CalculateIntegrating(Chain chain,double first,double last,double step)
        {
            for (double i = first; i <= last;)
            {
                double A = (double)chain.K / (i * Math.Sqrt(Math.Pow(m, 2) + 1));
                double F = -(Math.PI / 2) - Math.Atan(m);

                chain.A.Add(A);
                chain.F.Add(F);

                i += step;
            }
        }
        private void CalculateLatency(Chain chain, double first, double last, double step)
        {
            for (double i = first; i < last;)
            {
                double A = Math.Pow(Math.E, m * i * (double)chain.tau);
                double F = -(i * (double)chain.tau);

                chain.A.Add(A);
                chain.F.Add(F);

                i += step;
            }
        }
        private void CalculateProportional(Chain chain, double first, double last, double step)
        {

            for (double i = first; i < last;)
            {

                double A = (double)chain.K / (Math.Sqrt(Math.Pow(1 - (double)chain.T * m * i, 2) + Math.Pow((double)chain.T * i, 2)));
                double F = -(Math.Atan(((double)chain.T * i) / (1 - (double)chain.T * i * m)));

                chain.A.Add(A);
                chain.F.Add(F);
                i += step;
            }
        }
        public void DrawExtCh(List<double> listOfY, List<double> step)
        {
            
            GraphPane pane = zedGraph.GraphPane;
            zedGraph.Text = "ЛРЗ";
            pane.Title.Text = "Построение Линии Равного Затухания ";
            pane.Title.FontSpec.FontColor = Color.Fuchsia;
            pane.XAxis.Title.Text = "Kp";
            pane.XAxis.Title.FontSpec.IsUnderline = true;
            pane.XAxis.Title.FontSpec.FontColor = Color.OrangeRed;
            pane.YAxis.Title.Text = "Kp/Tи";
            pane.YAxis.Title.FontSpec.IsUnderline = true;
            pane.YAxis.Title.FontSpec.FontColor = Color.Aqua;
            pane.CurveList.Clear();
            PointPairList list = new PointPairList();
            for (int i = 0; i < listOfY.Count ; i++)
            {
                list.Add(step[i], listOfY[i]);
            }

            LineItem myCurve = pane.AddCurve("ЛРЗ", list, Color.Plum, SymbolType.Circle);
            myCurve.Symbol.Size = 6;
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 1F;
            myCurve.Line.Width = 1;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }
        

    }

}
