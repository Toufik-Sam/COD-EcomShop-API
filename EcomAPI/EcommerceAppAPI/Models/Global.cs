using EcomDataAccess;
namespace EcommerceAppAPI.Models
{
    public class Global:IUserService
    {
        private clsUser _user = new clsUser(-1, "", "", "", "", Permissions.Addmin, "");
        public clsUser GetUser()
        {
            return _user;
        }
        public void SetUser(clsUser user)
        {
            _user = user;
        }
    }
}
