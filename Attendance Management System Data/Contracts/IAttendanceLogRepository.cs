using Attendance_Management_System_Data.Models;

namespace Attendance_Management_System_Data.Contracts
{
    public interface IAttendanceLogRepository
    {
        Task<List<AttendanceLog>> RetrieveAll();
        Task<List<AttendanceLog>> RetrieveData(int employeeId);
        Task<List<AttendanceLog>> RetrieveData(List<int> ids);
        Task<AttendanceLog> Find(int id);
        Task<AttendanceLog> Create(AttendanceLog log, AttendanceLogState state, AttendanceLogStatus status, AttendanceLogType type, Employee employee, EmployeeRole role);
        Task Update(AttendanceLog log, AttendanceLogState state, AttendanceLogStatus status, AttendanceLogType type, Employee employee, EmployeeRole role);

        Task Create(AttendanceLog log);

        Task<bool> Exists(AttendanceLog log, AttendanceLogStatus status, AttendanceLogType type, Employee employee);
        Task Delete(int id);
        Task Delete(List<int> ids);


    }
}
