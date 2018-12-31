using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.DAL;

namespace HiTech.BLL
{
    public class Clients: StakeHolder, IClients
    {
        private decimal creditLimit;
        private string bankAccount;

        public decimal CreditLimit { get => creditLimit; set => creditLimit = value; }
        public string BankAccount { get => bankAccount; set => bankAccount = value; }

        /// <summary>
        /// This function check if the id is already in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if it is already in use, False otherwise</returns>
        public bool IsDuplicateId(int id)
        {
            return ClientsDA.IsDuplicateId(id);
        }

        /// <summary>
        /// This method returns a list with all objetcs saved in the file.
        /// if there is no objects saved the List will be empty (null)
        /// </summary>
        /// <returns>List of object Clients </returns>
        public List<Clients> ListAllRecords()
        {
            return ClientsDA.ListAllRecords();
        }

        /// <summary>
        /// This method save an Object Client to file
        /// </summary>
        /// <param name="aClient"></param>
        public void SaveToFile(Clients aClient)
        {
            ClientsDA.SaveToFile(aClient);
        }

        /// <summary>
        /// This method updates the fields of an object Client
        /// </summary>
        /// <param name="aClient"></param>
        /// <returns>True if the object was updated; false otherwise</returns>
        public bool Update(Clients aClient)
        {
            return ClientsDA.Update(aClient);
        }


        /// <summary>
        /// This method search the Object Clients by id and delete it
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ClientsDA.Delete(id);
        }

        /// <summary>
        /// This fmethod searc and Object Client by its add
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an Object client that satisfy the search criteria</returns>
        public Clients SearchRecord(int id)
        {
            return ClientsDA.SearchRecord(id);
        }

        /// <summary>
        /// This method search an object Client by its name
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns>A list of objects Client that satisfy the search requirements</returns>
        public List<Clients> SearchRecord(string clientName)
        {
            return ClientsDA.SearchRecord(clientName);
        }
    }
}
