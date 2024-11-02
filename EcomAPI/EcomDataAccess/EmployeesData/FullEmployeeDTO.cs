using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.EmployeesData
{
    public class FullEmployeeDTO
    {
        public int EmployeeID { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public DateTime Registered_At { get; set; }
        public Permissions PermissionLevel { get; set; }
        public FullEmployeeDTO(int EmployeeID, string FirstName, string LastName, string Email, string Phone, string PassWord,
            DateTime Registered_At, Permissions PermissionLevel)
        {
            this.EmployeeID = EmployeeID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Password = PassWord;
            this.Registered_At = Registered_At;
            this.PermissionLevel = PermissionLevel;
        }
    }
}
