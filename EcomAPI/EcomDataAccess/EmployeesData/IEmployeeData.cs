using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomDataAccess.EmployeesData
{
    public interface IEmployeeData
    {
        Task<int> AddEmployee(FullEmployeeDTO Employee);
        Task<bool> UpdateEmployee(EmployeeDTO employee);
        Task<EmployeeDTO> GetEmployeeByID(int EmployeeID);
        Task<EmployeeDTO> GetEmployeeByEmailAndPassword(string Email, string Password);
        Task<IList<EmployeeDTO>> GetAllEmployees();
        Task<bool> DeleteEmployee(int EmployeeID);
        Task<int> IsEmployeeExist(int EmployeeID);
        Task<bool> UpdatePassword(int EmployeeID, string CurrentPassword, string NewPassword);
    }
}
