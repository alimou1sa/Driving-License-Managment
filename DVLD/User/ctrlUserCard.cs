using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Controls
{
    public partial class ctrlUserCard : UserControl
    {

        private clsUser _User;
        private int _UserID = -1;

        public int UserID
        {
            get { return _UserID; }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.FindByUserID (UserID);
            if (_User == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
                _FillUserInfo();
        }

        private void _FillUserInfo()
        {

            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName.ToString();
            lblJobTitle.Text = _User.JobTitle.ToString();
            lblPassword.Text = _User.Password.ToString();
            lblSalary.Text = _User.Salary.ToString();

            txtPermissions.Text = _User.GetPermissionAsString();


            if (_User.IsActive)
                 lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";

        }

     
        private void _ResetPersonInfo()
        {
            
            ctrlPersonCard1.ResetPersonInfo();
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
            lblJobTitle.Text = "[???]";
            lblPassword.Text = "[???]";
            lblSalary.Text = "[???]";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void lblUserID_Click(object sender, EventArgs e)
        {

        }
    }
}
