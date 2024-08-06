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
    public partial class UC_AddUser : UserControl
    {

        function fn = new function();
        String query;
        public UC_AddUser()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            String role = txtUserRole.Text;
            String name = txtName.Text;
            String dob = txtDob.Text;
            //Int64 mobile = Int64.Parse(txtMobileNo.Text);
            Int64 mobile = 0;
            bool isMobileValid = Int64.TryParse(txtMobileNo.Text, out mobile);
            String email = txtEmail.Text;
            String username = txtUsername.Text;
            String pass = txtPassword.Text;

            try
            {
                if(txtUserRole.SelectedIndex != -1 && name != "" && dob != "" && isMobileValid && mobile != 0 && email != "" && username != "" && pass != "")
                {
                    query = "insert into users (userRole,name,dob,mobile,email,username,pass) values ('"+role+"', '"+name+"', '"+dob+"', '"+mobile+"', '"+email+"', '"+username+"', '"+pass+"')";
                    fn.setData(query, "Sign Up Successfully");
                }
                else
                {
                    MessageBox.Show("Please Enter All Information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Username already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        public void clearAll()
        {
            txtName.Clear();
            txtDob.ResetText();
            txtMobileNo.Clear();
            txtEmail.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtUserRole.SelectedIndex = -1;
            lblYesNo.Text = "";
        }

        private void UC_AddUser_Load(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            query = "Select * from users where username = '" + txtUsername.Text + "'";
            DataSet ds = fn.getData(query);

            string basepath = AppDomain.CurrentDomain.BaseDirectory;

            // if ds dataSet contains no datarow 
            if (ds.Tables[0].Rows.Count == 0)              
            {
                lblYesNo.Text = "OK";
                lblYesNo.ForeColor = Color.Green;
            }
            else
            {
                lblYesNo.Text = "Wrong";
                lblYesNo.ForeColor = Color.Red;
                //pictureBox1.ImageLocation = @"C:\Users\MAMATA PATRA\source\repos\PharmacyManagement\PharmacyManagement\Images\no.png";
            }
        }
    }
}
