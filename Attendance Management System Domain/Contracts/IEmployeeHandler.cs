using Attendance_Management_System_Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IEmployeeHandler
    {
        Task<List<string>> CanAdd(EmployeeDto employee);
        Task<List<string>> CanUpdate(EmployeeDto employee);
        Task<List<string>> CanDelete(int id);
        Task<List<string>> CanDelete(DeleteRangeDto deleteRange);
        Task<List<string>> CanUpdatePassword(EmployeeDto employee);
        Task<List<string>> CanUpdateProfilePicture(EmployeeDto employee);
        Task<List<string>> CanEmail(string email);
        Task<List<string>> CanResetPassword(ResetPasswordDto resetPassword);
    }
}
