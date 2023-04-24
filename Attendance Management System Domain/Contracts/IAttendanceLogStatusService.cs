using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IAttendanceLogStatusService
    {
        Task<AttendanceLogStatusDto> Find(int id);
        Task<AttendanceLogStatusDto> Find(string name);
    }
}
