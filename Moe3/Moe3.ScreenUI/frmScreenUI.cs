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
using Moe3.Logging;
using Moe3.Repository;

namespace Moe3.ScreenUI
{
    public partial class frmScreenUI : Form
    {
        private static Logger objLogger;
        public Queue<InventoryItem> listInventoryItems;
        public int mainWidth = 0;
        public int mainHeight = 0;
        public int gWidth = 0;
        public int gHeight = 0;
        public frmScreenUI()
        {
            InitializeComponent();
            objLogger = new Logger();
            
            objLogger.LogMsg(LogModes.UI, LogLevel.INFO, "Moe3 Screen UI Application Initialized !!");
            mainWidth = Screen.PrimaryScreen.Bounds.Width;
            mainHeight =Screen.PrimaryScreen.Bounds.Height;
            gWidth = mainWidth - (int) ((mainWidth * 1) / 100);
            gHeight = mainHeight - (int)((mainHeight * 13) / 100);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listInventoryItems = GetDataSourceFromDb();
            listInventoryItems.Enqueue(listInventoryItems.Dequeue());
            dataGridView1.Width = gWidth;
            dataGridView1.Height = gHeight;
            dataGridView1.DataSource = listInventoryItems.ToList();
            
        }

        private Queue<InventoryItem> GetDataSourceFromDb()
        {
            Queue<InventoryItem> listInventoryItems = new Queue<InventoryItem>();

            try
            {
             listInventoryItems = PartNumberRepository.GetAllNonZeroItems();
                  
            }
            catch (Exception exp)
            {
                objLogger.LogMsg(LogModes.UI, LogLevel.ERROR, "Error GetDataSourceFromDb - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                MessageBox.Show("Error while fetchig Data from DB",
                    "Error in DB Access", MessageBoxButtons.OK, MessageBoxIcon.Error);   
            }
            return listInventoryItems;
        }

        private void screenTime_Tick(object sender, EventArgs e)
        {

            listInventoryItems.Enqueue(listInventoryItems.Dequeue());
            dataGridView1.DataSource = listInventoryItems.ToList();
            dataGridView1.CurrentCell.Selected = false;
        }

        private void dataSourceResetTimer_Tick(object sender, EventArgs e)
        {
            listInventoryItems = GetDataSourceFromDb();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //foreach (DataGridViewRow Myrow in dataGridView1.Rows)
            //{            //Here 2 cell is target value and 1 cell is Volume
            //    if (Myrow.Cells[0].Value.ToString().EndsWith("0"))// Or your condition 
            //    {
            //        Myrow.DefaultCellStyle.BackColor = Color.Red;
            //    }
            //    else
            //    {
            //        Myrow.DefaultCellStyle.BackColor = Color.White;
            //    }
            //}
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //for (int i = 0; i < dataGridView1.DisplayedRowCount(false); ++i)
            //{            //Here 2 cell is target value and 1 cell is Volume

            //    DataGridViewRow Myrow = dataGridView1.Rows[i];
            //    if (Myrow.Cells[0].Value.ToString().EndsWith("0"))// Or your condition 
            //    {
            //        Myrow.DefaultCellStyle.BackColor = Color.Red;
            //    }
            //    else
            //    {
            //        Myrow.DefaultCellStyle.BackColor = Color.White;
            //    }
            //}
        }
    }
}
