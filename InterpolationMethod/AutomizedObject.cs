using System;
using System.Collections.Generic;
using System.Linq;

namespace InterpolationMethod
{
    public class AutomizedObject
    {
        private List<Chain> Chains { get;}
        public List<double> CommonA { get;}
        public List<double> CommonF { get; }


        public AutomizedObject(List<Chain> chains)
        {
            if (chains == null)
            {
                throw new ArgumentNullException(nameof(chains),"Chains не может равняться NULL!");
            }
            Chains = chains;
            CommonA = new List<double>();
            CommonF = new List<double>();
            for (int j = 0; j < chains[0].A.Count ; j++)
            {
                double number1 = chains[0].A[j];
                double number2 = 1;
                double number3 = 1;
                if (chains.Count >= 2)
                {
                    number2 = chains[1].A[j];
                }

                if (chains.Count >= 3)
                {
                    number3 = chains[2].A[j];
                }
                double finalNumber = number1 * number2 * number3;
                CommonA.Add(finalNumber);
            }
            
            for (int j = 0; j < chains[0].F.Count ; j++)
            {
                double number1 = chains[0].F[j];
                double number2 = 0;
                double number3 = 0;
                if (chains.Count >=2)
                {
                    number2 = chains[1].F[j];
                }
                if (chains.Count >= 3)
                {
                    number3 = chains[2].F[j];
                }
                double finalNumber = number1 + number2 + number3;
                CommonF.Add(finalNumber);
            }
        }
        
        
    }
}