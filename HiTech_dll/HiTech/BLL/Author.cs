using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.DAL;

namespace HiTech.BLL
{
    public class Author:IAuthor
    {
        private int authorid;
        private string firstName;
        private string lastName;

        public int AuthorId { get => authorid; set => authorid = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }

        /// <summary>
        /// This function check if the id is already in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if it is already in use, False otherwise</returns>
        public bool IsDuplicateId(int id)
        {
            return AuthorDA.IsDuplicateId(id);
        }

        /// <summary>
        /// This method returns a list with all objetcs saved in the file.
        /// if there is no objects saved the List will be empty (null)
        /// </summary>
        /// <returns>List of object Authors </returns>
        public List<Author> ListAllRecords()
        {
            return AuthorDA.ListAllRecords();
        }

        /// <summary>
        ///  This method save an Object Author to file
        /// </summary>
        /// <param name="anAuthor"></param>
        public void SaveToFile(Author anAuthor)
        {
            AuthorDA.SaveToFile(anAuthor);
        }

        /// <summary>
        /// This method search the Object Author by id and delete it
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            AuthorDA.Delete(id);
        }

        /// <summary>
        /// This method updates the fields of an object Author
        /// </summary>
        /// <param name="anAuthor"></param>
        /// <returns>True if the Updade was succesfull; false otherwise</returns>
        public bool Update(Author anAuthor)
        {
            return AuthorDA.Update(anAuthor);
        }

        /// <summary>
        /// This method search an Author by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Author SearchRecord(int id)
        {
            return AuthorDA.SearchRecord(id);
        }

        /// <summary>
        /// This method search all the objects Author by its name.
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns>A list with all Authors that satisfy the seachName</returns>
        public List<Author> SearchRecord(string searchName)
        {
            return AuthorDA.SearchRecord(searchName);
        }
    }

    public class AuthorProduct :IAuthorProduct

    {
        private int productId;
        private int authorId;

        public int ProductId { get => productId; set => productId = value; }
        public int AuthorId { get => authorId; set => authorId = value; }

        /// <summary>
        /// This function check if the Id supplied is already used
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True id it is already in use; false otherwise </returns>
        public bool IsDuplicateId(int id)
        {
            return AuthorProductDA.IsDuplicateId(id);
        }

        /// <summary>
        /// This method returns a list with all objects AuthorProduct 
        /// that are saved in the file
        /// </summary>
        /// <returns>a List with all Author Product Objects</returns>
        public List<AuthorProduct> ListAllRecords()
        {
            return AuthorProductDA.ListAllRecords();
        }

        /// <summary>
        /// This method saves an Object AuthorProduct to a file
        /// </summary>
        /// <param name="AuthorProduct"></param>
        public void SaveToFile(List<AuthorProduct> someAuthors)
        {
            AuthorProductDA.SaveToFile(someAuthors);
        }

        /// <summary>
        /// This method sarch an object AuthorProduct by its Id and deletes it 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            AuthorProductDA.Delete(id);
        }

        /// <summary>
        /// This method search  all the objects AuthorProduct by Author Id and return a list with them.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A list with all Books of ann Author</returns>
        public List<AuthorProduct> SearchRecord(int id,int searchBy=0)
        {
            return AuthorProductDA.SearchRecord(id,searchBy);
        }

    }
}
