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
    public partial class frmReporting : Form
    {
        public frmReporting()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void frmReporting_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtPartNumber.Clear();
            txtVersion.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fromDate = Convert.ToDateTime(dtpFrom.Text).ToString("dd/MM/yyyy");
            string endDate = Convert.ToDateTime(dtpTo.Text).ToString("dd/MM/yyyy");
            

            List<InventoryDaiyFact> listDailyFacts = PartNumberRepository.GetInventoryReport(fromDate, endDate);
            reportViewer1.LocalReport.DataSources.Clear(); //clear report
            reportViewer1.LocalReport.ReportEmbeddedResource = "InventoryReport.rdlc"; // bind reportviewer with .rdlc

            Microsoft.Reporting.WinForms.ReportDataSource dataset = new Microsoft.Reporting.WinForms.ReportDataSource("InventoryDS", listDailyFacts); // set the datasource
            reportViewer1.LocalReport.DataSources.Add(dataset);
            dataset.Value = listDailyFacts;

            reportViewer1.LocalReport.Refresh();
            reportViewer1.RefreshReport();

        }
    }
}
