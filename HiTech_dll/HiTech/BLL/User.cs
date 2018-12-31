using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.Security;
using HiTech.DAL;

namespace HiTech.BLL
{
    public class User: IUser
    {
        private int user_id;
        private string userName;
        private Login.UserLevel userLevel;

        public int User_id { get => user_id; set => user_id = value; }
        public string UserName { get => userName; set => userName = value; }
        public Login.UserLevel UserLevel { get => userLevel; set => userLevel = value; }

        /// <summary>
        /// This function check if the id is already in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if it is already in use, False otherwise</returns>
        public bool IsDuplicateId(int id)
        {
            return UserDA.IsDuplicateId(id);
        }

        /// <summary>
        /// This method returns a list with all objetcs saved in the file.
        /// if there is no objects saved the List will be empty (null)
        /// </summary>
        /// <returns>List of object User </returns>
        public List<User> ListAllRecords()
        {
            return UserDA.ListAllRecords();
        }

        /// <summary>
        /// This method save an Object User into file Authors.dat
        /// </summary>
        /// <param name="anUser"></param>
        public void SaveToFile(User anUser)
        {
            UserDA.SaveToFile(anUser);
        }

        /// <summary>
        /// This method search a User by its Id and delete it. It also deletes the dependencies.
        /// In this case login and pwd info 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //=================================================
            // First Delete Dependencies related to Login and 
            // Password are
            //=================================================
            User anUser = new User();
            anUser= UserDA.SearchRecord(id);
            Login.DeleteUser(anUser.UserName);

            //=================================================
            // Now remove the user
            //=================================================    
            UserDA.Delete(id);
        }

        /// <summary>
        ///  This function updates the fields of an object User
        /// </summary>
        /// <param name="anUser"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public bool Update(User anUser)
        {
            return UserDA.Update(anUser);
        }

        /// <summary>
        /// This method searches an User by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an object User that satisfy the search criteria</returns>
        public User SearchRecord(int id)
        {
            return UserDA.SearchRecord(id);
        }

        /// <summary>
        /// This method searches all the objects User by a given name
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns>A list of objects User that satisfy the search criteria</returns>
        public List<User> SearchRecord(string searchName)
        {
            return UserDA.SearchRecord(searchName);
        }

        /// <summary>
        /// This method returns the Id of a given UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns> The userId</returns>
        public int GetUserNameId(string userName)
        {
            int id = 0;

            List<User> someUsers = new List<User>();
            someUsers = SearchRecord(userName);
            if(someUsers.Count>0)
            {
                id = someUsers[0].User_id;
            }
            return id;
        }

        /// <summary>
        /// This method creates an user and the respective login/pwd info
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="userLevel"></param>
        public void CreateUser(string userName, int userId,string password, int userLevel)
        {
            User anUser = new User();
            anUser.User_id = userId;
            anUser.UserName = userName;
            anUser.UserLevel = (Login.UserLevel)userLevel;
            anUser.SaveToFile(anUser);
            Login.CreateUser(anUser.UserName, password, anUser.UserLevel);
        }

        /// <summary>
        /// This method resets user password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>New password</returns>
        public bool ResetPwd(string password)
        {
            return Login.ResetPwd(this.userName, password);
        }
    }
}
