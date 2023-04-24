using Attendance_Management_System_Data.Models;
using System;
using System.Security.Claims;

namespace Attendance_Management_System_Data.Contracts
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> RetrieveAll();
        Task<bool> RefreshTokenExists(string refreshToken);

        Task<List<Employee>> RetrieveData(List<int> ids);
        Task<Employee> FindById(int id);
        Task<Employee> FindByName(string firstname, string middlename, string lastname);

        Task<List<Employee>> RetrieveAbsentEmployees(DateTime date, int logTypeId);

        Task<Employee> FindByCode(string code);

        Task<Employee> FindByEmail(string email);

        Task<bool> EmployeeExists(string code, string password);

        Task<bool> CodeExists(Employee employee);

        Task<bool> EmailAddressExists(Employee employee);
        Task<bool> NameExists(Employee employee);

        Task<Employee> Create(Employee employee, EmployeeRole role);

        Task<Employee> Update(Employee employee, EmployeeRole role);

        Task Delete(int id);
        Task Delete(List<int> ids);

    }
}
