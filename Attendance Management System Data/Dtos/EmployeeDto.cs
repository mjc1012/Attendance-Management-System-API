using Microsoft.AspNetCore.Http;

namespace Attendance_Management_System_Data.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Code { get; set; }

        public string EmployeeRoleName   { get; set; }
        public string ProfilePictureImageName { get; set; }
        public string Password { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public string ResetPasswordToken { get; set; }

        public DateTime ResetPasswordExpiry { get; set; }

        public IFormFile ImageFile { get; set; }

        public bool ToDelete { get; set; }

    }
}
