namespace EcommerceAppAPI.Models
{
    public interface IUserService
    {
        clsUser GetUser();
        void SetUser(clsUser user);
    }
}
