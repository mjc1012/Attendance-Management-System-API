using Attendance_Management_System_Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IAttendanceLogHandler
    {
        Task<List<string>> CanAdd(AttendanceLogDto log);
        Task<List<string>> CanUpdate(AttendanceLogDto log);
        Task<List<string>> CanDelete(int id);
        Task<List<string>> CanDelete(DeleteRangeDto deleteRange);
    }
}
