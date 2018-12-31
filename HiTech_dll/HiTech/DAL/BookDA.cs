using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiTech.BLL;
using System.IO;

namespace HiTech.DAL
{
    public class BookDA
    {
        static string filePath = Application.StartupPath + @"\Books.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";
        public enum SearchBy : int { ID = 0, Author = 1, SupllierID = 2 };
        /// <summary>
        /// This method saves the content of an object Book into the  file
        /// Books.dat
        /// </summary>
        /// <param name="aBook"></param>
        /// <returns></returns>
        public static void SaveToFile(Book aBook)
        {

            //Create the object of type StreamWriter and  open the file Books.dat
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine( aBook.ProductId.ToString() + ';' + aBook.Title + ';' + 
                              aBook.Year.ToString() + ';' + aBook.Category + ';' +
                              aBook.UnitPrice.ToString() + ';' + aBook.SupplierId.ToString() + ';' + 
                              aBook.ISBN + ';' + aBook.QOH.ToString());
            }

        }

        /// <summary>
        /// This method search an object Book by its Id and delete it.
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
        ///  This function updates the fields of an object Book
        /// </summary>
        /// <param name="aBook"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(Book aBook)
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
                    if (aBook.ProductId != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // write the updated Book to the new file
                        sw.WriteLine(aBook.ProductId.ToString() + ';' + aBook.Title + ';' +
                                     aBook.Year.ToString() + ';' + aBook.Category + ';' +
                                     aBook.UnitPrice.ToString() + ';' + aBook.SupplierId.ToString() + ';' +
                                     aBook.ISBN + ';' + aBook.QOH.ToString());
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
        /// This method search for an ID into the file if it is already in use return true
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
        /// <returns>If found returns an object Book, return and book set to null if the ID is not found /returns>
        public static Book SearchRecord(int id)
        {
            Book aBook = new Book();
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
                            // Book found
                            aBook.ProductId= Convert.ToInt32(fields[0]);
                            aBook.Title = fields[1];
                            aBook.Year = Convert.ToInt32(fields[2]);
                            aBook.Category = fields[3];
                            aBook.UnitPrice = Convert.ToDecimal(fields[4]);
                            aBook.SupplierId = Convert.ToInt32(fields[5]);
                            aBook.ISBN = fields[6];
                            aBook.QOH = Convert.ToInt32(fields[7]);
                            return aBook;
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

            // Book not found
            aBook = null;
            return aBook;
        }

        /// <summary>
        /// This method search all the records that match the searchName.
        /// </summary>
        /// <param name="searchName" and Optional [seachBy=searchBy=FirstName]></param>
        /// <returns>A list of Book with the records found/returns>
        public static List<Book> SearchRecord(string searchName)
        {
            List<Book> someBooks = new List<Book>();

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
                            // Book found
                            Book aBook = new Book();
                            aBook.ProductId = Convert.ToInt32(fields[0]);
                            aBook.Title = fields[1];
                            aBook.Year = Convert.ToInt32(fields[2]);
                            aBook.Category = fields[3];
                            aBook.UnitPrice = Convert.ToDecimal(fields[4]);
                            aBook.SupplierId = Convert.ToInt32(fields[5]);
                            aBook.ISBN = fields[6];
                            aBook.QOH = Convert.ToInt32(fields[7]);
                            someBooks.Add(aBook);
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

            return someBooks;
        }

        public static List<Book> SearchRecord(int Id,SearchBy searchBy=0)
        {
            List<Book> someBooks = new List<Book>();

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
                            // Book found
                            Book aBook = new Book();
                            aBook.ProductId = Convert.ToInt32(fields[0]);
                            aBook.Title = fields[1];
                            aBook.Year = Convert.ToInt32(fields[2]);
                            aBook.Category = fields[3];
                            aBook.UnitPrice = Convert.ToDecimal(fields[4]);
                            aBook.SupplierId = Convert.ToInt32(fields[5]);
                            aBook.ISBN = fields[6];
                            aBook.QOH = Convert.ToInt32(fields[7]);
                            someBooks.Add(aBook);
                        }
                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                //MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return someBooks;
        }

        /// <summary>
        /// This method list all the records in Books.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all Books in the file/returns>
        public static List<Book> ListAllRecords()
        {
            List<Book> allBooks = new List<Book>();
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

                        Book aBook = new Book();
                        aBook.ProductId = Convert.ToInt32(fields[0]);
                        aBook.Title = fields[1];
                        aBook.Year = Convert.ToInt32(fields[2]);
                        aBook.Category = fields[3];
                        aBook.UnitPrice = Convert.ToDecimal(fields[4]);
                        aBook.SupplierId = Convert.ToInt32(fields[5]);
                        aBook.ISBN = fields[6];
                        aBook.QOH = Convert.ToInt32(fields[7]);
                        allBooks.Add(aBook);

                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return allBooks;
        }

        /// <summary>
        /// This method return a list of objects Books searched by Book ISBN
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns>A List of object Book</returns>
        public static List<Book> SearchISBN(string isbn)
        {
            List<Book> someBooks = new List<Book>();

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

                        if (fields[6] == isbn)
                        {
                            // Book found
                            Book aBook = new Book();
                            aBook.ProductId = Convert.ToInt32(fields[0]);
                            aBook.Title = fields[1];
                            aBook.Year = Convert.ToInt32(fields[2]);
                            aBook.Category = fields[3];
                            aBook.UnitPrice = Convert.ToDecimal(fields[4]);
                            aBook.SupplierId = Convert.ToInt32(fields[5]);
                            aBook.ISBN = fields[6];
                            aBook.QOH = Convert.ToInt32(fields[7]);
                            someBooks.Add(aBook);
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

            return someBooks;
        }
    }
}
