using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IAttendanceLogStateService
    {
        Task<AttendanceLogStateDto> Find(int id);
        Task<AttendanceLogStateDto> Find(string name);
    }
}
