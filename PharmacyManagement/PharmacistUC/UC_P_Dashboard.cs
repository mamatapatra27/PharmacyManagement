using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyManagement.PharmacistUC
{
    public partial class UC_P_Dashboard : UserControl
    {

        function fn = new function();
        String query;
        DataSet ds;
        Int64 count;

        public UC_P_Dashboard()
        {
            InitializeComponent();
        }

        // Load dashboard
        private void UC_P_Dashboard_Load(object sender, EventArgs e)
        {
            loadChart();
        }

        public void loadChart()
        {
            // for valid medicine
            query = "select count(mname) from medic where TRY_CONVERT(DATE, eDate, 103) >= GETDATE()";
            ds = fn.getData(query);
            count = Int64.Parse(ds.Tables[0].Rows[0][0].ToString());
            this.chart1.Series["Valid Medicines"].Points.AddXY("Medicine Validity Chart", count);

            // for Expired Medicine
            query = "select count(mname) from medic where TRY_CONVERT(DATE, eDate, 103) <= GETDATE()";
            ds = fn.getData(query);
            count = Int64.Parse(ds.Tables[0].Rows[0][0].ToString());
            this.chart1.Series["Expired Medicines"].Points.AddXY("Medicine Validity Chart", count);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            // for no repeatation reloading
            chart1.Series["Valid Medicines"].Points.Clear();
            chart1.Series["Expired Medicines"].Points.Clear();

            // load chart
            loadChart();
        }
    }
}
