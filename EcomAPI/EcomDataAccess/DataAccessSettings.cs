using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
