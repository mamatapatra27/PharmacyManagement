using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyManagement.AdministratorUC
{
    public partial class UC_Profile : UserControl
    {

        function fn = new function();
        String query;
        DataSet ds;

        public UC_Profile()
        {
            InitializeComponent();
        }

        //receive the ID value from Administrator page
        public string ID
        {
            set { userNameLabel.Text = value; }
        }

        // Click anywhere inside the form
        private void UC_Profile_Enter(object sender, EventArgs e)
        {
            query = "select * from users where username = '"+userNameLabel.Text+"'";
            ds = fn.getData(query);

            // fill the TextBoxes
            txtUserRole.Text = ds.Tables[0].Rows[0][1].ToString();
            txtName.Text = ds.Tables[0].Rows[0][2].ToString();
            txtDob.Text = ds.Tables[0].Rows[0][3].ToString();
            txtMobile.Text = ds.Tables[0].Rows[0][4].ToString();
            txtEmail.Text = ds.Tables[0].Rows[0][5].ToString();
            txtPassword.Text = ds.Tables[0].Rows[0][7].ToString();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //set the textBoxes value which is actually present in DB
            UC_Profile_Enter(this, null);
        }

        // update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            String role = txtUserRole.Text;
            String name = txtName.Text;
            String dob = txtDob.Text;
            //Int64 mobile = Int64.Parse(txtMobile.Text);
            Int64 mobile = 0;
            bool isMobileValid = Int64.TryParse(txtMobile.Text, out mobile);
            String email = txtEmail.Text;
            String username = userNameLabel.Text;
            String pass = txtPassword.Text;

            if (txtUserRole.SelectedIndex != -1 && name != "" && dob != "" && isMobileValid && mobile != 0 && email != "" && username != "" && pass != "")
            {
                query = "update users set userRole = '" + role + "', name = '" + name + "', dob = '" + dob + "', mobile = '" + mobile + "', email = '" + email + "', pass = '" + pass + "' where username = '" + username + "'";
                fn.setData(query, "Profile Updation Successful");
            }
            else
            {
                MessageBox.Show("Please Enter All Information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
