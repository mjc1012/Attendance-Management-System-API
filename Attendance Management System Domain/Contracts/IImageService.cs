
using Attendance_Management_System_Data.Dtos;
using Microsoft.AspNetCore.Http;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IImageService
    {
        string SaveImage(string base64String);
        string SaveImage(IFormFile imageFile);
        void DeleteImage(AttendanceLogDto attendanceLog);
        void DeleteImage(EmployeeDto employee);
    }
}
