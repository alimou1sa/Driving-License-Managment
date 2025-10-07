using System;
using System.Data;
using System.Runtime.InteropServices;
using DVLD_DataAccess;

namespace DVLD_Buisness
{
    public  class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int UserID { set; get; }
        public int PersonID { set; get; }
        public clsPerson PersonInfo;
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }
        public string JobTitle { set; get; }
        public int Permissions { set; get; }
        public double Salary { set; get; }


        public clsUser()

        {     
            this.UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;
            this.JobTitle = "";
            this.Permissions = 0;
            this.Salary = 0;
            Mode = enMode.AddNew;
        }

        private clsUser(int UserID, int PersonID, string Username,string Password,
            bool IsActive, string JobTitle , double Salary, int Permissions)

        {
            this.Permissions = Permissions;
            this.Salary = Salary;
            this.JobTitle = JobTitle;
            this.UserID = UserID; 
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.UserName = Username;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            //call DataAccess Layer 

            this.UserID = clsUserData.AddNewUser(this.PersonID,this.UserName,
                this.Password,this.IsActive,this .JobTitle,this .Salary,this .Permissions);

            return (this.UserID != -1);
        }
        private bool _UpdateUser()
        {
            //call DataAccess Layer 

            return clsUserData.UpdateUser(this.UserID,this.PersonID,this.UserName,
                this.Password,this.IsActive,this .JobTitle ,this .Salary ,this .Permissions);
        }
        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = -1,Permission=0;
            string UserName = "", Password = "",JpbTitle="";
            bool IsActive = false;
            double Salory = 0;
            bool IsFound = clsUserData.GetUserInfoByUserID
                                ( UserID,ref PersonID, ref UserName,ref Password,ref IsActive,ref JpbTitle,ref Salory,ref Permission);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID,PersonID,UserName,Password,IsActive, JpbTitle,Salory, Permission);
            else
                return null;
        }
        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1,Permissions=0;
            string UserName = "", Password = "", JobTitle="";
            bool IsActive = false;
            double Salory = 0;
            bool IsFound = clsUserData.GetUserInfoByPersonID
                                (PersonID, ref UserID, ref UserName, ref Password, ref IsActive,ref JobTitle,ref Salory,ref Permissions);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID, UserID, UserName, Password, IsActive,JobTitle,Salory,Permissions);
            else
                return null;
        }
        public static clsUser FindByUsernameAndPassword(string UserName,string Password)
        {
            int UserID = -1, Permissions=0;
            int PersonID=-1;
           double Salory = 0;
            bool IsActive = false;
            string JobTitle = "";
            bool IsFound = clsUserData.GetUserInfoByUsernameAndPassword
                                (UserName , Password,ref UserID,ref PersonID, ref IsActive,ref JobTitle,ref Salory,ref Permissions);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID, PersonID, UserName, Password, IsActive,JobTitle,Salory,Permissions);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUser();

            }

            return false;
        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID); 
        }

        public static bool isUserExist(int UserID)
        {
           return clsUserData.IsUserExist(UserID);
        }

        public static bool isUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool isUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }

        public enum enPermissions
        {
            FullAccess = -1, ManageApplications = 1, ManagePeople = 2, ManageDrivers = 4,
            ManageUsers = 8, ManageAccountSetting = 16
        };
        public enPermissions _permissions;

        public bool CheckAccessPermission(enPermissions Permission)
        {
            int _Permission = (int)Permission;

            if (this.Permissions == (int)enPermissions.FullAccess)
                return true;

            if ((_Permission & this.Permissions) == _Permission)
                return true;
            else
                return false;
        }

        public string GetPermissionAsString()
        {
            string Permissions = "";

            if (this.CheckAccessPermission(clsUser.enPermissions.FullAccess))
            {
                Permissions += " Full Access , ";
                return Permissions.Remove(Permissions.Length - 2);
            }
            if (this.CheckAccessPermission(clsUser.enPermissions.ManageApplications))
                Permissions += " Manage Applications ,";
            if (this.CheckAccessPermission(clsUser.enPermissions.ManagePeople))
                Permissions += " Manage People ,";
            if (this.CheckAccessPermission(clsUser.enPermissions.ManageDrivers))
                Permissions += " Manage Drivers , ";
            if (this.CheckAccessPermission(clsUser.enPermissions.ManageUsers))
                Permissions += " Manage Users , ";
            if (this.CheckAccessPermission(clsUser.enPermissions.ManageAccountSetting))
                Permissions += " Manage AccountSetting , ";


            return Permissions.Remove(Permissions.Length - 2);
        }
    }
}
