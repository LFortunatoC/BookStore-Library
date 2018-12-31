using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.DAL;
using HiTech.Security;

namespace HiTech.BLL
{
    public class Employee: IEmployee
    {
        private int employee_Id;
        private string firstName;
        private string lastName;
        private string job_Title;
        private string address;
        private string zipcode;
        private string phonNumber;
        private DateTime hire_Date;

        public int Employee_Id { get => employee_Id; set => employee_Id = value; }
        public string Job_Title { get => job_Title; set => job_Title = value; }
        public DateTime Hire_Date { get => hire_Date; set => hire_Date = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string PhonNumber { get => phonNumber; set => phonNumber = value; }
        public string Address { get => address; set => address = value; }
        public string Zipcode { get => zipcode; set => zipcode = value; }

        /// <summary>
        /// This function check if the id is already in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if it is already in use, False otherwise</returns>
        public bool IsDuplicateId(int id)
        {
            return EmployeeDA.IsDuplicateId(id);
        }


        /// <summary>
        /// This method save an Object Employee to file 
        /// </summary>
        /// <param name="anEmployee"></param>
        public void SaveToFile(Employee anEmployee)
        {
            EmployeeDA.SaveToFile(anEmployee);
        }

        /// <summary>
        /// This method search the Object Employee by its id and its dependencies and delete them
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {

            //===================================================================================
            // First check dependencies, in this case check whether the Employee to be deleted is
            // a system user. If so delete the user than delete the employee
            //===================================================================================
            User anUser = new User();
            anUser = anUser.SearchRecord(id);  // search system User by employee ID
            if (anUser != null)
            {
                // The employee is a system user as well. So delete the user.
                Login.DeleteUser(anUser.UserName);
                anUser.Delete(Convert.ToInt32(anUser.User_id));
            }
            //=============================
            // Now delete the employee
            //=============================
            EmployeeDA.Delete(id);
        }

        /// <summary>
        /// This method updates the fields of an object Employee
        /// </summary>
        /// <param name="emp"></param>
        /// <returns>True if the update was succesfull, False otherwise</returns>
        public bool Update(Employee emp)
        {
            return EmployeeDA.Update(emp);
        }

        /// <summary>
        /// This method search an Employee by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An object employee that satisfy the search criteria</returns>
        public Employee SearchRecord(int id)
        {
            return EmployeeDA.SearchRecord(id);
        }

        /// <summary>
        /// This method returns a list with all objetcs saved in the file.
        /// if there is no objects saved the List will be empty (null)
        /// </summary>
        /// <returns>List of object Employee </returns>
        public List<Employee> ListAllRecords()
        {
            return EmployeeDA.ListAllRecords();
        }

        /// <summary>
        /// This method search objects Employee by FirstName = 0, LastName = 1, FullName = 2 
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="searchBy"></param>
        /// <returns>A list of Employee that satisfies the search requirements</returns>
        public List<Employee> SearchRecord(string searchName, int searchBy = 0) //Default search by FirstName
        {
            return EmployeeDA.SearchRecord(searchName, (EmployeeDA.SearchBy)searchBy);
        }
    }
}
