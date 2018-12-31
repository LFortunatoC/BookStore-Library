using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiTech.BLL
{
    public class StakeHolder
    {
        private int id;
        private string name;
        private string street;
        private string city;
        private string postalCode;
        private string phoneNum;
        private string faxNum;
 

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Street { get => street; set => street = value; }
        public string City { get => city; set => city = value; }
        public string PostalCode { get => postalCode; set => postalCode = value; }
        public string PhoneNum { get => phoneNum; set => phoneNum = value; }
        public string FaxNum { get => faxNum; set => faxNum = value; }
    }
}
