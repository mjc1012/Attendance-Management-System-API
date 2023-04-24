using Attendance_Management_System_Data.Models;

namespace Attendance_Management_System_Data.Contracts
{
    public interface IAttendanceLogTypeRepository
    {
        Task<AttendanceLogType> Find(int id);
        Task<AttendanceLogType> Find(string name);
        Task<int> RetrieveId(DateTime date, Employee employee, AttendanceLog log);
    }
}
