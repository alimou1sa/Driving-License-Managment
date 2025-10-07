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
using DVLD_Buisness;
using DVLD.Classes;
using DVLD.People;
using DVLD.Controls;
using System.Runtime.Remoting.Messaging;

namespace DVLD.User
{
    public partial class frmAddUpdateUser: Form
    {

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _UserID = -1;
        clsUser _User;
       
        public frmAddUpdateUser()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _UserID = UserID;
        }

        private void _ResetDefualtValues()
        {
      

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();
             
                tpLoginInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";
             
                tpLoginInfo.Enabled = true;
                btnSave.Enabled=true;
             

            }
            
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtJobTitle.Text = "";
            txtSalary.Text = "";
            chkIsActive.Checked = true; 
        }

        private void _LoadData()
        {

            _User = clsUser.FindByUserID(_UserID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _User, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
            txtSalary.Text=_User.Salary.ToString();
            txtJobTitle.Text = _User.JobTitle.ToString();
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
            _SetCheckedPemissions();
        }

        void _SetCheckedPemissions()
        {
            if (_User.Permissions == -1)
            {
                chbFullAccess.Checked = true;
                chbManagePeople.Checked = true;
                chbManagrApplication.Checked = true;
                chbMenageUsers.Checked = true;
                return;
            }

            else if (_User.CheckAccessPermission(clsUser.enPermissions.ManagePeople))
            {
                chbFullAccess.Checked = false;
                chbManagePeople.Checked = true;
                chbManagrApplication.Checked = false;
                chbMenageUsers.Checked = false;
                chbManageDrivers.Checked = false;
                chbManageAccountSetting.Checked = false;

            }

            else if (_User.CheckAccessPermission(clsUser.enPermissions.ManageApplications))
            {
                chbFullAccess.Checked = false;
                chbManagePeople.Checked = false;
                chbManagrApplication.Checked = true;
                chbMenageUsers.Checked = false;
                chbManageDrivers.Checked = false;
                chbManageAccountSetting.Checked = false;
            }
            else if (_User.CheckAccessPermission(clsUser.enPermissions.ManageDrivers))
            {
                chbFullAccess.Checked = false;
                chbManagePeople.Checked = false;
                chbManagrApplication.Checked = false;
                chbMenageUsers.Checked = false;
                chbManageDrivers.Checked = true;
                chbManageAccountSetting.Checked = false;
            }
            else if (_User.CheckAccessPermission(clsUser.enPermissions.ManageUsers))
            {
                chbFullAccess.Checked = false;
                chbManagePeople.Checked = false;
                chbManagrApplication.Checked = false;
                chbMenageUsers.Checked = true;
                chbManageDrivers.Checked = false;
                chbManageAccountSetting.Checked = false;
            }
            else if (_User.CheckAccessPermission(clsUser.enPermissions.ManageAccountSetting))
            {
                chbFullAccess.Checked = false;
                chbManagePeople.Checked = false;
                chbManagrApplication.Checked = false;
                chbMenageUsers.Checked = false;
                chbManageDrivers.Checked = false;
                chbManageAccountSetting.Checked = true;

            }


        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                    e.Cancel = true;
                    errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            };

        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if ( string.IsNullOrEmpty ( txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            };

        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            };


            if (_Mode == enMode.AddNew)
            {

                if (clsUser.isUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is used by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                };
            } 
            else
            {
                if (_User.UserName !=txtUserName.Text.Trim())
                {
                        if (clsUser.isUserExist(txtUserName.Text.Trim()))
                        {
                            e.Cancel = true;
                            errorProvider1.SetError(txtUserName, "username is used by another user");
                            return;
                        }
                        else
                        {
                            errorProvider1.SetError(txtUserName, null);
                        };
                }
            }
        }


        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void btnPersonInfoNext_Click_1(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                if (clsUser.isUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {

                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }

                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                }
            }

            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();

            }

        }
        int _ReadPermissionsToSet()
        {

            int Permissions = 0;

            if (chbFullAccess.Checked)
                Permissions += (int)clsUser.enPermissions.FullAccess;
            if (chbManagrApplication.Checked)
                Permissions += (int)clsUser.enPermissions.ManageApplications;
            if (chbManagePeople.Checked)
                Permissions += (int)clsUser.enPermissions.ManagePeople;
            if (chbMenageUsers.Checked)
                Permissions += (int)clsUser.enPermissions.ManageUsers;
            if (chbManageDrivers.Checked)
                Permissions += (int)clsUser.enPermissions.ManageDrivers;
            if (chbManageAccountSetting.Checked)
                Permissions += (int)clsUser.enPermissions.ManageAccountSetting;
            
            return Permissions;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
     
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chkIsActive.Checked;
            _User.Salary = Convert.ToDouble(txtSalary.Text);
            _User.JobTitle = txtJobTitle.Text;
            _User.Permissions = _ReadPermissionsToSet();

            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this .Close();
        }

        private void tpLoginInfo_Click(object sender, EventArgs e)
        {

        }

        private void txtSalary_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            };
        }

        private void chbFullAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (chbFullAccess.Checked)
            {
                chbManagePeople.Checked = true;
                chbManageDrivers.Checked = true;
                chbMenageUsers.Checked = true;
                chbManageAccountSetting.Checked = true;
                chbManagrApplication.Checked = true;

            }
            if (!chbFullAccess.Checked)
            {
                chbManagePeople.Checked = false;
                chbManageDrivers.Checked = false;
                chbMenageUsers.Checked = false;
                chbManageAccountSetting.Checked = false;
                chbManagrApplication.Checked = false;
            }
        }
    }
}
