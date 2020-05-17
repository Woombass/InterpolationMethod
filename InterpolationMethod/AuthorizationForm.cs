using System;
using System.Windows.Forms;

namespace InterpolationMethod
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
        }

        private void passwordButton_Click(object sender, EventArgs e)
        {
            if (password.Text == "volpt2020ATP41")
            {
                MessageBox.Show("Пароль верный!", "Оповещение", MessageBoxButtons.OK);
                var adminPanel = new Admin_Panel();
                adminPanel.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пароль не верный!", "Алярм!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}