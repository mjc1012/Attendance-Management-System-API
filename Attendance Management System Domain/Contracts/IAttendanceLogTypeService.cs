using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IAttendanceLogTypeService
    {
        Task<AttendanceLogTypeDto> Find(int id);
        Task<AttendanceLogTypeDto> Find(string name);
        Task<int> RetrieveId(DateTime date, EmployeeDto employee, AttendanceLogDto log);
    }
}
