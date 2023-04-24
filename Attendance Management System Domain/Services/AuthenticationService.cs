
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using Attendance_Management_System_Domain.Contracts;
using Attendance_Management_System_Data;
using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Contracts;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Attendance_Management_System_Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private static readonly RNGCryptoServiceProvider rng = new();
        private static readonly int saltSize = 16;
        private static readonly int hashSize = 16;
        private static readonly int iterations = 10000;
        private readonly IEmployeeService _employeeService;
        public AuthenticationService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public string CreateTemporaryPassword(string firstname, string lastname)
        {
            return firstname.Replace(" ", string.Empty) + "Alliance" + lastname.Replace(" ", string.Empty) + "@123"; 
        }

        public string HashPassword(string password)
        {
            byte[] salt;
            rng.GetBytes(salt = new byte[saltSize]);
            var key = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = key.GetBytes(hashSize);
            var hashBytes = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);
            var base64Hash = Convert.ToBase64String(hashBytes);
            return base64Hash;
        }

        public bool VerifyPassword(EmployeeDto employee1, EmployeeDto employee2)
        {
            if(employee1.Code == "ADMIN")
            {
                if (employee1.Password == employee2.Password) return true;
            }
            var hashBytes = Convert.FromBase64String(employee2.Password);
            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);
            var key = new Rfc2898DeriveBytes(employee1.Password, salt, iterations);
            byte[] hash = key.GetBytes(hashSize);

            for (int i = 0; i < hashSize; i++)
            {
                if (hashBytes[i + saltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        public string CreateJwt(EmployeeDto employee)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryveryverysecret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, employee.EmployeeRoleName),
                new Claim(ClaimTypes.Name, employee.Code)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = credentials,
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
        {
            var key = Encoding.ASCII.GetBytes("veryveryverysecret.....");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("This is an invalid token");
            }
            else
            {
                return principal;
            }
        }

        

        public async Task<string> CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes).Replace("/", "!").Replace("\\", "!");

            var tokenInUser = await _employeeService.RefreshTokenExists(refreshToken);

            if (tokenInUser)
            {
                return await CreateRefreshToken();
            }
            return refreshToken;
        }

        public async Task SaveTokens(EmployeeDto employee, EmployeeRoleDto role, string accessToken, string refreshToken)
        {
            try
            {
                employee.AccessToken = accessToken;
                employee.RefreshToken = refreshToken;
                await _employeeService.Update(employee, role);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveTokens(EmployeeDto employee, EmployeeRoleDto role, string accessToken, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            try
            {
                employee.AccessToken = accessToken;
                employee.RefreshToken = refreshToken;
                employee.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                await _employeeService.Update(employee, role);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
