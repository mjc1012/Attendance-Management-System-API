using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IUnitOfWork 
    {
        IAttendanceLogService AttendanceLogService { get; }
        IAttendanceLogStateService AttendanceLogStateService { get; }
        IAttendanceLogStatusService AttendanceLogStatusService { get; }
        IAttendanceLogTypeService AttendanceLogTypeService { get; }
        IAuthenticationService AuthenticationService { get; }
        IEmailService EmailService { get; }
        IEmployeeRoleService EmployeeRoleService { get; }
        IEmployeeService EmployeeService { get; }
        IImageService ImageService { get; }
        IAttendanceLogHandler AttendanceLogHandler { get; }
        IEmployeeHandler EmployeeHandler { get; }
    }
}
