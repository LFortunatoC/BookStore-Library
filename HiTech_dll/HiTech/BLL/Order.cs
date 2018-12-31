using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.DAL;

namespace HiTech.BLL
{
    public class Order
    {
        private int id;
        private DateTime shippingDate;
        private DateTime requiredDate;
        private int clientId;
        private int clerkId;
        private int productId;
        private int quantity;
        private decimal unitPrice;
        private int invoiceId;

        public int Id { get => id; set => id = value; }
        public DateTime ShippingDate { get => shippingDate; set => shippingDate = value; }
        public DateTime RequiredDate { get => requiredDate; set => requiredDate = value; }
        public int ClientId { get => clientId; set => clientId = value; }
        public int ClerkId { get => clerkId; set => clerkId = value; }
        public int ProductId { get => productId; set => productId = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public decimal UnitPrice { get => unitPrice; set => unitPrice = value; }
        public int InvoiceId { get => invoiceId; set => invoiceId = value; }

        /// <summary>
        /// Default COnstructor of Class Order
        /// </summary>
        public Order()
        {
            
        }

        
        /// <summary>
        ///  This overloaded constructor receives all the values for the Order Class fields
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="productId"></param>
        /// <param name="qtty"></param>
        /// <param name="unitPrice"></param>
        /// <param name="shippingDate"></param>
        /// <param name="requiredDate"></param>
        /// <param name="clerkId"></param>
        public Order(int clientId, int productId, int qtty, decimal unitPrice, DateTime shippingDate, DateTime requiredDate, int clerkId)
        {
            this.ClientId = clientId;
            this.ProductId = productId;
            this.Quantity = qtty;
            this.UnitPrice = unitPrice;
            this.ShippingDate = shippingDate;
            this.RequiredDate = requiredDate;
            this.ClerkId = clerkId;
        }

        /// <summary>
        /// This method save an Object Order to file
        /// </summary>
        /// <param name="anOrder"></param>
        public void SaveToFile(Order anOrder)
        {
            OrderDA.SaveToFile(anOrder);
        }

        /// <summary>
        /// This method search an Order and delete it.
        /// The search can be done by:
        /// ClientId = 0,  ProductId = 1, ClerkId = 2, InvoiceId = 3/7, ShippingDate = 4 and RequiredDate = 5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchBy"></param>
        public void Delete(int id, int searchBy=7)
        {
            OrderDA.Delete(id,(OrderDA.SearchBy)searchBy);
        }

        /// <summary>
        /// This function updates the fields of an object Order
        /// </summary>
        /// <param name="anOrder"></param>
        /// <returns>True if the update was succesfull; False otherwise</returns>
        public bool Update(Order anOrder)
        {
            return OrderDA.Update(anOrder);
        }

        /// <summary>
        /// This method search for an ID into the file
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if the ID is alread saved, False otherwise/returns>
        public bool IsDuplicateId(int id)
        {
            return OrderDA.IsDuplicateId(id);
        }

        /// <summary>
        /// This method search all the records that match the searc Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If found returns an list of  object Order, return and empty list  if the ID is not found /returns>
        public List<Order> SearchRecord(int id, int searchBy = 0)
        {
            return OrderDA.SearchRecord(id, (OrderDA.SearchBy)searchBy);
        }

        public List<Order> SearchRecord(DateTime date, int searchBy=6)
        {
            return OrderDA.SearchRecord(date, (OrderDA.SearchBy)searchBy);
        }

        /// <summary>
        /// This method list all the records in Orders.dat
        /// </summary>
        /// <param></param>
        /// <returns>A list of all Orders in the file/returns>
        public List<Order> ListAllRecords()
        {
            return OrderDA.ListAllRecords();
        }

        /// <summary>
        /// This funcion get the Next ORder ID (private key) from the XML file
        /// </summary>
        /// <returns>int NextId for an Order</returns>
        public static int GetNextId()
        {
            return OrderDA.GetNextId();
        }

        /// <summary>
        /// This method set the next Id value and save it in the XML file
        /// </summary>
        public static void SetNextId()
        {
            OrderDA.SetNextId();
        }
    }
}

