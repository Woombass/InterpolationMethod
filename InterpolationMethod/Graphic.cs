using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterpolationMethod
{
    public class Graphic
    {
        public List<double> Points { get; set; } = new List<double>();
        public double K { get; set; }
        public double Delta { get; set; }
        public double Ymax
        {
            get => (Points.Max());
            set {  }
        }

        public double Ymin
        {
            get => (Points.Min());
            set {}
        }

        public double Tau { get; set; }

        public double T { get; set; }

        public List<double> NormPoints { get; set; } = new List<double>();

        public List<double> Step { get; set; } = new List<double>();
        public Graphic(List<double> points , List<double> step)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points), "Набор точек не может быть NULL");
            }

            if (step == null)
            {
                throw new ArgumentNullException(nameof(step), "Лист шагов не может быть NULL");
            }

            Step = step;
            Points = points;
        }

    }
}