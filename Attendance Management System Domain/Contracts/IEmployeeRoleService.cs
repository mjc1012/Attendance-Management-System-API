
using Attendance_Management_System_Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IEmployeeRoleService
    {
        Task<EmployeeRoleDto> Find(string name);
    }
}
