using System;
using System.Collections.Generic;

namespace InterpolationMethod
{
    public class FrequencyCalculator
    {
        private ControllerExtendedCharacteristics Controller;
        public List<double> Frequencies { get; set; }
        private List<Chain> Chains { get; set; }
        private Chain ProportionalChain;
        private Chain LatencyChain;
        public double Frequency1 { get; private set; } = 0;
        public double Frequency0 { get; private set; } = 0;
        private Chain IntegratingChain;
        private double CurrentA;
        private double CurrentF;
        private const double M = 0.221;
        public FrequencyCalculator(List<Chain> chains)
        {
            Chains = chains ?? throw new ArgumentNullException();
            for (int i = 0; i <= Chains.Count - 1; i++)
            {
                switch (Chains[i].Type)
                {
                    case Chain.ChainType.Integrating:
                        IntegratingChain = Chains[i];
                        break;
                    case Chain.ChainType.Latency:
                        LatencyChain = Chains[i];
                        break;
                    case Chain.ChainType.Proportional:
                        ProportionalChain = Chains[i];
                        break;
                }
            }
        }
        public void Calculate()
        {
            if (ProportionalChain == null && LatencyChain == null && IntegratingChain != null)
            {
                CalculateI();
            }
            else if (LatencyChain == null && IntegratingChain == null && ProportionalChain != null)
            {
                CalculateP();
            }
            else if (IntegratingChain == null && ProportionalChain == null && LatencyChain != null)
            {
                CalculateL();
            }
            else if (ProportionalChain != null && LatencyChain != null && IntegratingChain == null)
            {
                CalculatePL();
            }
            else if (LatencyChain != null && IntegratingChain != null && ProportionalChain == null)
            {
                CalculateLI();
            }
            else if (ProportionalChain != null && IntegratingChain != null && LatencyChain == null)
            {
                CalculatePI();
            }
        }
        private void CalculateIntegrating(Chain chain,double first,double last,double step)
        {
            for (double i = first; i <= last;)
            {
                double A = (double)chain.K / (i * Math.Sqrt(Math.Pow(M, 2) + 1));
                double F = -(Math.PI / 2) - Math.Atan(M);
                chain.A.Add(A);
                chain.F.Add(F);
                i += step;
            }
        }
        private void CalculateLatency(Chain chain, double first, double last, double step)
        {
            for (double i = first; i < last;)
            {
                double A = Math.Pow(Math.E, M * i * (double)chain.tau);
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

                double A = (double) chain.K /
                           (Math.Sqrt(Math.Pow(1 - (double) chain.T * M * i, 2) + Math.Pow((double) chain.T * i, 2)));
                double F = -(Math.Atan(((double) chain.T * i) / (1 - (double) chain.T * i * M)));

                chain.A.Add(A);
                chain.F.Add(F);
                i += step;
            }
        }
        private void CalculatePL()
        {
            double F;
            double compareExpression = Math.Round(-Math.PI / 2 + Math.Atan(M), 2);
            do
            {
                Frequency1 += 0.0001;
                F = -(Frequency1 * (double) LatencyChain.tau) +
                    -Math.Atan((double) ProportionalChain.T * Frequency1 / (1 - (double) ProportionalChain.T * Frequency1 * M));

                F = Math.Round(F, 2);
                CurrentF = F;
            } while (Math.Abs(CurrentF - Math.Round(Math.PI,2)) > 0.1);

            do
            {
                Frequency0 += 0.0001;
                F = -(Frequency0 * (double) LatencyChain.tau) +
                    -Math.Atan((double) ProportionalChain.T * Frequency0 / (1 - (double) ProportionalChain.T * Frequency0 * M));
                F = Math.Round(F, 2);
                CurrentF = F;

            } while (Math.Abs(CurrentF - compareExpression) > 0.1);
        }
        private void CalculatePI()
        {
            
        }
        private void CalculateLI()
        {
            
        }
        private void CalculateL()
        {
            
        }
        private void CalculateP()
        {
            
        }
        private void CalculateI()
        {
            
        }
    }
}