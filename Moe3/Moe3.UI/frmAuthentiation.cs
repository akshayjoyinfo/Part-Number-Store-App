using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Moe3.Repository;

namespace Moe3.UI
{
    public partial class frmAuthentiation : Form
    {
        public frmAuthentiation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOtp.Text))
            {
                try
                {
                    bool loginStatus = PartNumberRepository.ValidateAdminOtp(txtOtp.Text);
                    if (loginStatus == true)
                    {

                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                    }
                    else
                    {

                        MessageBox.Show("Oops!, OTP Passowrd  is wrong", "Admin Login", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Error while logging Authenticate panel", "Login Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("OTP is empty", "Authenticate Login", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
    }
}
