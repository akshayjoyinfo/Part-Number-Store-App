using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Moe3.Logging;
using Moe3.Repository;

namespace Moe3.UI
{
    public partial class frmAdmin : Form
    {
        private static Logger objLogger;
        public frmAdmin()
        {
            InitializeComponent();
            objLogger = new Logger();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text))
            {
                try
                {
                    bool loginStatus = PartNumberRepository.ValidateAdminUser(txtUsername.Text, txtPassword.Text);
                    if (loginStatus == true)
                    {
                        
                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                    }
                    else
                    {

                        MessageBox.Show("Oops!, Username or Password is wrong", "Admin Login", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Error while logging Admin panel", "Login Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Username or Password is Empty", "Admin Login", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            
        }
    }
}
