using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> RetrieveAll();
        Task<List<EmployeeDto>> RetrieveData(List<int> ids);
        Task<bool> RefreshTokenExists(string refreshToken);
        Task<EmployeeDto> FindById(int id);
        Task<EmployeeDto> FindByName(string firstname, string middlename, string lastname);

        Task<List<EmployeeDto>> RetrieveAbsentEmployees(DateTime date, int logTypeId);

        Task<EmployeeDto> FindByCode(string code);

        Task<EmployeeDto> FindByEmail(string email);

        Task<bool> EmployeeExists(string code, string password);


        Task<EmployeeDto> Create(EmployeeDto employee, EmployeeRoleDto role);

        Task<EmployeeDto> Update(EmployeeDto employee, EmployeeRoleDto role);

        Task Delete(int id);
        Task Delete(DeleteRangeDto deleteRange);
        Task<bool> CodeExists(EmployeeDto employee);

        Task<bool> EmailAddressExists(EmployeeDto employee);
        Task<bool> NameExists(EmployeeDto employee);
    }
}
