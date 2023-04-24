using Attendance_Management_System_Data.Models;

namespace Attendance_Management_System_Data.Contracts
{
    public interface IAttendanceLogStatusRepository
    {
        Task<AttendanceLogStatus> Find(int id);
        Task<AttendanceLogStatus> Find(string name);
    }
}
