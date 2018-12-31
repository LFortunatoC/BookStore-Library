using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.DAL;

namespace HiTech.BLL
{
    public class Book : Product,IBooks
    {
        private string isbn;

        public string ISBN { get => isbn; set => isbn = value; }

        /// <summary>
        /// This function check if the id is already in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if it is already in use, False otherwise</returns>
        public bool IsDuplicateId(int id)
        {
            return BookDA.IsDuplicateId(id);
        }

        /// <summary>
        /// This method returns a list with all objetcs saved in the file.
        /// if there is no objects saved the List will be empty (null)
        /// </summary>
        /// <returns>List of object Books </returns>
        public List<Book> ListAllRecords()
        {
            return BookDA.ListAllRecords();
        }

        /// <summary>
        /// This method save an Object Book to file
        /// </summary>
        /// <param name="aBook"></param>
        public void SaveToFile(Book aBook)
        {
            BookDA.SaveToFile(aBook);
        }

        /// <summary>
        /// This method updates the fields of an object Book
        /// </summary>
        /// <param name="aBook"></param>
        /// <returns>True if the object was updated; false otherwise</returns>
        public bool Update(Book aBook)
        {
            return BookDA.Update(aBook);
        }

        /// <summary>
        /// This method search the Object Book by id and delete it
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            BookDA.Delete(id);
        }

        /// <summary>
        /// This method search a book by its Id than Delete it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Book SearchRecord(int id)
        {
            return BookDA.SearchRecord(id);
        }

        /// <summary>
        /// This method search a book by its name
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns>A list of books that satisfy the search requirements</returns>
        public List<Book> SearchRecord(string searchName)
        {
            return BookDA.SearchRecord(searchName);
        }

        /// <summary>
        /// This method search a book by its Id, Supplier Id or Author Id
        /// ID = 0, Author = 1, SupllierID = 2
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchBy"></param>
        /// <returns>A list of book that satisfies the search requirement</returns>
        public List<Book> SearchRecord(int id, int searchBy=2) // Default search by Supplier Id
        {
            return BookDA.SearchRecord(id,(BookDA.SearchBy)searchBy);
        }

        /// <summary>
        /// This method search a Book by its ISBN
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns>A list of book that satisfies the search requirement</returns>
        public List<Book> SearchISBN(string isbn)
        {
            return BookDA.SearchISBN(isbn);
        }
    }
}
