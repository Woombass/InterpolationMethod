using System;
using System.Collections.Generic;

namespace InterpolationMethod
{
    public class FrequencyCalculator
    {
        private ControllerExtendedCharacteristics Controller;

        //public List<double> Frequencies { get; set; }
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

        private void CalculateIntegrating(Chain chain, double first, double last, double step)
        {
            for (double i = first; i <= last;)
            {
                double A = (double) chain.K / (i * Math.Sqrt(Math.Pow(M, 2) + 1));
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
                double A = Math.Pow(Math.E, M * i * (double) chain.tau);
                double F = -(i * (double) chain.tau);
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
            double compareExpression;
            double compareExpression2;
            do
            {
                Frequency1 += 0.0005;
                F = -(Frequency1 * (double) LatencyChain.tau) +
                    -Math.Atan((double) ProportionalChain.T * Frequency1 /
                               (1 - (double) ProportionalChain.T * Frequency1 * M));

                F = Math.Round(F, 2);
                CurrentF = F;
                compareExpression2 = Math.Abs(Math.Abs(CurrentF) - Math.Abs(Math.Round(Math.PI, 2)));
            } while (compareExpression2 > 0.05);

            do
            {
                Frequency0 += 0.0005;
                F = -(Frequency0 * (double) LatencyChain.tau) +
                    -Math.Atan((double) ProportionalChain.T * Frequency0 /
                               (1 - (double) ProportionalChain.T * Frequency0 * M));
                F = Math.Round(F, 2);
                CurrentF = F;
                var comparabelVar = Math.Abs(Math.Round(-Math.PI / 2 + Math.Atan(M), 2));
                compareExpression = Math.Abs(Math.Abs(CurrentF) - comparabelVar);
            } while (compareExpression > 0.05);
        }

        private void CalculatePI()
        {
            double F;
            double compareExpression;
            double compareExpression2;

            var comparabelVar = Math.Abs(Math.Round(Math.PI / 2 + Math.Atan(M), 2));
            var comparableVar2 = Math.Abs(Math.Round(Math.PI, 2));

            double IntegratingF = -(Math.PI / 2) - Math.Atan(M);
            double ProportionalF;

            do
            {
                ProportionalF = -(Math.Atan(((double) ProportionalChain.T * Frequency1) /
                                            (1 - (double) ProportionalChain.T * Frequency1 * M)));
                F = IntegratingF + ProportionalF;
                F = Math.Round(F, 2);
                CurrentF = F;
                compareExpression2 = Math.Abs(Math.Abs(CurrentF) - comparableVar2);
                Frequency1 += 0.0005;
            } while (compareExpression2 > 0.05);

            do
            {
                ProportionalF = -(Math.Atan(((double) ProportionalChain.T * Frequency0) /
                                            (1 - (double) ProportionalChain.T * Frequency0 * M)));
                F = IntegratingF + ProportionalF;
                F = Math.Round(F, 2);

                CurrentF = F;

                Frequency0 += 0.0005;

                compareExpression = Math.Abs(Math.Abs(CurrentF) - comparabelVar);
            } while (compareExpression > 0.05);
        }

        private void CalculateLI()
        {
            Frequency0 = 0;
            Frequency1 = 0;
            double F;
            double compareExpression1;
            double compareExpression2;

            var comparableVar1 = Math.Abs(Math.Round(Math.PI / 2 + Math.Atan(M), 2));
            var comparableVar2 = Math.Abs(Math.Round(Math.PI, 2));

            var integratingF = -(Math.PI / 2) - Math.Atan(M);
            double latencyF;
            do
            {
                latencyF = -(Frequency0 * (double) LatencyChain.tau);

                F = latencyF + integratingF;
                F = Math.Round(F, 2);

                CurrentF = F;

                Frequency0 += 0.0005;

                compareExpression1 = Math.Abs(Math.Abs(CurrentF) - comparableVar1);
            } while (compareExpression1 > 0.05);

            do
            {
                latencyF = -(Frequency1 * (double) LatencyChain.tau);

                F = latencyF + integratingF;

                F = Math.Round(F, 2);

                CurrentF = F;

                Frequency1 += 0.0005;

                compareExpression2 = Math.Abs(Math.Abs(CurrentF) - comparableVar2);
            } while (compareExpression2 > 0.05);
        }
        private void CalculateL()
        {
            Frequency0 = 0;
            Frequency1 = 0;
            double compareExpression1;
            double compareExpression2;

            var comparableVar1 = Math.Abs(Math.Round(Math.PI / 2 + Math.Atan(M), 2));
            var comparableVar2 = Math.Abs(Math.Round(Math.PI, 2));
            double latencyF;

            do
            {
                latencyF = -(Frequency0 * (double) LatencyChain.tau);

                CurrentF = latencyF;

                compareExpression1 = Math.Abs(Math.Abs(CurrentF) - comparableVar1);

                Frequency0 += 0.0005;
            } while (compareExpression1 > 0.05);

            do
            {
                latencyF = -(Frequency1 * (double) LatencyChain.tau);

                CurrentF = latencyF;

                compareExpression2 = Math.Abs(Math.Abs(CurrentF) - comparableVar2);

                Frequency1 += 0.0005;
            } while (compareExpression2 > 0.05);
        }

        //TODO: Разобраться, что за хуйня.
        private void CalculateP()
        {
            Frequency0 = 0;
            Frequency1 = 0;
            double compareExpression1;
            double compareExpression2;
            double ProportionalF;

            var comparableVar1 = Math.Abs(Math.Round(Math.PI / 2 + Math.Atan(M), 2));
            var comparableVar2 = Math.Abs(Math.Round(Math.PI, 2));

            do
            {
                ProportionalF = -(Math.Atan(((double) ProportionalChain.T * Frequency0) /
                                            (1 - (double) ProportionalChain.T * Frequency0 * M)));

                ProportionalF = Math.Round(ProportionalF, 2);
                CurrentF = ProportionalF;
                
                compareExpression1 = Math.Abs(Math.Abs(CurrentF) - comparableVar1);
                
                Frequency0 += 0.001;
            } while (compareExpression1 > 0.23);
            
            Frequency1 = Frequency0;

            do
            {
 
                ProportionalF = -(Math.Atan(((double) ProportionalChain.T * Frequency1) /
                                            (1 - (double) ProportionalChain.T * Frequency1 * M)));
                ProportionalF = Math.Round(ProportionalF, 2);
                CurrentF = ProportionalF;

                compareExpression2 = Math.Abs(Math.Abs(CurrentF) - comparableVar2);
                Frequency1 += 0.005;
            } while (compareExpression2 > 0.05);
        }

        //TODO: Реализовать расчёт интегрирующего.
        private void CalculateI()
        {
        }
    }
}