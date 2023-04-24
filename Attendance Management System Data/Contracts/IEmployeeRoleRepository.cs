using Attendance_Management_System_Data.Models;

namespace Attendance_Management_System_Data.Contracts
{
    public interface IEmployeeRoleRepository
    {
        Task<EmployeeRole> Find(string name);
    }
}
