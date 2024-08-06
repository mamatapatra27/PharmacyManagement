using DGVPrinterHelper;
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
    public partial class UC_P_SellMedicine : UserControl
    {

        function fn = new function();
        String query;
        DataSet ds;

        public UC_P_SellMedicine()
        {
            InitializeComponent();
        }

        private void UC_P_SellMedicine_Load(object sender, EventArgs e)
        {
            // first clear the Items in ListBox
            listBoxMedicines.Items.Clear();
            // select Medicine name which is Valid means not expired.
            query = "select mname from medic where TRY_CONVERT(DATE, eDate, 103) >= GETDATE() and quantity > '0'";
            ds = fn.getData(query);

            //loop repeated number of Medicine name which is valid.
            for(int i=0; i < ds.Tables[0].Rows.Count; i++)
            {
                //add these Medicine name into ListBox
                listBoxMedicines.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        // for Refreshing
        private void btnSync_Click(object sender, EventArgs e)
        {
            UC_P_SellMedicine_Load(this, null);
        }

        // for search Medicine
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // first clear all Items
            listBoxMedicines.Items.Clear();

            // query
            query = "select mname from medic where mname like '" + txtSearch.Text + "%' and TRY_CONVERT(DATE, eDate, 103) >= GETDATE() and quantity > '0'";
            ds = fn.getData(query);

            //loop repeated for above query's medicine name
            for(int i=0; i < ds.Tables[0].Rows.Count; i++)
            {
                listBoxMedicines.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }

        }

        private void listBoxMedicines_SelectedIndexChanged(object sender, EventArgs e)
        {
            // first clear the number of Unit
            txtNoOfUnit.Clear();

            // Extract the name of Medicine which is selected into ListBox, and store it in name.
            String name = listBoxMedicines.GetItemText(listBoxMedicines.SelectedItem);
            
            //set the Medicine name
            txtMediName.Text = name;

            // extract all the details related to this name
            query = "select mid, eDate, perUnit from medic where mname = '"+name+"'";
            ds = fn.getData(query);

            // set the data into textBoxes
            txtMediID.Text = ds.Tables[0].Rows[0][0].ToString();
            txtEDate.Text = ds.Tables[0].Rows[0][1].ToString();
            txtPricePerUnit.Text = ds.Tables[0].Rows[0][2].ToString();

        }

        private void txtNoOfUnit_TextChanged(object sender, EventArgs e)
        {
            if(txtNoOfUnit.Text != "")
            {
                Int64 unitPrice = Int64.Parse(txtPricePerUnit.Text);
                Int64 noOfUnit = Int64.Parse(txtNoOfUnit.Text);
                Int64 totalAmount = unitPrice * noOfUnit;
                txtTotalPrice.Text = totalAmount.ToString();
            }
            else
            {
                txtTotalPrice.Clear();
            }
        }


        // for add to cart button
        protected int n, totalAmount = 0;
        protected Int64 quantity, newQuantity;

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if(txtMediID.Text != "")
            {
                // get quantity
                query = "select quantity from medic where mid = '" + txtMediID.Text + "'";
                ds = fn.getData(query);

                // extract the ds value into and store into quantity
                quantity = Int64.Parse(ds.Tables[0].Rows[0][0].ToString());
                //new quantity
                newQuantity = quantity - Int64.Parse(txtNoOfUnit.Text);

                if(newQuantity >= 0)
                {
                    // Inside n, store the index of new row
                    n = guna2DataGridView1.Rows.Add();

                    guna2DataGridView1.Rows[n].Cells[0].Value = txtMediID.Text;
                    guna2DataGridView1.Rows[n].Cells[1].Value = txtMediName.Text;
                    guna2DataGridView1.Rows[n].Cells[2].Value = txtEDate.Text;
                    guna2DataGridView1.Rows[n].Cells[3].Value = txtPricePerUnit.Text;
                    guna2DataGridView1.Rows[n].Cells[4].Value = txtNoOfUnit.Text;
                    guna2DataGridView1.Rows[n].Cells[5].Value = txtTotalPrice.Text;

                    // Total Amount
                    totalAmount = totalAmount + int.Parse(txtTotalPrice.Text);

                    //Total Label
                    totalLabel.Text = "Rs. " + totalAmount.ToString();

                    //update the quantity of database after add to cart.
                    query = "update medic set quantity = '" + newQuantity + "' where mid = '"+txtMediID.Text+"'";
                    fn.setData(query, "Medicine Added");
                }
                else
                {
                    MessageBox.Show("Medicine is out of Stock.\n Only " + quantity + " left", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //clear all textboxes
                clearAll();
                // again reload this page
                UC_P_SellMedicine_Load(this, null);
            }

            else
            {
                MessageBox.Show("Select Medicine First", "Information !!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Extract the cell value from dataGridView, for remove
        int valueAmount;
        String valueId;
        protected Int64 noOfUnit;
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //extract the cells value and stored it.
                valueAmount = int.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                valueId = guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                noOfUnit = Int64.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());

            }
            catch (Exception)
            {

            }
        }

        // for remove button
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(valueId != "")
            {
                try
                {
                    // remove the selected row.
                    guna2DataGridView1.Rows.RemoveAt(this.guna2DataGridView1.SelectedRows[0].Index);
                }
                catch (Exception)
                {

                }
                finally
                {
                    //first select the quantity from medic
                    query = "select quantity from medic where mid = '" + valueId + "'";
                    ds = fn.getData(query);
                    quantity = Int64.Parse(ds.Tables[0].Rows[0][0].ToString());
                    //after remove the medicine, we again add the removed quantity into quantity, stored it newQuantity
                    newQuantity = quantity + noOfUnit;

                    // query for update the newQuantity into medic table 
                    query = "update medic set quantity = '" + newQuantity + "' where mid = '" + valueId + "'";
                    fn.setData(query, "Medicine Removed From Cart");

                    //calculate the total amount again
                    totalAmount = totalAmount - valueAmount;
                    // update the total label text
                    totalLabel.Text = "Rs. " + totalAmount.ToString();
                }

                //reload the page
                UC_P_SellMedicine_Load(this, null);
            }
        }


        // Purchase And Print
        private void btnPurchasePrint_Click(object sender, EventArgs e)
        {
            DGVPrinter print = new DGVPrinter();
            print.Title = "Medicine Bill";
            print.SubTitle = String.Format("Date:- {0}", DateTime.Now.Date);
            print.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            print.PageNumbers = true;
            print.PageNumberInHeader = false;
            print.PorportionalColumns = true;
            print.HeaderCellAlignment = StringAlignment.Near;
            print.Footer = "Total Payable Amount : " + totalLabel.Text;
            print.FooterSpacing = 15;
            print.PrintDataGridView(guna2DataGridView1);

            // then set totalAmount and label
            totalAmount = 0;
            totalLabel.Text = "Rs. 00";
            guna2DataGridView1.DataSource = 0;
        }


        // for clear all textboxes
        private void clearAll()
        {
            txtMediID.Clear();
            txtMediName.Clear();
            txtNoOfUnit.Clear();
            txtTotalPrice.Clear();
            txtEDate.ResetText();
            txtPricePerUnit.Clear();
        }
    }
}
