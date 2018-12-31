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
   public class InvoiceDA
    {
        static string filePath = Application.StartupPath + @"\Invoices.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";

        public enum SearchBy : int { InvoiceId = 0, ClientId = 1};

        /// <summary>
        /// This method saves the content of an object Invoice into the  file
        /// Invoices.dat
        /// </summary>
        /// <param name="anInvoice"></param>
        /// <returns></returns>
        public static void SaveToFile(Invoice anInvoice)
        {

            //Create the object of type StreamWriter and  open the file Invoices.dat
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(anInvoice.Id.ToString() + "," + anInvoice.ClientId.ToString() + "," + anInvoice.Date.ToString() + "," + anInvoice.IsOpen.ToString());
            }

        }

        /// <summary>
        /// This method searches and delete and object Invoice by its Id
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
        ///  This function updates the fields of an object Invoice
        /// </summary>
        /// <param name="anInvoice"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(Invoice anInvoice)
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
                    if (anInvoice.Id != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // write the updated Invoice to the new file
                        sw.WriteLine(anInvoice.Id.ToString() + "," + anInvoice.ClientId.ToString() + "," + anInvoice.Date.ToString() + "," + anInvoice.IsOpen.ToString());
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
        /// This method search all the records that match the search Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If found returns an list of  object Invoice, return and empty list  if the ID is not found /returns>
        public static List<Invoice> SearchRecord(int id, SearchBy searchBy = 0)
        {
            List<Invoice> someInvoices = new List<Invoice>();
            int Idx = 0;
            if (Convert.ToInt32(searchBy) !=0) { Idx = 1; } // Force Search by Invoice Number

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

                        if (id == Convert.ToInt32(fields[Idx]))
                        {
                            // Invoice found
                            Invoice anInvoice = new Invoice();
                            anInvoice.Id=Convert.ToInt32(fields[0]);
                            anInvoice.ClientId=Convert.ToInt32(fields[1]);
                            anInvoice.Date=Convert.ToDateTime(fields[2]);
                            anInvoice.IsOpen=Convert.ToBoolean(fields[3]);
                            someInvoices.Add(anInvoice);
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

            return someInvoices;
        }

        public static List<Invoice> SearchRecord(DateTime date)
        {
            List<Invoice> someInvoices = new List<Invoice>();

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

                        if (date == Convert.ToDateTime(fields[2]))
                        {
                            // Invoice found
                            Invoice anInvoice = new Invoice();
                            anInvoice.Id = Convert.ToInt32(fields[0]);
                            anInvoice.ClientId = Convert.ToInt32(fields[1]);
                            anInvoice.Date = Convert.ToDateTime(fields[2]);
                            anInvoice.IsOpen = Convert.ToBoolean(fields[3]);
                            someInvoices.Add(anInvoice);
                        }
                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            return someInvoices;
        }

        public static List<Invoice> SearchRecord(bool OpenedInvoices)
        {
            List<Invoice> someInvoices = new List<Invoice>();

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

                        if (OpenedInvoices == Convert.ToBoolean(fields[3]))
                        {
                            // Invoice found
                            Invoice anInvoice = new Invoice();
                            anInvoice.Id = Convert.ToInt32(fields[0]);
                            anInvoice.ClientId = Convert.ToInt32(fields[1]);
                            anInvoice.Date = Convert.ToDateTime(fields[2]);
                            anInvoice.IsOpen = Convert.ToBoolean(fields[3]);
                            someInvoices.Add(anInvoice);
                        }
                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            return someInvoices;
        }

        /// <summary>
        /// This method list all the records in Invoices.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all Invoice in the file/returns>
        public static List<Invoice> ListAllRecords()
        {
            List<Invoice> allInvoices = new List<Invoice>();
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

                        Invoice anInvoice = new Invoice();
                        anInvoice.Id = Convert.ToInt32(fields[0]);
                        anInvoice.ClientId = Convert.ToInt32(fields[1]);
                        anInvoice.Date = Convert.ToDateTime(fields[2]);
                        anInvoice.IsOpen = Convert.ToBoolean(fields[3]);
                        allInvoices.Add(anInvoice);

                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return allInvoices;
        }
    }
}
