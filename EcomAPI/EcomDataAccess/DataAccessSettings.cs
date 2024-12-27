using Microsoft.Extensions.Configuration;
namespace EcomDataAccess
{
    public class DataAccessSettings : IDataAccessSettings
    {
        private readonly IConfiguration _config;

        public DataAccessSettings(IConfiguration config)
        {
            this._config = config;
        }
        public  string ConnectionString => _config["ConnectionStrings:Default"];
    }
}
