using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiTech.BLL
{
    public interface IClients
    {
        void SaveToFile(Clients aClient); //
        bool Update(Clients aClient);
        void Delete(int id);
        bool IsDuplicateId(int id);
        Clients SearchRecord(int id);
        List<Clients> SearchRecord(string searchName);
        List<Clients> ListAllRecords();
    }

    public interface ISuppliers
    {
        void SaveToFile(Suppliers aSupplier); //
        bool Update(Suppliers aSupplier);
        void Delete(int id);
        bool IsDuplicateId(int id);
        Suppliers SearchRecord(int id);
        List<Suppliers> SearchRecord(string searchName);
        List<Suppliers> ListAllRecords();
    }

}
