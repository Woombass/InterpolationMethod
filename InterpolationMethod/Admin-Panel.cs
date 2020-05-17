using System;
using System.Windows.Forms;

namespace InterpolationMethod
{
    public partial class Admin_Panel : Form
    {
        public Admin_Panel()
        {
            InitializeComponent();
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Program.ChangePerm(checkBox1.Checked);
        }
    }
}