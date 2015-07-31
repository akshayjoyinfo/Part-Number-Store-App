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
    public partial class frmLiveStock : Form
    {
        public frmLiveStock()
        {
            InitializeComponent();
        }

        private void frmLiveStock_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            Queue<InventoryItem> listInventoryItems  = PartNumberRepository.GetAllNonZeroItems();
            reportViewer1.LocalReport.DataSources.Clear(); //clear report
            reportViewer1.LocalReport.ReportEmbeddedResource = "LiveStock.rdlc"; // bind reportviewer with .rdlc

            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("LiveStockDataset", listInventoryItems.ToList()); // set the datasource
            reportViewer1.LocalReport.DataSources.Add(dataset);
            dataset.Value = listInventoryItems.ToList();

            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
