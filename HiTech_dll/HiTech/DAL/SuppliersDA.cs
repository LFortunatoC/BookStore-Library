using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.BLL;
using System.Windows.Forms;
using System.IO;

namespace HiTech.DAL
{
    public class SuppliersDA
    {
        static string filePath = Application.StartupPath + @"\Suppliers.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";

        /// <summary>
        /// This method saves the content of an object Suppliers into the  file
        /// Suppliers.dat
        /// </summary>
        /// <param name="Supplier"></param>
        /// <returns></returns>
        public static void SaveToFile(Suppliers supplier)
        {

            //Create the object of type StreamWriter and  open the file Suppliers.dat
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(supplier.Id + "," + supplier.Name + "," + supplier.PhoneNum + "," + supplier.FaxNum + "," +
                             supplier.Street + "," + supplier.PostalCode + "," + supplier.City + "," +
                             supplier.ProductId);
            }

        }

        /// <summary>
        /// Ths method search a object Supplier by its Id and delete it
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
        ///  This function updates the fields of an object Supplier
        /// </summary>
        /// <param name="Supplier"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(Suppliers supplier)
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
                    if (supplier.Id != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // write the updated Supplier to the new file
                        sw.WriteLine(supplier.Id + "," + supplier.Name + "," + supplier.PhoneNum + "," + supplier.FaxNum + "," +
                                     supplier.Street + "," + supplier.PostalCode + "," + supplier.City + "," +
                                     supplier.ProductId);
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
            return false;// This id isstring line  OK
        }

        /// <summary>
        /// This method search for the first occurence of an ID into the file.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If found returns an object Supplier, return a Supplier set to null if the ID is not found /returns>
        public static Suppliers SearchRecord(int id)
        {
            Suppliers aSupplier = new Suppliers();
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
                            // Supplier found
                            aSupplier.Id = Convert.ToInt32(fields[0]);
                            aSupplier.Name = fields[1];
                            aSupplier.PhoneNum = fields[2];
                            aSupplier.FaxNum = fields[3];
                            aSupplier.Street = fields[4];
                            aSupplier.PostalCode = fields[5];
                            aSupplier.City = fields[6];
                            aSupplier.ProductId = Convert.ToInt32(fields[7]);
                            return aSupplier;
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

            // Supplier not found
            aSupplier = null;
            return aSupplier;
        }

        /// <summary>
        /// This method search all the records that match the searchName.
        /// </summary>
        /// <param name="serachName" and Optional [seachBy=searchBy=FirstName]></param>
        /// <returns>A list of Suppliers with the records found/returns>
        public static List<Suppliers> SearchRecord(string searchName)
        {
            List<Suppliers> someSuppliers = new List<Suppliers>();

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

                        if (searchName == fields[1])
                        {
                            // Supplier found
                            Suppliers aSupplier = new Suppliers();
                            aSupplier.Id = Convert.ToInt32(fields[0]);
                            aSupplier.Name = fields[1];
                            aSupplier.PhoneNum = fields[2];
                            aSupplier.FaxNum = fields[3];
                            aSupplier.Street = fields[4];
                            aSupplier.PostalCode = fields[5];
                            aSupplier.City = fields[6];
                            aSupplier.ProductId = Convert.ToInt32(fields[7]);
                            someSuppliers.Add(aSupplier);
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
            return someSuppliers;
        }

        /// <summary>
        /// This method list all the records in Suppliers.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all Suppliers in the file/returns>
        public static List<Suppliers> ListAllRecords()
        {
            List<Suppliers> allSuppliers = new List<Suppliers>();
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

                        Suppliers aSupplier = new Suppliers();
                        aSupplier.Id = Convert.ToInt32(fields[0]);
                        aSupplier.Name = fields[1];
                        aSupplier.PhoneNum = fields[2];
                        aSupplier.FaxNum = fields[3];
                        aSupplier.Street = fields[4];
                        aSupplier.PostalCode = fields[5];
                        aSupplier.City = fields[6];
                        aSupplier.ProductId = Convert.ToInt32(fields[7]);
                        allSuppliers.Add(aSupplier);

                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return allSuppliers;
        }
    }
}
