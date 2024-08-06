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
    public partial class UC_P_AddMedicine : UserControl
    {
        function fn = new function();
        String query;

        public UC_P_AddMedicine()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
                String mid = txtMediId.Text;
                String mname = txtMediName.Text;
                String mnumber = txtMediNumber.Text;
                String mdate = txtManufacturingDate.Text;
                String edate = txtExpireDate.Text;
                //Int64 quantity = Int64.Parse(txtQuantity.Text);
                Int64 quantity = 0;
                bool isQuantityValid = Int64.TryParse(txtQuantity.Text, out quantity);

                //Int64 perunit = Int64.Parse(txtPricePerUnit.Text);
                Int64 perunit = 0;
                bool isPerUnitValid = Int64.TryParse(txtPricePerUnit.Text, out perunit);


                //insert query
            if(txtMediId.Text != "" && txtMediName.Text != "" && txtMediNumber.Text != "" && isQuantityValid && quantity != 0 && isPerUnitValid && perunit != 0)
            {
                query = "insert into medic(mid, mname, mnumber, mDate, eDate, quantity, perUnit) values ('" + mid + "', '" + mname + "', '" + mnumber + "', '" + mdate + "', '" + edate + "', '" + quantity + "', '" + perunit + "')";
                fn.setData(query, "Medicine Added to Database");
        
            }
            else
            {
                MessageBox.Show("Missing some Information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        public void clearAll()
        {
            txtMediId.Clear();
            txtMediName.Clear();
            txtMediNumber.Clear();
            txtQuantity.Clear();
            txtPricePerUnit.Clear();
            txtManufacturingDate.ResetText();
            txtExpireDate.ResetText();
        }
    }
}
