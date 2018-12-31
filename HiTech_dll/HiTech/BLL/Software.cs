using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.DAL;

namespace HiTech.BLL
{
    public class Software:Product,ISoftware
    {
        private string version;

        public string Version { get => version; set => version = value; }

        /// <summary>
        /// This function check if the id is already in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if it is already in use, False otherwise</returns>
        public bool IsDuplicateId(int id)
        {
            return SoftwareDA.IsDuplicateId(id);
        }

        /// <summary>
        /// This method returns a list with all objetcs saved in the file.
        /// if there is no objects saved the List will be empty (null)
        /// </summary>
        /// <returns>List of object Software </returns>
        public List<Software> ListAllRecords()
        {
            return SoftwareDA.ListAllRecords();
        }

        /// <summary>
        /// This method save an Object Software to file
        /// </summary>
        /// <param name="aSoftware"></param>
        public void SaveToFile(Software aSoftware)
        {
            SoftwareDA.SaveToFile(aSoftware);
        }

        /// <summary>
        /// This method updates the field of an object Software 
        /// </summary>
        /// <param name="aSoftware"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public bool Update(Software aSoftware)
        {
            return SoftwareDA.Update(aSoftware);
        }

        /// <summary>
        /// This method search an object Book by its Id and delete it
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            SoftwareDA.Delete(id);
        }

        /// <summary>
        /// This method search a Software by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an object Software that satisfy the search criteria</returns>
        public Software SearchRecord(int id)
        {
            return SoftwareDA.SearchRecord(id);
        }

        /// <summary>
        /// This method search all the object Software by searchName
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns>A list with al objects Software that satisfy the search criteria</returns>
        public List<Software> SearchRecord(string searchName)
        {
            return SoftwareDA.SearchRecord(searchName);
        }

        /// <summary>
        /// This method searchs a Software by its  Id = 0, Author = 1, SupllierID = 2
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="searchBy"> Id = 0, Author = 1, SupllierID = 2</param>
        /// <returns>A list of objects Software that satisfies the search criteria</returns>
        public List<Software> SearchRecord(int Id, int searchBy=2) // Default search by Supplier Id
        {
            return SoftwareDA.SearchRecord(Id, (SoftwareDA.SearchBy)searchBy);
        }
    }
}
