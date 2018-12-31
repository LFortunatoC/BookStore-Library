using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiTech.BLL
{
    public class Product
    {
        private int productId;
        private string title;
        private int year;
        private string category;
        private decimal unitPrice;
        private int supplierId;
        private int qoh;

        public int ProductId {get => productId;set => productId = value;}
        public string Title { get => title; set => title = value; }
        public int Year { get => year; set => year = value; }
        public string Category { get => category; set => category = value; }
        public decimal UnitPrice { get => unitPrice; set => unitPrice = value; }
        public int SupplierId { get => supplierId; set => supplierId = value; }
        public int QOH { get => qoh; set => qoh = value; }

    }
}
