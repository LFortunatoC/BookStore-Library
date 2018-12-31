using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HiTech.Security
{
    public static class Login
    {
        public enum UserLevel : byte { NONE = 0, ADMIN = 1, MIS_MANAGER = 2, SALES_MANAGER = 3, CONTROLLER = 4, CLERK = 5 };
        private static string loggedUserId;
        private static bool mustChangePwd;
        private static UserLevel curUserLevel = 0;
        private static string lastStatus;

        private static string filePath = Application.StartupPath + @"\Login.dat";
        private static string tempFilePath = Application.StartupPath + @"\tmp.dat";
        private static string logFilePath = Application.StartupPath + @"\Log.dat";
        
        public static UserLevel CurUserLevel { get => curUserLevel; }
        public static bool MustChangePwd { get => mustChangePwd; }
        public static string LoggedUserId { get => loggedUserId; set => loggedUserId = value; }
        public static string LastStatus { get => lastStatus; set => lastStatus = value; }

        /// <summary>
        /// This method create a user, it first check whether the logged user has enough 
        /// access rights to create another user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <param name="usrlevel"></param>
        /// <returns>True if the user was created succesfully; False Otherwise</returns>
        public static bool CreateUser(string userId, string pwd, UserLevel usrlevel)
        {
            if ((curUserLevel == UserLevel.MIS_MANAGER)|| (curUserLevel == UserLevel.ADMIN))
            {
                string userRecord = SearchUser(userId); //Check if user is alread registered
                if (userRecord != null)
                {
                    MessageBox.Show("User Id already registered", "Duplicaded User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
               
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    /* Record format: USER_ID, User_Level, creation_Date, creator_Id, mustChangePwd, */
                    string newUser = userId + ',' + (byte)usrlevel + ',' + DateTime.Now.ToString() + ',' + LoggedUserId + ',' + "true";
                    
                    //Save user Password
                    Password.Result result= Password.SavePassword(userId, pwd);
                    if (result == Password.Result.PASS)
                    {
                        sw.WriteLine(newUser);
                        WriteToLog("Added User - " + DateTime.Now.ToString() + ", New User Id: " + userId + ",UserLevel: " + usrlevel + ", Added by: " + LoggedUserId);
                        MessageBox.Show("New user created succesfully", "Add New User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Duplicated Password Id", "Duplicated ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                 }
            }
            else
            {
                MessageBox.Show("Not enough access rights to Add an User","Access Denied",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// This method searches and deletes an user by its Id.
        /// First it checks whether the logged user has enough access rights to delete an user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True if the user was deleted/ False otherwise</returns>
        public static bool DeleteUser(string userId)
        {
            if ((curUserLevel == UserLevel.MIS_MANAGER)|| (curUserLevel == UserLevel.ADMIN))
            {
                if (LoggedUserId == userId)
                {
                    MessageBox.Show("User cannot remove himself", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string userRecord = SearchUser(userId);
                if (userRecord != null)
                {
                    Password.Result result = Password.DeletePassword(userId);
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        StreamWriter sw = new StreamWriter(tempFilePath);
                        string line = sr.ReadLine();
                        while (line != null)
                        {
                            string[] fields = line.Split(',');
                            if (userId != fields[0])
                            {
                                sw.WriteLine(line);
                            }
                            line = sr.ReadLine();
                        }
                        sr.Close();
                        sw.Close();
                        File.Delete(filePath);
                        File.Move(tempFilePath, filePath);
                    }
                    WriteToLog("User Removed- " + DateTime.Now.ToString() + ", Removed User Id: " + userId + ", Removed by: " + LoggedUserId);
                    //MessageBox.Show("User Removed succesfully", "User Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("User not found", "User not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Not enough access rights to Add an User", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
        }

        /// <summary>
        /// This method changes User password, it requires confirmation of the current password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns>True if the password was changed; False otherwise</returns>
        public static bool ChangePwd(string userId, string currPwd, string newPwd)
        {
            if(LoggedUserId == userId) // Looged user can change only his password
            {
                Password.Result result = Password.ChangePassword(userId, currPwd, newPwd);
                if (result==Password.Result.PASS)
                {
                    UpdatedPwdStatus(userId, false);
                    WriteToLog("Password Changed  - " + DateTime.Now.ToString() + ", User ID: " + LoggedUserId);
                    //MessageBox.Show("Password Changed succesfully","Password Changed",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Failure changing Password", "Password not Changed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Looged User must be the same who wants to change Password", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        /// <summary>
        /// This method resets the password of an user, Only the administrator or MIS Manager can call it
        /// The method checks the access rights of the logged user before reseting a password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool ResetPwd(string userId, string pwd)
        {
            if((curUserLevel==UserLevel.MIS_MANAGER)|| (curUserLevel == UserLevel.ADMIN))
            {
                Password.Result result = Password.ResetPassword(userId, pwd);
                if (result == Password.Result.PASS)
                {
                        UpdatedPwdStatus(userId, true);
                        WriteToLog("Password Reset- " + DateTime.Now.ToString() + ",User Id: " + userId + ",Reset by: " + LoggedUserId);
                        return true;
                }
            } 
            else
            {
                MessageBox.Show("Not enough access rights to Add an User", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return false;
        }
        
        /// <summary>
        /// This method handles the Login of an user. it checks the userName and after the Password
        /// UserName is case sensitive adn accpet letters and numbers and spaces between them
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool UserLogin(string userId, string pwd)
        {
            string userData = SearchUser(userId);
            if (userData == null)
            {
                MessageBox.Show("User not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Password.Result result =Password.ValidatePassword(userId, pwd);
            if(result==Password.Result.PASS)
            {
                LoggedUserId = userId;
                string[] fields = userData.Split(',');
                curUserLevel =(UserLevel)Convert.ToByte(fields[1]);
                WriteToLog("Login - " + DateTime.Now.ToString() + ", User Id: " + LoggedUserId);
                
                /* Check if user needs to changes his/her password*/
                if (fields[4]=="true") /* Record format: USER_ID, User_Level, creation_Date, creator_Id, mustChangePwd, */
                {
                    mustChangePwd = true; // Update information about User.
                }

                return true;
            }
            else if (result == Password.Result.NOTFOUND)
            {
                MessageBox.Show("User not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (result == Password.Result.FAILED)
            {
                MessageBox.Show("Wrong Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Login Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        /// <summary>
        /// This method perfom the Log out of the current logged user
        /// </summary>
        public static void UserLogout()
        {
            WriteToLog("Logout - " + DateTime.Now.ToString() + ", User Id: " + LoggedUserId );
            LoggedUserId = null;
            curUserLevel=UserLevel.NONE;
        }

        /// <summary>
        /// This method search a User by its username = userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>all user information</returns>
        private static string SearchUser(string userId)
        {
            string userData = null;
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                while(line != null)
                {
                    string[] fields = line.Split(',');
                    if(userId==fields[0])
                    {
                        userData = line;
                        return userData;
                    }
                    line = sr.ReadLine();
                }
            }
            return userData;
        }

        /// <summary>
        /// This method updates the need of the user to change his password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mustChangePwd"></param>
        private static void UpdatedPwdStatus(string userId, bool mustChangePwd)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                StreamWriter sw = new StreamWriter(tempFilePath);
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] fields = line.Split(',');
                    if (userId != fields[0])
                    {
                        sw.WriteLine(line);
                    }
                    else
                    {
                        /* Record format: USER_ID, User_Level, creation_Date, creator_Id, mustChangePwd, */
                        if (mustChangePwd == true)
                        {
                            fields[4] = "true";
                        }
                        else
                        {
                            fields[4] = "false";
                        }
                        sw.WriteLine(fields[0] + ',' + fields[1] + ',' + fields[2] + ',' + fields[3] + ',' + fields[4]);
                    }
                    line = sr.ReadLine();
                }
                sw.Close();
                sr.Close();
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);
            }
        }

        /// <summary>
        /// This method save all login actions in the file Log.dat
        /// </summary>
        /// <param name="msg"></param>
        private static void WriteToLog(string msg)
        {
            using (StreamWriter sw = new StreamWriter(logFilePath,true))
            {
                sw.WriteLine(msg);
            }
        }

    }
}
