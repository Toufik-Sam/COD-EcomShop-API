using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.CustomersData.CustomersAddresses
{
    public class CustomerAddressDTO
    {
        public int AddressID { set; get; }
        public int CustomerID { set; get; }
        public string AddressLine1 { set; get; }
        public string AddressLine2 { set; get; }
        public string PostalCode { set; get; }
        public string Country { set; get; }
        public string City { set; get; }
        public string PhoneNumber { set; get; }
        public CustomerAddressDTO(int AddressID, int CustomerID, string AddressLine1, string AddressLine2,
            string PostalCode, string Country, string City, string PhoneNumber)
        {
            this.AddressID = AddressID;
            this.CustomerID = CustomerID;
            this.AddressLine1 = AddressLine1;
            this.AddressLine2 = AddressLine2;
            this.PostalCode = PostalCode;
            this.Country = Country;
            this.City = City;
            this.PhoneNumber = PhoneNumber;
        }
    }
}
