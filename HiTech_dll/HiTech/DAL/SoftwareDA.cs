using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HiTech.BLL;

namespace HiTech.DAL
{
    public class SoftwareDA
    {
        static string filePath = Application.StartupPath + @"\Softwares.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";
        public enum SearchBy : int { ID = 0, Author = 1, SupllierID = 2 };

        /// <summary>
        /// This method saves the content of an object Software into the  file
        /// Softwares.dat
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void SaveToFile(Software aSw)
        {

            //Create the object of type StreamWriter and  open the file Users.dat
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(aSw.ProductId.ToString() + ';' + aSw.Title + ';' +
                              aSw.Year.ToString() + ';' + aSw.Category + ';' +
                              aSw.UnitPrice.ToString() + ';' + aSw.SupplierId + ';' +
                              aSw.Version + ';' + aSw.QOH.ToString());
            }

        }

        /// <summary>
        /// This method seach a Software by its Id and delete it
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
                    string[] fields = line.Split(';');
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
        ///  This function updates the fields of an object Software
        /// </summary>
        /// <param name="Software object"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(Software aSw)
        {
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                StreamWriter sw = new StreamWriter(filePath2, true);
                // read the first line
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] fields = line.Split(';');
                    if (aSw.ProductId != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // write the updated Software to the new file
                        sw.WriteLine(aSw.ProductId.ToString() + ';' + aSw.Title + ';' +
                                     aSw.Year.ToString() + ';' + aSw.Category + ';' +
                                     aSw.UnitPrice.ToString() + ';' + aSw.SupplierId + ';' +
                                     aSw.Version + ';' + aSw.QOH.ToString());
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
                        string[] fields = line.Split(';');
                        if (Convert.ToInt32(fields[0]) == id)
                        {
                            return true; // ID already in use--new one will is duplicated
                        }
                        // Read the next line
                        line = sr.ReadLine();
                    }
                    return false;  // This is a new Id not duplicated one
                }
            }
            return false;
        }

        /// <summary>
        /// This method search for the first occurence of an ID into the file.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If found returns an object User, return and User set to null if the ID is not found /returns>
        public static Software SearchRecord(int id)
        {

            Software aSw = new Software();
            if (File.Exists(filePath))  // Check if the file exists beforre reading it.
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // read the first line in the file;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        //split the line to get the Id
                        string[] fields = line.Split(';');

                        if (id == Convert.ToInt32(fields[0]))
                        {
                            // Software found
                            aSw.ProductId = Convert.ToInt32(fields[0]);
                            aSw.Title = fields[1];
                            aSw.Year = Convert.ToInt32(fields[2]);
                            aSw.Category = fields[3];
                            aSw.UnitPrice = Convert.ToDecimal(fields[4]);
                            aSw.SupplierId = Convert.ToInt32(fields[5]);
                            aSw.Version = fields[6];
                            aSw.QOH = Convert.ToInt32(fields[7]);
                            return aSw;
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
            aSw = null;
            return aSw;
        }

        /// <summary>
        /// This method search all the records that match the searchName.
        /// According to the optional parameter serachBy(enum) the method 
        /// </summary>
        /// <param name="searchName" and Optional [seachBy=searchBy=FirstName]></param>
        /// <returns>A list of User with the records found/returns>
        public static List<Software> SearchRecord(string searchName)
        {
            List<Software> someSws = new List<Software>();

            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // read the first line in the file;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        //split the line to get the Id
                        string[] fields = line.Split(';');

                        if (fields[1] == searchName)
                        {
                            // User found
                            Software aSw = new Software();
                            aSw.ProductId = Convert.ToInt32(fields[0]);
                            aSw.Title = fields[1];
                            aSw.Year = Convert.ToInt32(fields[2]);
                            aSw.Category = fields[3];
                            aSw.UnitPrice = Convert.ToDecimal(fields[4]);
                            aSw.SupplierId = Convert.ToInt32(fields[5]);
                            aSw.Version = fields[6];
                            aSw.QOH = Convert.ToInt32(fields[7]);
                            someSws.Add(aSw);
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

            return someSws;
        }

        public static List<Software> SearchRecord(int Id, SearchBy searchBy=0)
        {
            List<Software> someSws = new List<Software>();

            if (File.Exists(filePath))
            {
                int Idx = 0;
                if (searchBy == SearchBy.SupllierID) { Idx = 5; }
                if (searchBy == SearchBy.Author) { Idx = 2; }
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // read the first line in the file;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        //split the line to get the Id
                        string[] fields = line.Split(';');

                        if (Convert.ToInt32(fields[Idx]) == Id)
                        {
                            // User found
                            Software aSw = new Software();
                            aSw.ProductId = Convert.ToInt32(fields[0]);
                            aSw.Title = fields[1];
                            aSw.Year = Convert.ToInt32(fields[2]);
                            aSw.Category = fields[3];
                            aSw.UnitPrice = Convert.ToDecimal(fields[4]);
                            aSw.SupplierId = Convert.ToInt32(fields[5]);
                            aSw.Version = fields[6];
                            aSw.QOH = Convert.ToInt32(fields[7]);
                            someSws.Add(aSw);
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

            return someSws;
        }

        /// <summary>
        /// This method list all the records in Software.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all Software in the file/returns>
        public static List<Software> ListAllRecords()
        {
            List<Software> allSws = new List<Software>();
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // read the first line in the file;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        //split the line to get the Id
                        string[] fields = line.Split(';');

                        Software aSw = new Software();
                        aSw.ProductId = Convert.ToInt32(fields[0]);
                        aSw.Title = fields[1];
                        aSw.Year = Convert.ToInt32(fields[2]);
                        aSw.Category = fields[3];
                        aSw.UnitPrice = Convert.ToDecimal(fields[4]);
                        aSw.SupplierId = Convert.ToInt32(fields[5]);
                        aSw.Version = fields[6];
                        aSw.QOH = Convert.ToInt32(fields[7]);
                        allSws.Add(aSw);

                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return allSws;
        }
    }
}
