using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpolationMethod
{
    public partial class ChainEditor : Form
    {
        private AutomizedObject _automizedObject;
        private UnicornController unicorn = new UnicornController();
        public ChainEditor()
        {
            InitializeComponent();
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            //////////////////////////
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            groupBox4.Visible = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                groupBox2.Enabled = true;
                groupBox2.Visible = true;
            }
            if (checkBox1.Checked == false)
            {
                groupBox2.Enabled = false;
                groupBox2.Visible = false;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                groupBox3.Enabled = true;
                groupBox3.Visible = true;
            }
            if (checkBox2.Checked == false)
            {
                groupBox3.Enabled = false;
                groupBox3.Visible = false;
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                groupBox4.Enabled = true;
                groupBox4.Visible = true;
            }
            if (checkBox3.Checked == false)
            {
                groupBox4.Enabled = false;
                groupBox4.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExtendendedCharacteristics graphic = new ExtendendedCharacteristics();
            graphic.Show();
            ControllerExtendedCharacteristics controller = new ControllerExtendedCharacteristics();
            controller.zedGraph = graphic.zedGraphControl1;
            Chain firstChain;
            Chain secondChain;
            Chain thirdChain;
            if (checkBox1.Checked == true)
            {
                firstChain = new Chain(Chain.ChainType.Integrating);
                string num = Koef_Integrating.Text;
                num = num.Replace('.', ',').TrimEnd(' ');
                firstChain.K = double.Parse(num);
                controller.Chains.Add(firstChain);
            }
            if (checkBox2.Checked == true)
            {
                secondChain = new Chain(Chain.ChainType.Latency);
                string num = tau_Latency.Text;
                num = num.Replace('.', ',').TrimEnd(' ');
                secondChain.tau = double.Parse(num);
                controller.Chains.Add(secondChain);
            }
            if (checkBox3.Checked == true)
            {
                thirdChain = new Chain(Chain.ChainType.Proportional);
                string num = koef_Proportional.Text;
                num = num.Replace('.', ',').TrimEnd(' ');
                double k = double.Parse(num);
                num = T_Proportional.Text;
                num = num.Replace('.', ',').TrimEnd(' ');
                double T = double.Parse(num);
                thirdChain.K = k;
                thirdChain.T = T;
                controller.Chains.Add(thirdChain);
            }
            var freqController = new FrequencyCalculator(controller.Chains);
            freqController.Calculate();
            var last = freqController.Frequency1;
            var first = freqController.Frequency0;
            
            
            controller.CalculateExtendedCharacteristics(first,last,0.02);
            
            _automizedObject = new AutomizedObject(controller.Chains);
            
            unicorn.CalculateGamma(_automizedObject.CommonF);
            unicorn.CalculateKp(_automizedObject.CommonA);
            unicorn.CalculateKp_Ti(_automizedObject.CommonA);
            
            controller.DrawExtCh(unicorn.KpTi,unicorn.Kp);











        }
    }
}
