using EcomDataAccess;
using EcomDataAccess.CustomersData;
using EcomDataAccess.EmployeesData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomBusinessLayer.Employees
{
    public class clsEmployee : IEmployee
    {
        private readonly IEmployeeData _employeeData;

        public clsEmployee(IEmployeeData employeeData)
        {
            this._employeeData = employeeData;
        }
        public async Task<EmployeeDTO> AddNewEmployee(FullEmployeeDTO employeeDTO)
        {
            int newID = await _employeeData.AddEmployee(employeeDTO);
            return newID > 0 ? new EmployeeDTO(newID, employeeDTO.FirstName, employeeDTO.LastName, employeeDTO.Email,
                employeeDTO.Phone, employeeDTO.Registered_At,(Permissions)employeeDTO.PermissionLevel) : null!;
        }
        public async Task<EmployeeDTO> Find(int EmployeeID)
        {
            EmployeeDTO Res = await _employeeData.GetEmployeeByID(EmployeeID);
            return Res;
        }
        public async Task<EmployeeDTO> Find(string Email, string Password)
        {
            EmployeeDTO Res = await _employeeData.GetEmployeeByEmailAndPassword(Email, Password);
            return Res;
        }
        public async Task<bool> UpdateEmployee(EmployeeDTO employeeDTO)
        {
            bool flag = await _employeeData.UpdateEmployee(employeeDTO);
            return flag;
        }
        public async Task<IList<EmployeeDTO>> GetAllEmployees()
        {
            var EmployeeList = await _employeeData.GetAllEmployees();
            return EmployeeList;
        }
        public async Task<bool> DeleteEmployee(int EmployeeID)
        {
            bool flag = await _employeeData.DeleteEmployee(EmployeeID);
            return flag;
        }
        public async Task<bool>IsEmployeeExist(int EmployeeID)
        {
            int Found = await _employeeData.IsEmployeeExist(EmployeeID);
            return Found != -1;
        }
        public async Task<bool> UpdateEmployeePassword(int EmployeeID, string CurrentPassword, string NewPassword)
        {
            bool flag = await _employeeData.UpdatePassword(EmployeeID, CurrentPassword, NewPassword);
            return flag;
        }
    }
}
