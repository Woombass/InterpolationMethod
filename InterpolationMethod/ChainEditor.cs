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
            FormBorderStyle = FormBorderStyle.FixedDialog;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            //////////////////////////
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            groupBox4.Visible = false;
        }
        
        
        public double GetNumbers(string nums)
        {
            nums = nums.Replace('.', ',').TrimEnd(' ');
            return Convert.ToDouble(nums);
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
            ControllerExtendedCharacteristics controller = new ControllerExtendedCharacteristics();
            controller.zedGraph = graphic.zedGraphControl1;
            Chain firstChain;
            Chain secondChain;
            Chain thirdChain;

            try
            {
                if (checkBox1.Checked == true)
                {
                    firstChain = new Chain(Chain.ChainType.Integrating);

                    firstChain.K = GetNumbers(Koef_Integrating.Text);
                    controller.Chains.Add(firstChain);
                }
                if (checkBox2.Checked == true)
                {
                    secondChain = new Chain(Chain.ChainType.Latency);
                    secondChain.tau = GetNumbers(tau_Latency.Text);
                    controller.Chains.Add(secondChain);
                }
                if (checkBox3.Checked == true)
                {
                    thirdChain = new Chain(Chain.ChainType.Proportional);
                    double k = GetNumbers(koef_Proportional.Text);
                    double T = GetNumbers(T_Proportional.Text);
                    thirdChain.K = k;
                    thirdChain.T = T;
                    controller.Chains.Add(thirdChain);
                }
                var freqController = new FrequencyCalculator(controller.Chains);
                freqController.Calculate();
                var last = freqController.Frequency1;
                var first = freqController.Frequency0;
                var step = (last - first) / 15;
            
            
                controller.CalculateExtendedCharacteristics(first,last,step);
            
                _automizedObject = new AutomizedObject(controller.Chains);
            
                unicorn.CalculateGamma(_automizedObject.CommonF);
                unicorn.CalculateKp(_automizedObject.CommonA);
                unicorn.CalculateKp_Ti(_automizedObject.CommonA, first, last, step);
            
                controller.DrawExtCh(unicorn.KpTi,unicorn.Kp);
                graphic.Show();
                MessageBox.Show("Для расчёта параметров регулятора нажмите ЛКМ по точке, правее вершины ЛРЗ!",
                    "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Читайте ошибку! " + exception.Message ,"Алярм!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
