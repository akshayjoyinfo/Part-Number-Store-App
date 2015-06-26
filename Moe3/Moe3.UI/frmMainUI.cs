using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Moe3.Domain;
using  Moe3.Logging;
using Moe3.Repository;

namespace Moe3.UI
{
    public partial class frmMainUI : Form
    {
        private static Logger objLogger;
        public AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
        public AutoCompleteStringCollection partNumberAutoComplete = new AutoCompleteStringCollection();

        public frmMainUI()
        {
            InitializeComponent();
            objLogger = new Logger();
            objLogger.LogMsg(LogModes.UI,LogLevel.INFO,"Moe3 Application Initialized !!");
            
            

            SetAutoCompleteSources();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void SetAutoCompleteSources()
        {
            List<string> listStrings = PartNumberRepository.GetStoreLocations();
            List<string> listPartNumbers = PartNumberRepository.GetPartNumbers();
            txtLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtLocation.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtPartNumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtPartNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtRestPartNumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtRestPartNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;
            partNo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            partNo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            loc.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            loc.AutoCompleteSource = AutoCompleteSource.CustomSource;
         

            autoComplete.AddRange(listStrings.ToArray());
            partNumberAutoComplete.AddRange(listPartNumbers.ToArray());
            txtLocation.AutoCompleteCustomSource = autoComplete;
            txtPartNumber.AutoCompleteCustomSource = partNumberAutoComplete;
            txtRestPartNumber.AutoCompleteCustomSource = partNumberAutoComplete;
            partNo.AutoCompleteCustomSource = partNumberAutoComplete;
            loc.AutoCompleteCustomSource = autoComplete;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtVersion_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtVersion_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtPartNumber.Text.Trim()) && !string.IsNullOrEmpty(txtVersion.Text.Trim()))
                {
                    string partNumber = txtPartNumber.Text.Trim().ToString();
                    int version = Convert.ToInt32(txtVersion.Text.Trim().ToString());
                    InventoryItem item = new InventoryItem();
                    item.PartNumber = partNumber;
                    item.Version = version;
                    bool foundPartNumber = PartNumberRepository.CheckPartNumberExist(item);
                    if (foundPartNumber == true)
                    {
                        statusLabel.ForeColor = Color.Green;
                        statusLabel.Text = "PartNumber and Version already exist";
                    }
                    else
                    {
                        statusLabel.ForeColor = Color.Red;
                        statusLabel.Text = "PartNumber and Version not Exist, Please Contact Store Admin";
                        MessageBox.Show("PartNumber and Version not Exist, Please Contact Store Admin",
                            "PartNumber - Version Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPartNumber.Clear();
                        txtVersion.Clear();
                        txtQuantity.Clear();
                        txtLocation.Clear();
                        txtPartNumber.Focus();
                    }
                }

            }
            catch (Exception exp)
            {
                objLogger.LogMsg(LogModes.UI,LogLevel.ERROR, "Error while cheking PartNumber and Version - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "invalid PartNumer / Version";
                MessageBox.Show("invalid PartNumer / Version , Please Contact Store Admin",
                    "PartNumber - Version Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtLocation_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLocation.Text.Trim()))
                {
                    string location = txtLocation.Text.Trim().ToString();

                    InventoryItem item = new InventoryItem();
                    item.Location = location;

                    bool foundPartNumber = PartNumberRepository.CheckPartLocaionExist(item);
                    if (foundPartNumber == true)
                    {
                        statusLabel.ForeColor = Color.Green;
                        statusLabel.Text = "Location Exist";
                    }
                    else
                    {
                        statusLabel.ForeColor = Color.Red;
                        statusLabel.Text = "Location  Not Exist, Please Contact Store Admin";
                        MessageBox.Show("Location Not Exist",
                            "Locations Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtLocation.Clear();
                        txtLocation.Focus();
                    }

                }
            }
            catch (Exception exp)
            {
                objLogger.LogMsg(LogModes.UI, LogLevel.ERROR, "Error while cheking PartNumber and Version - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Invalid Location";
                MessageBox.Show("Location Missing , Please Contact Store Admin",
                    "Location Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAddPartNumberGroupSection()
        {
            txtLocation.Clear();
            txtPartNumber.Clear();
            txtQuantity.Clear();
            txtVersion.Clear();

        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string partNumber = txtPartNumber.Text.Trim().ToString();
                int version = Convert.ToInt32(txtVersion.Text.Trim().ToString());
                int quantity = Convert.ToInt32(txtQuantity.Text.Trim().ToString());
                string location = txtLocation.Text.Trim();
                InventoryItem item =  new InventoryItem(partNumber,version,quantity,location);
                bool updated = PartNumberRepository.AddPartNumber(item);
                if (updated)
                {
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Successfully Updated Inventory Item " + partNumber;
                    MessageBox.Show("Successfully Updated Inventory Item " + partNumber,
                        "PartNumber - Sucess Operation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAddPartNumberGroupSection();
                }
            }
            catch (Exception exp)
            {
                objLogger.LogMsg(LogModes.UI, LogLevel.ERROR, "Error while Updating Store Itrms- " + exp.Message + " StackTrace:- " + exp.StackTrace);
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Invalid Operation - Unable to Update ";
                MessageBox.Show("Error while Updating Inventory Items",
                    "PartNumber - Failed Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void btnSearchClear_Click(object sender, EventArgs e)
        {
            txtRestPartNumber.Clear();
            txtRestVersion.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPartNumber.Clear();
            txtVersion.Clear();
            txtQuantity.Clear();
            txtLocation.Clear();
        }

        private void txtRestVersion_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtRestPartNumber.Text.Trim()) && !string.IsNullOrEmpty(txtRestVersion.Text.Trim()))
                {
                    string partNumber = txtRestPartNumber.Text.Trim().ToString();
                    int version = Convert.ToInt32(txtRestVersion.Text.Trim().ToString());
                    InventoryItem item = new InventoryItem();
                    item.PartNumber = partNumber;
                    item.Version = version;
                    bool foundPartNumber = PartNumberRepository.CheckPartNumberExist(item);
                    if (foundPartNumber == true)
                    {
                        statusLabel.ForeColor = Color.Green;
                        statusLabel.Text = "PartNumber and Version already exist";
                    }
                    else
                    {
                        statusLabel.ForeColor = Color.Red;
                        statusLabel.Text = "PartNumber and Version not Exist, Please Contact Store Admin";
                        MessageBox.Show("PartNumber and Version not Exist, Please Contact Store Admin",
                            "PartNumber - Version Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtRestPartNumber.Clear();
                        txtRestVersion.Clear();
                        txtRestPartNumber.Focus();

                    }
                }

            }
            catch (Exception exp)
            {
                objLogger.LogMsg(LogModes.UI, LogLevel.ERROR, "Error while cheking PartNumber and Version - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "invalid PartNumer / Version";
                MessageBox.Show("invalid PartNumer / Version , Please Contact Store Admin",
                    "PartNumber - Version Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtRestPartNumber.Clear();
                txtRestVersion.Clear();
                txtRestPartNumber.Focus();

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string partNumber = txtRestPartNumber.Text.Trim().ToString();
                int version = Convert.ToInt32(txtRestVersion.Text.Trim().ToString());
                InventoryItem item = new InventoryItem();
                item.PartNumber = partNumber;
                item.Version = version;
                List<LocationQty> listLocations = PartNumberRepository.GetPartNumbersByPartNumberVersion(item);
                if (listLocations.Count == 0)
                {
                    statusLabel.ForeColor = Color.Red;
                    statusLabel.Text = "No PartNumber Locations availlable";
                    MessageBox.Show("Quantity is empty for Given PartNumber and Version","Search - PartNumber Operation",MessageBoxButtons.OK,MessageBoxIcon.Warning);

                }
                int totalQty = listLocations.Sum(x => x.Quantity);
                listLocations.Add(new LocationQty("Total :- ", totalQty));
                dgvSearchGrid.DataSource = listLocations;
                
            }
            catch (Exception exp)
            {
                objLogger.LogMsg(LogModes.UI, LogLevel.ERROR, "Error while Searching Location vise Part Numbers - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Error while Searching Location vise Part Numbers";
                MessageBox.Show("Error while Searching Location vise Part Numbers",
                    "Searching - Part Number Locations", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void ver_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(partNo.Text.Trim()) && !string.IsNullOrEmpty(ver.Text.Trim()))
                {
                    string partNumber = partNo.Text.Trim().ToString();
                    int version = Convert.ToInt32(ver.Text.Trim().ToString());
                    InventoryItem item = new InventoryItem();
                    item.PartNumber = partNumber;
                    item.Version = version;
                    bool foundPartNumber = PartNumberRepository.CheckPartNumberExist(item);
                    if (foundPartNumber == true)
                    {
                        statusLabel.ForeColor = Color.Green;
                        statusLabel.Text = "PartNumber and Version already exist";
                    }
                    else
                    {
                        statusLabel.ForeColor = Color.Red;
                        statusLabel.Text = "PartNumber and Version not Exist, Please Contact Store Admin";
                        MessageBox.Show("PartNumber and Version not Exist, Please Contact Store Admin",
                            "PartNumber - Version Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        partNo.Clear();
                        ver.Clear();
                        qty.Clear();
                        loc.Clear();
                        ;
                        partNo.Focus();

                    }
                }

            }
            catch (Exception exp)
            {
                objLogger.LogMsg(LogModes.UI, LogLevel.ERROR, "Error while cheking PartNumber and Version - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "invalid PartNumer / Version";
                MessageBox.Show("invalid PartNumer / Version , Please Contact Store Admin",
                    "PartNumber - Version Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                partNo.Clear();
                ver.Clear();
                qty.Clear();
                loc.Clear();
                ;
                partNo.Focus();
            }

        }

        private void loc_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(loc.Text.Trim()))
                {
                    string location = loc.Text.Trim().ToString();

                    InventoryItem item = new InventoryItem();
                    item.Location = location;

                    bool foundPartNumber = PartNumberRepository.CheckPartLocaionExist(item);
                    if (foundPartNumber == true)
                    {
                        statusLabel.ForeColor = Color.Green;
                        statusLabel.Text = "Location Exist";
                    }
                    else
                    {
                        statusLabel.ForeColor = Color.Red;
                        statusLabel.Text = "Location  Not Exist, Please Contact Store Admin";
                        MessageBox.Show("Location Not Exist",
                            "Locations Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtLocation.Clear();
                        txtLocation.Focus();
                    }

                }
            }
            catch (Exception exp)
            {
                objLogger.LogMsg(LogModes.UI, LogLevel.ERROR, "Error while cheking PartNumber and Version - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Invalid Location";
                MessageBox.Show("Location Missing , Please Contact Store Admin",
                    "Location Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLocation.Clear();
                txtLocation.Focus();
            }

        }

        private void gridTimer_Tick(object sender, EventArgs e)
        {
            dgvSearchGrid.DataSource = null;
            txtRestPartNumber.Clear();
            txtRestVersion.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string partNumber = partNo .Text.Trim().ToString();
                int version = Convert.ToInt32(ver.Text.Trim().ToString());
                int quantity = Convert.ToInt32(qty.Text.Trim().ToString());
                string location = loc.Text.Trim();
                InventoryItem item = new InventoryItem(partNumber, version, quantity, location);
                bool updated = PartNumberRepository.RestorePartNumber(item);
                if (updated)
                {
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Successfully Restoring Inventory Item " + partNumber;
                    MessageBox.Show("Successfully Restore Inventory Item " + partNumber,
                        "PartNumber - Restore Operation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearRestPartNumberGroupSection();
                }
            }
            catch (Exception exp)
            {
                objLogger.LogMsg(LogModes.UI, LogLevel.ERROR, "Error while Restoring Store Itrms- " + exp.Message + " StackTrace:- " + exp.StackTrace);
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "Invalid Operation - Unable to Restore ";
                MessageBox.Show("Error while Restore Inventory Items",
                    "PartNumber - Failed Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void ClearRestPartNumberGroupSection()
        {
            partNo.Clear();
            ver.Clear();
            loc.Clear();
            qty.Clear();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void reportingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReporting frmObj = new frmReporting();
            frmObj.Show();
        }

        
    }
}
