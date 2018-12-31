using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;

namespace HiTech.Security
{
    public static class Password
    {
        static string filePath = Application.StartupPath + @"\usrs.dat";
        static string tempFilePath = Application.StartupPath + @"\tmp.dat";
        //static private string userID = "";
        static private string hash="";
        static private string creationDate = "";
        public enum Result {PASS=0,FAILED=1,NOTFOUND=3, DUPLICATED =4};

        /// <summary>
        /// This method validates the entered password against the registered one
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns>PASS=0,FAILED=1,NOTFOUND=3</returns>
        public static Result ValidatePassword(string userId, string pwd)
        {
            string reference = SearchRecord(userId);
            if(reference != null)
            {
                string[] fields = reference.Split(',');
                string hashed = HashPwd(fields[1]+pwd); // Only the Hash is stored, not the pwd
                if(hashed == fields[2]) // Check if the hash matches with the stored one
                {
                    return Result.PASS;
                }
                else
                {
                    return Result.FAILED;
                }
            }
            return Result.NOTFOUND;
        }

        /// <summary>
        /// This method changes the password of a given userName, but
        /// first validate the current pwd then updated it
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public static Result ChangePassword(string userId,string currPwd, string newPwd)
        {
            Result result= ValidatePassword(userId, currPwd); // First validate the current pwd
            if (result==Result.PASS) // If the current pwd is validated update to the new one
            {
                result= ResetPassword(userId, newPwd); //Save the new pwd
                return Result.PASS;
            }
            return Result.FAILED;
        }

        /// <summary>
        /// This method changes the password of a given userName.
        /// There is no checking of the current password. This method must be called only 
        /// by the system adminstrator, or MIS Manager
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public static Result ResetPassword(string userId,string newPwd)
        {
            Result result = DeletePassword(userId);
            SavePassword(userId, newPwd);
            return Result.PASS;
        }


        /// <summary>
        /// This method delete an User by its userName
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Result DeletePassword(string userId)
        {
            string userFound = SearchRecord(userId);

            if (userFound == null)
            {
                return Result.NOTFOUND;
            }

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
                return Result.PASS;
            }
        }

        /// <summary>
        /// This method Save the Hash of user password 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static Result SavePassword(string userId,string pwd)
        {

            string reference = SearchRecord(userId);
            if (reference != null)
            {
                return Result.DUPLICATED;
            }

            DateTime currDate = System.DateTime.Now;
            creationDate = currDate.ToString();
            hash = HashPwd(creationDate + pwd); // To increase security hash more data than only pwd

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(userId + ',' + currDate.ToString() + ',' + hash);
            }
            return Result.PASS;

        }


        /// <summary>
        /// This method search a User by its userName
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private static string SearchRecord(string userID)
        {
            string record = null;
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                while (line !=null)
                {
                    string[] fields = line.Split(',');
                    if (userID == fields[0])
                    {
                        record = line;
                        return record;
                    }
                    line = sr.ReadLine();
                }
            }
                return record;
        }

        /// <summary>
        /// This private method generates the hash code of the user pwd
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        private static string HashPwd(string Input)
        {
            byte[] plain = Encoding.ASCII.GetBytes(Input);  // Get plain user pwd and converts t byte array
            HashAlgorithm sha2 = SHA256CryptoServiceProvider.Create(); //Create a object HashAlgorithm
            byte[] hashed = sha2.ComputeHash(plain); // Hash the plain pwd
            string hash = "";
            foreach(byte b in hashed)
            {
                hash = hash + b.ToString("X2"); // get for every hash bytes its hexadecimal representation
            }
            return hash;
        }
    }
}
