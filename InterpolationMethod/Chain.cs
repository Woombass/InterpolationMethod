using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolationMethod
{
    public class Chain
    {

        public enum ChainType { Integrating = 1, Latency, Proportional};
        /// <summary>
        /// Указывает тип звена.
        /// </summary>
        public ChainType Type { get; set; }
        /// <summary>
        /// Коэффициент усиления звена.
        /// </summary>
        public double? K { get; set; }
        /// <summary>
        /// Постоянная времени звена.
        /// </summary>
        public double? T { get; set; }
        /// <summary>
        /// Запаздывание звена.
        /// </summary>
        public double? tau { get; set; }
        /// <summary>
        /// Значения РАЧХ звена.
        /// </summary>
        public List<double> A { get; set; }
        /// <summary>
        /// Значение РФЧХ звена.
        /// </summary>
        public List<double> F { get; set; }
        public Chain(ChainType type)
        {
            A = new List<double>();
            F = new List<double>();
            Type = type;
            switch(type)
            {
                case ChainType.Integrating:
                    tau = null;
                    T = null;
                    break;
                case ChainType.Latency:
                    K = null;
                    T = null;
                    break;
                case ChainType.Proportional:
                    tau = null;
                    break;
            }
                
        }
    }
}
