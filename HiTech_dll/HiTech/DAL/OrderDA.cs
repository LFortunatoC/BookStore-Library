using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using HiTech.BLL;

namespace HiTech.DAL
{
    public class OrderDA
    {
        static string filePath = Application.StartupPath + @"\Orders.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";
        static string xmlFilePath= Application.StartupPath + @"\OrderId.xml";

       public enum SearchBy : int { ClientId = 0,  ProductId = 1, ClerkId = 2, InvoiceId = 3, ShippingDate = 4, RequiredDate = 5  };

        /// <summary>
        /// This method saves the content of an object Order into the  file
        /// Orders.dat
        /// </summary>
        /// <param name="anOrder"></param>
        /// <returns></returns>
        public static void SaveToFile(Order anOrder)
        {

            //Create the object of type StreamWriter and  open the file Orders.dat
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(/*anOrder.Id.ToString() +","+ */anOrder.ClientId.ToString() + "," +
                             anOrder.ProductId.ToString() + "," +
                             anOrder.Quantity.ToString() + "," + anOrder.UnitPrice.ToString() + "," +
                             anOrder.ShippingDate.ToShortDateString() + "," + anOrder.RequiredDate.ToShortDateString() +","+ 
                             anOrder.ClerkId.ToString()  + "," +
                             anOrder.InvoiceId.ToString());


            }

        }

        /// <summary>
        /// This method searches and delete and object Order by its Id
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(int id, SearchBy searchBy=SearchBy.InvoiceId )
        {
            int Idx = 7; //Delete by InvoiceID( same as OrderId)
            if (searchBy == SearchBy.ClientId) { Idx = 0; }
            if (searchBy == SearchBy.ProductId) {Idx=1;}
            if (searchBy == SearchBy.ClerkId) { Idx = 6; }
            if (searchBy == SearchBy.InvoiceId) { Idx = 7; }

            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath);
                StreamWriter sw = new StreamWriter(filePath2, true);
                // read the first line
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] fields = line.Split(',');
                    if (id != Convert.ToInt32(fields[Idx]))
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
        ///  This function updates the fields of an object Order
        /// </summary>
        /// <param name="anOrder"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(Order anOrder)
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
                    if (anOrder.Id != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // write the updated Order to the new file
                        sw.WriteLine(/*anOrder.Id.ToString() +","+ */anOrder.ClientId.ToString() + "," +
                             anOrder.ProductId.ToString() + "," +
                             anOrder.Quantity.ToString() + "," + anOrder.UnitPrice.ToString() + "," +
                             anOrder.ShippingDate.ToShortDateString() + "," + anOrder.RequiredDate.ToShortDateString() + "," +
                             anOrder.ClerkId.ToString() + "," +
                             anOrder.InvoiceId.ToString());
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
        /// This method search all the records that match the searc Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If found returns an list of  object Order, return and empty list  if the ID is not found /returns>
        public static List<Order> SearchRecord(int id, SearchBy searchBy=0)
        {
            List<Order> someOrders = new List<Order>();
            int Idx = 0;
            if (Convert.ToInt32(searchBy) ==2) { Idx = 6;} // Force Search by Invoice Number
            else if(Convert.ToInt32(searchBy) >= 3) { Idx = 7; } // Force Search by Invoice Number
            else { Idx = Convert.ToInt32(searchBy); } 

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
                            // Order found
                            Order anOrder = new Order();
                            //anOrder.Id = Convert.ToInt32(fields[0]);
                            anOrder.ClientId = Convert.ToInt32(fields[0]);
                            anOrder.ProductId = Convert.ToInt32(fields[1]);
                            anOrder.Quantity = Convert.ToInt32(fields[2]);
                            anOrder.UnitPrice = Convert.ToDecimal(fields[3]);
                            anOrder.ShippingDate = Convert.ToDateTime(fields[4]);
                            anOrder.RequiredDate = Convert.ToDateTime(fields[5]);
                            anOrder.ClerkId= Convert.ToInt32(fields[6]);
                            anOrder.InvoiceId = Convert.ToInt32(fields[7]);
                            someOrders.Add(anOrder);
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

            return someOrders;
        }

        public static List<Order> SearchRecord(DateTime date, SearchBy searchBy= SearchBy.ShippingDate)
        {
            List<Order> someOrders = new List<Order>();


            //ClientId = 0,  ProductId = 1, ClerkId = 2, InvoiceId = 3, ShippingDate = 4, RequiredDate = 5

            int Idx = 4; // Default seach  by Shipping Date
            if (Convert.ToInt32(searchBy)== 5) { Idx = 5; } 
           

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

                        if (date == Convert.ToDateTime(fields[Idx]))
                        {
                            // Order found
                            Order anOrder = new Order();
                            //anOrder.Id = Convert.ToInt32(fields[0]);
                            anOrder.ClientId = Convert.ToInt32(fields[0]);
                            anOrder.ProductId = Convert.ToInt32(fields[1]);
                            anOrder.Quantity = Convert.ToInt32(fields[2]);
                            anOrder.UnitPrice = Convert.ToDecimal(fields[3]);
                            anOrder.ShippingDate = Convert.ToDateTime(fields[4]);
                            anOrder.RequiredDate = Convert.ToDateTime(fields[5]);
                            anOrder.ClerkId = Convert.ToInt32(fields[6]);
                            anOrder.InvoiceId = Convert.ToInt32(fields[7]);
                            someOrders.Add(anOrder);
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
            return someOrders;
        }

        /// <summary>
        /// This method list all the records in Orders.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all Orders saved in the file/returns>
        public static List<Order> ListAllRecords()
        {
            List<Order> allOrders = new List<Order>();
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

                        Order anOrder = new Order();
                        //anOrder.Id = Convert.ToInt32(fields[0]);
                        anOrder.ClientId = Convert.ToInt32(fields[0]);
                        anOrder.ProductId = Convert.ToInt32(fields[1]);
                        anOrder.Quantity = Convert.ToInt32(fields[2]);
                        anOrder.UnitPrice = Convert.ToDecimal(fields[3]);
                        anOrder.ShippingDate = Convert.ToDateTime(fields[4]);
                        anOrder.RequiredDate = Convert.ToDateTime(fields[5]);
                        anOrder.ClerkId = Convert.ToInt32(fields[6]);
                        anOrder.InvoiceId = Convert.ToInt32(fields[7]);
                        allOrders.Add(anOrder);

                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return allOrders;
        }


        /// <summary>
        /// This method Get the next Order Id from an XML file
        /// IF the file doesnt exists yet the method creates it
        /// and set initial order Id to 90000
        /// </summary>
        /// <returns>int the Next Order Id</returns>
        public static int GetNextId()
        {
            if(!File.Exists(xmlFilePath)) // If the file doesn't exists yet create it 
            {
                int id = 90000; // Order Id will start from 90000
                // The file doesnt exists yet. 
                //So create the file
                // create the XmlWriterSettings object
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = (" ");
                // create the XmlWriter object
                XmlWriter xmlOut = XmlWriter.Create(xmlFilePath, settings);
                // write the start of the document
                xmlOut.WriteStartDocument();
                xmlOut.WriteStartElement("ID");
                {
                    xmlOut.WriteElementString("NextID", "900000");
                    // write the end tag for the root element
                    xmlOut.WriteEndElement();
                }
                // close the XmlWriter object
                xmlOut.Close();
                return id;
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);
                XmlNode node = doc.SelectSingleNode("ID/NextID"); // [index of user node]
                return (Convert.ToInt32(node.InnerText)+1);
            }
        }

        /// <summary>
        /// This method set the Next Order id. If there is no Id it sets the first 
        /// Order Id to 90000
        /// </summary>
        public static void SetNextId()
        {
            if (!File.Exists(xmlFilePath)) // 
            {
                GetNextId(); // Creates the file and set ID to start with 90000
            }
            else
            {
                int id = 90000;
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);
                XmlNode node = doc.SelectSingleNode("ID/NextID"); // [index of user node]
                id = (1 + (Convert.ToInt32(node.InnerText)));
                node.InnerText = id.ToString("00000");
                doc.Save(xmlFilePath);
            }
        }

    }
}
