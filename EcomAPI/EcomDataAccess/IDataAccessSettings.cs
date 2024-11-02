using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess
{
    public interface IDataAccessSettings
    {
        string ConnectionString { get; }
    }
}
