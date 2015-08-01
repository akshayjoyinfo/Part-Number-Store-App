using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Moe3.Domain;
using Moe3.Repository;

namespace Moe3.UI
{
    public partial class frmAdminPanel : Form
    {
        public frmAdminPanel()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtPartNumber.Clear();
            txtVersion.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPartNumber.Text) && !string.IsNullOrEmpty(txtVersion.Text))
            {
                InventoryItem item = new InventoryItem();
                item.PartNumber = txtPartNumber.Text;
                item.Version = Convert.ToInt32(txtVersion.Text);
                bool partNumberExist = PartNumberRepository.CheckPartNumberExist(item);
                if (!partNumberExist)
                {
                    bool addstatus = PartNumberRepository.AddPartNumberAdmin(item);
                    if (addstatus)
                    {
                        MessageBox.Show("Successfully added PartNumber", "PartNumber", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        txtPartNumber.Clear();
                        txtVersion.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Unable to add Part Number and Version", "PartNumber", MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                        txtPartNumber.Clear();
                        txtVersion.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("PartNumber and Version already exist", "PartNumber", MessageBoxButtons.OK,
                        MessageBoxIcon.Asterisk);
                    txtPartNumber.Clear();
                    txtVersion.Clear();
                }
            }
            else
            {
                MessageBox.Show("PartNumber and Version already exist", "PartNumber", MessageBoxButtons.OK,
                          MessageBoxIcon.Asterisk);

                txtPartNumber.Clear();
                txtVersion.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLocation.Text))
            {
                InventoryItem item = new InventoryItem();
                item.Location = txtLocation.Text;
                bool locationExist = PartNumberRepository.CheckPartLocaionExist(item);

                if (!locationExist)
                {
                    bool addStatus = PartNumberRepository.AddLocationColumn(txtLocation.Text);
                    if(addStatus)
                        MessageBox.Show("Location Added successfully", "Location", MessageBoxButtons.OK,
                          MessageBoxIcon.Asterisk);
                    else
                    {
                        MessageBox.Show("Unable to Add Location", "Location", MessageBoxButtons.OK,
                          MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    MessageBox.Show("Location already exist", "Location", MessageBoxButtons.OK,
                          MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("Location should not be empty", "Location", MessageBoxButtons.OK,
                          MessageBoxIcon.Asterisk);
            }
            txtLocation.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Are you sure want to perform Master Reset in DB ",
                "Master Reset DB", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            if (DialogResult.Yes == confirmResult)
            {
                frmAuthentiation adm = new frmAuthentiation();
                DialogResult res =  adm.ShowDialog();

                if (DialogResult.Yes == res)
                {
                    if (PartNumberRepository.ResetInventoryItemsToZero())
                    {
                        MessageBox.Show("Successfully Reset Invetory Items", "Reset Inventory", MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to Reset the Inventory", "Reset Inventory", MessageBoxButtons.OK,
                          MessageBoxIcon.Error);
        
                    }
                }
            }

        }
    }
}
