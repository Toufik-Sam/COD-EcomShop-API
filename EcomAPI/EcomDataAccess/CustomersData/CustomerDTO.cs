using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.CustomersData
{
    public class CustomerDTO
    {
        public int CustomerID { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Registered_At { get; set; }
        public CustomerDTO(int CustomerID, string FirstName, string LastName, string Email, string Phone,
            DateTime Registered_At)
        {
            this.CustomerID = CustomerID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Registered_At = Registered_At;
        }
    }
}
