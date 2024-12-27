using EcomDataAccess;

namespace EcommerceAppAPI.Models
{
    public class clsUser
    {
        public int UserID { set; get; }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public Permissions Permission { set; get; }
        public string Token { set; get; }
        public clsUser(int UserID, string LastName, string FirstName, string Email, string Phone, Permissions Permission, string Token)
        {
            this.UserID = UserID;
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.Email = Email;
            this.Phone = Phone;
            this.Permission = Permission;
            this.Token = Token;
        }
    }
}
