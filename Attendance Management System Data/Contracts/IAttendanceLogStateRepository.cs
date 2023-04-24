using Attendance_Management_System_Data.Models;

namespace Attendance_Management_System_Data.Contracts
{
    public interface IAttendanceLogStateRepository
    {

        Task<AttendanceLogState> Find(int id);
        Task<AttendanceLogState> Find(string name);
    }
}
