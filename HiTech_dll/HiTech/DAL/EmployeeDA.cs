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
        public static class EmployeeDA
    {
        public enum SearchBy : int { FirstName = 0, LastName = 1, FullName = 2 };
        static string filePath = Application.StartupPath + @"\Employees.dat";
        static string filePath2 = Application.StartupPath + @"\temp.txt";
        /// <summary>
        /// This method saves the content of an object Employee into the  file
        /// Employees.dat
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static void SaveToFile(Employee employee)
            {

                //Create the object of type StreamWriter and  open the file Employees.dat
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                sw.WriteLine(employee.Employee_Id + ";" + employee.FirstName + ";" + employee.LastName + ";" + employee.PhonNumber + ";" +
                              employee.Address + ";" + employee.Zipcode + ";" + employee.Job_Title + ";" + employee.Hire_Date.ToShortDateString());
                }

            }

        /// <summary>
        /// This method searches and delete and object Employee by its Id
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
        ///  This function updates the fields of an object Employee
        /// </summary>
        /// <param name="anEmployee"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public static bool Update(Employee employee)
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
                        if (employee.Employee_Id != Convert.ToInt32(fields[0]))
                        {
                            //write to the new file
                            sw.WriteLine(line);
                        }
                        else
                        {
                            // write the updated employee to the new file
                            sw.WriteLine(employee.Employee_Id + ";" + employee.FirstName + ";" + employee.LastName + ";" + employee.PhonNumber + ";" +
                            employee.Address + ";" + employee.Zipcode + ";" + employee.Job_Title + ";" + employee.Hire_Date.ToShortDateString());
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
            /// <returns>If found returns an object Employee, return and Employee set to null if the ID is not found /returns>
            public static Employee SearchRecord(int id)
            {
                Employee anEmployee = new Employee();
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
                                // Employee found
                                anEmployee.Employee_Id = Convert.ToInt32(fields[0]);
                                anEmployee.FirstName = fields[1];
                                anEmployee.LastName = fields[2];
                                anEmployee.PhonNumber = fields[3];
                                anEmployee.Address = fields[4];
                                anEmployee.Zipcode = fields[5];
                                anEmployee.Job_Title = fields[6];
                                anEmployee.Hire_Date = Convert.ToDateTime(fields[7]);
                                return anEmployee;
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

                // Employee not found
                anEmployee = null;
                return anEmployee;
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
            /// <returns>A list of employee with the records found/returns>
            public static List<Employee> SearchRecord(string searchName, SearchBy searchBy = SearchBy.FirstName)
            {
                List<Employee> someEmployees = new List<Employee>();
                string[] fullName = new string[2];
                int FieldIdx1 = 1, FieldIdx2 = 1; // These indexes points to LastName or FirstName read from file

                fullName[0] = searchName;
                fullName[1] = searchName;

                switch (searchBy)
                {
                    case SearchBy.FirstName:
                        // Both indexes points to FirstName in the splited array from the line read from file
                        FieldIdx1 = 1;
                        FieldIdx2 = 1;
                        break;

                    case SearchBy.LastName:
                        // Indexes points to LastName in the splited array from the line read from file
                        FieldIdx1 = 2;
                        FieldIdx2 = 2;

                        break;

                    case SearchBy.FullName:
                        string Name = searchName.Replace(',', ' '); // Repalce ',' by white space, in order to split the First Namefrom LastName
                        fullName = Name.Split(' ');

                        // Indexes points to FirstName and Last name in the splited array from the line read from file
                        FieldIdx1 = 1;
                        FieldIdx2 = 2;
                        break;

                    default:
                        FieldIdx1 = 1;
                        FieldIdx2 = 1;
                        break;
                }


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

                            // This if will check for FirstName  
                            // fullName[0] and fullName[1] = FirstName against fields[FieldIdx1=1] and  fields[FieldIdx2=1]= firstName read from File 
                            // This if will check for LasstName  
                            // fullName[0] and fullName[1] = LaststName against fields[FieldIdx1=2] and  fields[FieldIdx2=2]= lastName read from File 
                            // This if will check for FullName  
                            // fullName[0] = FirstName and fullName[1] = LaststName against fields[FieldIdx1=1]=firstName  and  fields[FieldIdx2=2]= lastName read from File 

                            if ((fullName[0] == fields[FieldIdx1]) && (fullName[1] == fields[FieldIdx2]))
                            {
                                // Employee found
                                Employee anEmployee = new Employee();
                                anEmployee.Employee_Id = Convert.ToInt32(fields[0]);
                                anEmployee.FirstName = fields[1];
                                anEmployee.LastName = fields[2];
                                anEmployee.PhonNumber = fields[3];
                                anEmployee.Address = fields[4];
                                anEmployee.Zipcode = fields[5];
                                anEmployee.Job_Title = fields[6];
                                anEmployee.Hire_Date = Convert.ToDateTime(fields[7]);
                                someEmployees.Add(anEmployee);
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

                return someEmployees;
            }

            /// <summary>
            /// This method list all the records in Employees.dat
            /// </summary>
            /// <param></param>
            /// <returns>A list of all employee in the file/returns>
            public static List<Employee> ListAllRecords()
            {
                List<Employee> allEmployees = new List<Employee>();
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

                            Employee anEmployee = new Employee();
                            anEmployee.Employee_Id = Convert.ToInt32(fields[0]);
                            anEmployee.FirstName = fields[1];
                            anEmployee.LastName = fields[2];
                            anEmployee.PhonNumber = fields[3];
                            anEmployee.Address = fields[4];
                            anEmployee.Zipcode = fields[5];
                            anEmployee.Job_Title = fields[6];
                            anEmployee.Hire_Date = Convert.ToDateTime(fields[7]);
                            allEmployees.Add(anEmployee);

                            // read the next line
                            line = sr.ReadLine();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("File not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return allEmployees;
            }
        }
    }
