using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IAttendanceLogService
    {

        Task<List<AttendanceLogDto>> RetrieveAll();
        Task<List<AttendanceLogDto>> RetrieveData(int employeeId);
        Task<List<AttendanceLogDto>> RetrieveData(List<int> ids);
        Task<AttendanceLogDto> Find(int id);
        Task<AttendanceLogDto> Create(AttendanceLogDto log, AttendanceLogStateDto state, AttendanceLogStatusDto status, AttendanceLogTypeDto type, EmployeeDto employee, EmployeeRoleDto role);
        Task Update(AttendanceLogDto log, AttendanceLogStateDto state, AttendanceLogStatusDto status, AttendanceLogTypeDto type, EmployeeDto employee, EmployeeRoleDto role);
        Task Create(AttendanceLog log);
        Task Delete(int id);
        Task Delete(DeleteRangeDto deleteRange);
        Task<bool> Exists(AttendanceLogDto log, AttendanceLogStatusDto status, AttendanceLogTypeDto type, EmployeeDto employee);
    }
}
