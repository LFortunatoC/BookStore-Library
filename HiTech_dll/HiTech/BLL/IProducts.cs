using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiTech.BLL
{
        public interface IBooks
        {
            void SaveToFile(Book aBook); //
            void Delete(int id);
            bool IsDuplicateId(int id);
            Book SearchRecord(int id);
            List<Book> SearchRecord(string searchName);
            List<Book> SearchRecord(int Id, int searchBy=0);
            List<Book> ListAllRecords();
        }

        public interface ISoftware
        {
            void SaveToFile(Software aSoftware); //
            void Delete(int id);
            bool IsDuplicateId(int id);
            Software SearchRecord(int id);
            List<Software> SearchRecord(string searchName);
            List<Software> SearchRecord(int Id, int searchBy = 0);
            List<Software> ListAllRecords();
        }

        public interface IAuthor
        {
            void SaveToFile(Author anAuthor); //
            void Delete(int id);
            bool IsDuplicateId(int id);
            Author SearchRecord(int id);
            List<Author> SearchRecord(string searchName);
            List<Author> ListAllRecords();
        }
        
        public interface IAuthorProduct
        {
            void SaveToFile(List<AuthorProduct> someAuthors); //
            void Delete(int id);
            bool IsDuplicateId(int id);
            List<AuthorProduct> SearchRecord(int id,int searchBy=0);
            List<AuthorProduct> ListAllRecords();
        }
}
