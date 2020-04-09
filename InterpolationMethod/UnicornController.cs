using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace InterpolationMethod
{
     public class UnicornController
    {
        public  List<double> Kp { get; set; }
        public  List<double> KpTi { get; set; }
        public List<double> Gamma { get; set; }
        private const double m = 0.221;
        public  List<double> CalculateGamma(List<double> fList)
        {
            if (fList.Count <= 0)
            {
                return null;
            }
            List<double> gamma = new List<double>();
            foreach (var item in fList)
            {
                double number = -(Math.PI / 2) + Math.Atan(m) - item;
                gamma.Add(number);
            }

            Gamma = gamma;
            return gamma;
        }
        public  List<double> CalculateKp( List<double> aList)
        {
            if (Gamma == null || aList == null)
            {
                return null;
            }
            List<double> kp = new List<double>();
            for (int i = 0; i < Gamma.Count - 1; i++)
            {
                double number = (Math.Sqrt(Math.Pow(m, 2) + 1) / aList[i]) * Math.Sin(Gamma[i]);
                kp.Add(number);
            }
            Kp = kp;
            return kp;
        }
        public  List<double> CalculateKp_Ti(List<double> aList)
        {
            if (aList == null || Gamma == null)
            {
                return null;
            }
            
            List<double> Kp_ti = new List<double>();

            for (int i = 0; i < Gamma.Count - 1; i++)
            {
                double number = (Math.Sqrt(Math.Pow(m, 2) + 1) / aList[i]) * (Math.Cos(Gamma[i]) + m * Math.Sin(Gamma[i]));
                Kp_ti.Add(number);
            }
            KpTi = Kp_ti;
            return Kp_ti;
        }
    }
}