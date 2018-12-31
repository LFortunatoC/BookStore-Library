using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HiTech.BLL;
using HiTech.Security;


namespace HiTech.DAL
{
    public class UserDA
    {
        static string filePath = Application.StartupPath + @"\Users.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";
        /// <summary>
        /// This method saves the content of an object user into the  file
        /// Users.dat
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void SaveToFile(User user)
        {
            //Create the object of type StreamWriter and  open the file Users.dat
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(user.User_id + "," + user.UserName + "," + (byte)user.UserLevel);
            }
        }
        
        /// <summary>
        /// This method search an object user by its Id and delete it
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(int id)
        {
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                StreamWriter sw = new StreamWriter(filePath2, true);
                // read the first line
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] fields = line.Split(',');
                    if (id != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }

                    // Read the next line
                    line = sr.ReadLine();
                }

                //Close the files
                sr.Close();
                sw.Close();

                //delete the old file
                File.Delete(filePath);

                //Rename the new file with the old name
                File.Move(filePath2, filePath);
            }
            else
            {
                MessageBox.Show("File Not Found", "Error");
            }
        }

        /// <summary>
        /// This method updates the fields of an object User
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(User anUser)
        {
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                StreamWriter sw = new StreamWriter(filePath2, true);
                // read the first line
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] fields = line.Split(',');
                    if (anUser.User_id != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // write the updated User to the new file
                        sw.WriteLine(anUser.User_id + "," + anUser.UserName + "," + (byte)anUser.UserLevel);
                    }

                    // Read the next line
                    line = sr.ReadLine();
                }

                //Close the files
                sr.Close();
                sw.Close();

                //delete the old file
                File.Delete(filePath);

                //Rename the new file with the old name
                File.Move(filePath2, filePath);
            }
            else
            {
                MessageBox.Show("File Not Found", "Error");
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method search for an ID into the file
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the ID is alread saved, False otherwise/returns>
        public static bool IsDuplicateId(int id)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] fields = line.Split(',');
                        if (Convert.ToInt32(fields[0]) == id)
                        {
                            return true;
                        }
                        // Read the next line
                        line = sr.ReadLine();
                    }
                    return false;  // This is a new Id not duplicated one
                }
            }
            return false;// This id is a string line  OK
        }

        /// <summary>
        /// This method search for the first occurence of an ID into the file.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If found returns an object User, return and User set to null if the ID is not found /returns>
        public static User SearchRecord(int id)
        {
            User anUser = new User();
            if (File.Exists(filePath))  // Check if the file exists beforre reading it.
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // read the first line in the file;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        //split the line to get the Id
                        string[] fields = line.Split(',');

                        if (id == Convert.ToInt32(fields[0]))
                        {
                            // User found
                            anUser.User_id = Convert.ToInt32(fields[0]);
                            anUser.UserName = fields[1];
                            anUser.UserLevel = (Login.UserLevel) Convert.ToInt32(fields[2]);
                            return anUser;
                        }
                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // User not found
            anUser = null;
            return anUser;
        }

        /// <summary>
        /// This method search all the records that match the UserName.
        /// </summary>
        /// <param name="searchName" and Optional [seachBy=searchBy=FirstName]></param>
        /// <returns>A list of User with the records found/returns>
        public static List<User> SearchRecord(string userName)
        {
            List<User> someUsers = new List<User>();

            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // read the first line in the file;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        //split the line to get the Id
                        string[] fields = line.Split(',');

                        if (fields[1] == userName)
                        {
                            // User found
                            User anUser = new User();
                            anUser.User_id = Convert.ToInt32(fields[0]);
                            anUser.UserName = fields[1];
                            anUser.UserLevel = (Login.UserLevel)Convert.ToInt32(fields[2]);
                            someUsers.Add(anUser);
                        }
                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return someUsers;
        }

        /// <summary>
        /// This method list all the records in Users.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all users in the file/returns>
        public static List<User> ListAllRecords()
        {
            List<User> allUsers = new List<User>();
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // read the first line in the file;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        //split the line to get the Id
                        string[] fields = line.Split(',');

                        User anUser = new User();
                        anUser.User_id = Convert.ToInt32(fields[0]);
                        anUser.UserName = fields[1];
                        anUser.UserLevel = (Login.UserLevel)Convert.ToInt32(fields[2]);
                        allUsers.Add(anUser);

                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return allUsers;
        }
    }
}
