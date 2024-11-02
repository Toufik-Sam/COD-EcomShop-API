using EcomDataAccess.EmployeesData;

namespace EcomBusinessLayer.Employees
{
    public interface IEmployee
    {
        Task<EmployeeDTO> AddNewEmployee(FullEmployeeDTO employeeDTO);
        Task<bool> DeleteEmployee(int EmployeeID);
        Task<EmployeeDTO> Find(int EmployeeID);
        Task<EmployeeDTO> Find(string Email, string Password);
        Task<IList<EmployeeDTO>> GetAllEmployees();
        Task<bool> UpdateEmployee(EmployeeDTO employeeDTO);
        Task<bool> IsEmployeeExist(int EmployeeID);
        Task<bool> UpdateEmployeePassword(int EmployeeID, string CurrentPassword, string NewPassword);
    }
}