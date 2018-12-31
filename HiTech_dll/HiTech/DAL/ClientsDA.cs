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
    public class ClientsDA
    {
        static string filePath = Application.StartupPath + @"\Clients.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";

        /// <summary>
        /// This method saves the content of an object Clients into the  file
        /// Clients.dat
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static void SaveToFile(Clients client)
        {

            //Create the object of type StreamWriter and  open the file Clients.dat
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(client.Id + "," + client.Name + "," + client.PhoneNum + "," + client.FaxNum + "," +
                             client.Street + "," + client.PostalCode + "," + client.City + "," + 
                             client.CreditLimit.ToString() + "," + client.BankAccount);
            }

        }

        /// <summary>
        /// This method searches and delete and object Client by its Id
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
        ///  This function updates the fields of an object Client
        /// </summary>
        /// <param name="client"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(Clients client)
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
                    if (client.Id != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // write the updated Client to the new file
                        sw.WriteLine(client.Id + "," + client.Name + "," + client.PhoneNum + "," + client.FaxNum + "," +
                                     client.Street + "," + client.PostalCode + "," + client.City + "," +
                                     client.CreditLimit.ToString() + "," + client.BankAccount);
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
        /// <returns>If found returns an object Client, return and Client set to null if the ID is not found /returns>
        public static Clients SearchRecord(int id)
        {
            Clients aClient = new Clients();
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
                            // Client found
                            aClient.Id = Convert.ToInt32(fields[0]);
                            aClient.Name = fields[1];
                            aClient.PhoneNum = fields[2];
                            aClient.FaxNum = fields[3];
                            aClient.Street = fields[4];
                            aClient.PostalCode = fields[5];
                            aClient.City = fields[6];
                            aClient.CreditLimit = Convert.ToDecimal(fields[7]);
                            aClient.BankAccount = fields[8];
                            return aClient;
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

            // Client not found
            aClient = null;
            return aClient;
        }

        /// <summary>
        /// This method search all the records that match the searchName.
        /// According to the optional parameter serachBy(enum) the method 
        /// search for the FirstName, LastName or both
        /// The first name is set as default value for the Optional parameter.
        /// In case of the search for FullName the format accepted are:
        ///  FirstName + ',' + LastName and FirstName + ' ' + LastName like: "John,Smith" or "John Smith"
        /// </summary>
        /// <param name="searchName" and Optional [seachBy=searchBy=FirstName]></param>
        /// <returns>A list of Clients with the records found/returns>
        public static List<Clients> SearchRecord(string searchName)
        {
            List<Clients> someClients = new List<Clients>();
            
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
                                // Client found
                                Clients aClient = new Clients();
                                aClient.Id = Convert.ToInt32(fields[0]);
                                aClient.Name = fields[1];
                                aClient.PhoneNum = fields[2];
                                aClient.FaxNum = fields[3];
                                aClient.Street = fields[4];
                                aClient.PostalCode = fields[5];
                                aClient.City = fields[6];
                                aClient.CreditLimit = Convert.ToDecimal(fields[7]);
                                aClient.BankAccount = fields[8];
                                someClients.Add(aClient);
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
            return someClients;
        }

        /// <summary>
        /// This method list all the records in Clients.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all clients in the file/returns>
        public static List<Clients> ListAllRecords()
        {
            List<Clients> allClients = new List<Clients>();
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

                        Clients aClient = new Clients();
                        aClient.Id = Convert.ToInt32(fields[0]);
                        aClient.Name = fields[1];
                        aClient.PhoneNum = fields[2];
                        aClient.FaxNum = fields[3];
                        aClient.Street = fields[4];
                        aClient.PostalCode = fields[5];
                        aClient.City = fields[6];
                        aClient.CreditLimit = Convert.ToDecimal(fields[7]);
                        aClient.BankAccount = fields[8];
                        allClients.Add(aClient);

                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return allClients;
        }
    }
}
