using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.BLL;
using System.IO;
using System.Windows.Forms;

namespace HiTech.DAL
{
    public class AuthorDA
    {
        static string filePath = Application.StartupPath + @"\Authors.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";
        /// <summary>
        /// This method saves the content of an object user into the  file
        /// Authors.dat
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void SaveToFile(Author anAuthor)
        {

            //Create the object of type StreamWriter and  open the file Authors.dat
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(anAuthor.AuthorId + "," + anAuthor.FirstName + "," + anAuthor.LastName);
            }

        }

        /// <summary>
        /// This method search and delete and Author by its Is
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
        ///  This function updates the fields of an object Author
        /// </summary>
        /// <param name="Author"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(Author anAuthor)
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
                    if (anAuthor.AuthorId != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // write the updated User to the new file
                        sw.WriteLine(anAuthor.AuthorId + "," + anAuthor.FirstName + "," + anAuthor.LastName);
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
        /// <returns>If found returns an object anAuthor, return and object Author set to null if the ID is not found /returns>
        public static Author SearchRecord(int id)
        {
            Author anAuthor = new Author();
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
                            // Author found
                            anAuthor.AuthorId = Convert.ToInt32(fields[0]);
                            anAuthor.FirstName = fields[1];
                            anAuthor.LastName = fields[2];
                            return anAuthor;
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
            anAuthor = null;
            return anAuthor;
        }

        /// <summary>
        /// This method search all the records that match the searchName.
        /// 
        /// </summary>
        /// <param name="searchName" ></param>
        /// <returns>A list of User with the records found/returns>
        public static List<Author> SearchRecord(string searchName)
        {
            List<Author> someAuthors = new List<Author>();

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

                        if ((fields[1] == searchName) || (fields[2] == searchName))
                        {
                            // Author found
                            Author anAuthor = new Author();
                            anAuthor.AuthorId = Convert.ToInt32(fields[0]);
                            anAuthor.FirstName = fields[1];
                            anAuthor.LastName = fields[2];
                            someAuthors.Add(anAuthor);
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

            return someAuthors;
        }

        /// <summary>
        /// This method list all the records in Users.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all Authors in the file/returns>
        public static List<Author> ListAllRecords()
        {
            List<Author> allAuthors = new List<Author>();
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
                        Author anAuthor = new Author();
                        anAuthor.AuthorId = Convert.ToInt32(fields[0]);
                        anAuthor.FirstName = fields[1];
                        anAuthor.LastName = fields[2];
                        allAuthors.Add(anAuthor);

                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return allAuthors;
        }
    }
    //============================================================================================================
    //============================================================================================================
    public class AuthorProductDA
    {
        static string filePath = Application.StartupPath + @"\AuthorProduct.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";

        /// <summary>
        /// This method saves the content of an object AuthorProduct into the  file
        /// AuthorProduct.dat
        /// </summary>
        /// <param name="anAuthor"></param>
        /// <returns></returns>
        public static void SaveToFile(List<AuthorProduct> someAuthors)
        {
            //Create the object of type StreamWriter and  open the file AuthorProduct.dat
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                foreach (AuthorProduct authorProduct in someAuthors)
                {
                    sw.WriteLine(authorProduct.AuthorId + "," + authorProduct.ProductId);
                }
            }
        }

        /// <summary>
        /// This method search and delete an AuthorProduct by its Id
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
        ///  This function updates the fields of an object AuthorProduct
        /// </summary>
        /// <param name="anAuthor"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(AuthorProduct anAuthor)
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
                    if (anAuthor.AuthorId != Convert.ToInt32(fields[0]))
                    {
                        //write to the new file
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // write the updated User to the new file
                        sw.WriteLine(anAuthor.AuthorId + "," + anAuthor.ProductId);
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
        /// <returns>If found returns an object AuthorProduct, return and object AuthorProduct set to null if the ID is not found /returns>
        public static List<AuthorProduct> SearchRecord(int id,int searchBy=0)
        {
            int Idx = 0; //Default Search by Author ID
            if (searchBy > 0) { Idx = 1; } //If searchBy is different of author ID, force search by Book Id

            List<AuthorProduct> authorBooks = new List<AuthorProduct>();
            

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
                            // Author or Book Id found
                            AuthorProduct anAuthor = new AuthorProduct();
                            anAuthor.AuthorId = Convert.ToInt32(fields[0]);
                            anAuthor.ProductId = Convert.ToInt32(fields[1]);
                            authorBooks.Add(anAuthor);
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
            return authorBooks;
        }


        /// <summary>
        /// This method list all the records in Users.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all Authors in the file/returns>
        public static List<AuthorProduct> ListAllRecords()
        {
            List<AuthorProduct> allAuthors = new List<AuthorProduct>();
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
                        AuthorProduct anAuthor = new AuthorProduct();
                        anAuthor.AuthorId = Convert.ToInt32(fields[0]);
                        anAuthor.ProductId= Convert.ToInt32(fields[1]);
                        allAuthors.Add(anAuthor);

                        // read the next line
                        line = sr.ReadLine();
                    }
                }
            }
            else
            {
                MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return allAuthors;
        }
    }
}
