using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.DAL;

namespace HiTech.BLL
{
    public class Suppliers: StakeHolder, ISuppliers
    {
        private int productId;

        public int ProductId { get => productId; set => productId = value; }

        /// <summary>
        /// This function check if the id is already in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if it is already in use, False otherwise</returns>
        public bool IsDuplicateId(int id)
        {
            return SuppliersDA.IsDuplicateId(id);
        }

        /// <summary>
        /// This method returns a list with all objetcs saved in the file.
        /// if there is no objects saved the List will be empty (null)
        /// </summary>
        /// <returns>List of object Suppliers </returns>
        public List<Suppliers> ListAllRecords()
        {
            return SuppliersDA.ListAllRecords();
        }

        /// <summary>
        /// This method save an Object Supplier to file
        /// </summary>
        /// <param name="aSupplier"></param>
        public void SaveToFile(Suppliers aSupplier)
        {
           SuppliersDA.SaveToFile(aSupplier);
        }

        /// <summary>
        ///  This function updates the fields of an object Supplier
        /// </summary>
        /// <param name="aSupplier"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public bool Update(Suppliers aSupplier)
        {
            return SuppliersDA.Update(aSupplier);
        }

        /// <summary>
        ///  This method search aSupplier by its Id than delete it
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            SuppliersDA.Delete(id);
        }

        /// <summary>
        /// This method search an object Supplier by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an Object Supplier that satisfies the search criteria</returns>
        public Suppliers SearchRecord(int id)
        {
                return SuppliersDA.SearchRecord(id);
        }

        /// <summary>
        /// This method search an object Supplier by its name
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns>A list of all objects Supplier that satisfy the search criteria </returns>
        public List<Suppliers> SearchRecord(string searchName)
        {
            return SuppliersDA.SearchRecord(searchName);
        }
    }
}
