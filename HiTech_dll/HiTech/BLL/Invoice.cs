using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTech.DAL;

namespace HiTech.BLL
{
    public class Invoice
    {
        private int id;
        private int clientId;
        private DateTime date;
        private bool isOpen;

        public int Id { get => id; set => id = value; }
        public DateTime Date { get => date; set => date = value; }
        public int ClientId { get => clientId; set => clientId = value; }
        public bool IsOpen { get => isOpen; set => isOpen = value; }
    }
}
