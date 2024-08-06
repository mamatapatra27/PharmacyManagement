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
    public partial class UC_P_MedicineValidityCheck : UserControl
    {
        function fn = new function();
        String query;
        public UC_P_MedicineValidityCheck()
        {
            InitializeComponent();
        }

        private void txtCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            // for valid medicines
            if(txtCheck.SelectedIndex == 0)
            {
                query = "select * from medic where TRY_CONVERT(DATE, eDate, 103) >= GETDATE()";

                //repeated method
                setDataGridView(query, "Valid Medicines", Color.Black);
            }

            // for Expired Medicines
            else if (txtCheck.SelectedIndex == 1)
            {
                query = "select * from medic where TRY_CONVERT(DATE, eDate, 103) <= GETDATE()";

                //repeated method
                setDataGridView(query, "Expired Medicines", Color.Red);
            }

            // for all Medicines
            if (txtCheck.SelectedIndex == 2)
            {
                query = "select * from medic";

                //repeated method
                setDataGridView(query, "All Medicines", Color.Black);
            }
        }

        // for repeated coding make a method
        private void setDataGridView(String query, String labelName, Color col)
        {
            DataSet ds = fn.getData(query);
            guna2DataGridView1.DataSource = ds.Tables[0];

            //set Label
            setLabel.Text = labelName;
            setLabel.ForeColor = col;
        }

        private void UC_P_MedicineValidityCheck_Load(object sender, EventArgs e)
        {

            // set the label text when page load.
            query = "select * from medic";

            //repeated method
            setDataGridView(query, "All Medicines", Color.Black);

        }
    }
}
