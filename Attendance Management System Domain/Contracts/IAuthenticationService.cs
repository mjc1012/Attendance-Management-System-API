using Attendance_Management_System_Data.Dtos;
using System.Security.Claims;

namespace Attendance_Management_System_Domain.Contracts
{
    public interface IAuthenticationService
    {
        bool VerifyPassword(EmployeeDto employee1, EmployeeDto employee2);
        string HashPassword(string password);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);
        string CreateTemporaryPassword(string firstname, string lastname);

        public Task<string> CreateRefreshToken();

        public Task SaveTokens(EmployeeDto employee, EmployeeRoleDto role, string accessToken, string refreshToken);

        public Task SaveTokens(EmployeeDto employee, EmployeeRoleDto role, string accessToken, string refreshToken, DateTime refreshTokenExpiryTime);

        public string CreateJwt(EmployeeDto employee);
    }
}
