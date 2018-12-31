using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiTech.BLL
{
        public interface IUser
        {
            void SaveToFile(User anUser); //
            bool Update(User anUser);
            void Delete(int id);
            bool IsDuplicateId(int id);
            User SearchRecord(int id);
            List<User> SearchRecord(string searchName);
            List<User> ListAllRecords();
        }

        public interface IEmployee
        {
            void SaveToFile(Employee anEmployee); //
            bool Update(Employee emp);
            void Delete(int id);
            bool IsDuplicateId(int id);
            Employee SearchRecord(int id);
            List<Employee> SearchRecord(string searchName, int searchBy = 0);
            List<Employee> ListAllRecords();
        }
}
