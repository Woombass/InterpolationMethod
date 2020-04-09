using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolationMethod
{
    public class ControllerGraphic
    {
        public Graphic Graph;
        public ControllerGraphic()
        {

        }
        public ControllerGraphic(List<double> points, List<double> step)
        {
            Graph = new Graphic(points, step);
        }
        public void Normalize()
        {
            var normPoints = new List<double>();

            
            for (int i = 0; i < Graph.Points.Count; i++)
            {
                double temp;
                temp = (Graph.Points[i] - Graph.Ymin) / (Graph.Ymax - Graph.Ymin);
                normPoints.Add(temp);
            }

            if (normPoints != null)
            {
                Graph.NormPoints = normPoints;
            }
        }
        public void Calculate(double Ta, double Tb, double Ya, double Yb, double delta)
        {

            var Tau = (Tb * Math.Log(1 - Ya) - Ta * Math.Log(1 - Yb)) / (Math.Log(1 - Ya) - Math.Log(1 - Yb));

            var T = (-((Ta - Tau) / Math.Log(1 - Ya))) + (-((Tb - Tau) / (Math.Log(1 - Yb))));
            Graph.Delta = delta;
            var k = (Graph.Points.Max() - Graph.Points.Min()) / Graph.Delta;
            Graph.Tau = Tau;
            Graph.T = T;
            Graph.K = k;

        }
    }
}
