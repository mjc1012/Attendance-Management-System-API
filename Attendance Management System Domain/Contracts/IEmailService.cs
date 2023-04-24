using Attendance_Management_System_Data.Dtos;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IEmailService
    {
        void SendEmail(EmailDto email);
    }
}
