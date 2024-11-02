using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.CustomersData
{
    public class FullCustomerDTO
    {
        public int CustomerID { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public DateTime Registered_At { get; set; }
        public FullCustomerDTO(int CustomerID,string FirstName, string LastName, string Email, string Phone, string PassWord,
            DateTime Registered_At)
        {
            this.CustomerID = CustomerID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Password = PassWord;
            this.Registered_At = Registered_At;
        }
    }
}
