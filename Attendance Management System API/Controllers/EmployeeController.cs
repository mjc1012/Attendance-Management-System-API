using Attendance_Management_System_Data;
using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using Attendance_Management_System_Domain.Contracts;
using Attendance_Management_System_Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using static Attendance_Management_System_Data.Constants;

namespace Attendance_Management_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly HttpClient _client;
        public EmployeeController(IUnitOfWork uow)
        {
            _uow = uow;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] EmployeeDto request)
        {
            try
            {
                EmployeeDto employee = await _uow.EmployeeService.FindByCode(request.Code);
                EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(employee.EmployeeRoleName);
                if (employee == null || !_uow.AuthenticationService.VerifyPassword(request, employee))
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = "User Not Found" });
                }
                string accessToken = _uow.AuthenticationService.CreateJwt(employee);
                string refreshToken = await _uow.AuthenticationService.CreateRefreshToken();
                DateTime refreshTokenExpiryTime = DateTime.Now.AddDays(5);
                await _uow.AuthenticationService.SaveTokens(employee, role, accessToken, refreshToken, refreshTokenExpiryTime);
                TokenDto token = new()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "User Found", Value = token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
        {
            try
            {
                var principal = _uow.AuthenticationService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
                EmployeeDto employee = await _uow.EmployeeService.FindByCode(principal.Identity.Name);
                EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(employee.EmployeeRoleName);
                if (employee == null || employee.RefreshToken != tokenDto.RefreshToken || employee.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = "Invalid Request" });
                }

                string newAccessToken = _uow.AuthenticationService.CreateJwt(employee);
                string newRefreshToken = await _uow.AuthenticationService.CreateRefreshToken();
                 await _uow.AuthenticationService.SaveTokens(employee, role, newAccessToken, newRefreshToken);
                TokenDto token = new()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
                
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Token refreshed", Value = token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                List<EmployeeDto> responseData = await _uow.EmployeeService.RetrieveAll();
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("date/{date}")]
        public async Task<IActionResult> RecordAbsencesOnDate(string date)
        {
            try
            {
                List<EmployeeDto> employeesTimeInAbsent = await _uow.EmployeeService.RetrieveAbsentEmployees(DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture), 1);
                List<EmployeeDto> employeesTimeOutAbsent = await  _uow.EmployeeService.RetrieveAbsentEmployees(DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture), 2);

                if (employeesTimeInAbsent.Count > 0 || employeesTimeOutAbsent.Count > 0)
                {
                    foreach (EmployeeDto employee in employeesTimeInAbsent)
                    {
                        await _uow.AttendanceLogService.Create(new AttendanceLog
                        {
                            TimeLog = DateTime.Now,
                            ImageName = "default_image.jpg",
                            EmployeeId = employee.Id,
                            AttendanceLogStateId = 3,
                            AttendanceLogStatusId = 2,
                            AttendanceLogTypeId = 1
                        });
                    }

                    foreach (EmployeeDto employee in employeesTimeOutAbsent)
                    {
                        await _uow.AttendanceLogService.Create(new AttendanceLog
                        {
                            TimeLog = DateTime.Now,
                            ImageName = "default_image.jpg",
                            EmployeeId = employee.Id,
                            AttendanceLogStateId = 3,
                            AttendanceLogStatusId = 2,
                            AttendanceLogTypeId = 2
                        });
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Successfully Added Absences" });
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = "Everyone is Persent" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{employeeCode}")]
        public async Task<IActionResult> GetEmployee(string employeeCode)
        {
            try
            {
                EmployeeDto responseData = await _uow.EmployeeService.FindByCode(employeeCode);
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpGet("employee/{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                EmployeeDto responseData = await _uow.EmployeeService.FindById(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeDto employee)
        {
            try
            {
                employee.FirstName = employee.FirstName.ToUpper();
                employee.MiddleName = (employee.MiddleName != "" && employee.MiddleName != null)? employee.MiddleName.ToUpper() : "";
                employee.LastName = employee.LastName.ToUpper();
                List<string> validationErrors = await _uow.EmployeeHandler.CanAdd(employee);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    if (employee.ImageFile == null)
                    {
                        employee.ProfilePictureImageName = "default_image.jpg";
                    }
                    else
                    {
                        employee.ProfilePictureImageName = _uow.ImageService.SaveImage(employee.ImageFile);
                    }
                    string tempPassword = _uow.AuthenticationService.CreateTemporaryPassword(employee.FirstName, employee.LastName);
                    employee.Password = _uow.AuthenticationService.HashPassword(tempPassword);
                    EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(employee.EmployeeRoleName);

                    EmployeeDto createdEmployee = await _uow.EmployeeService.Create(employee, role);

                    PersonDto person = new()
                    {
                        FirstName = createdEmployee.FirstName,
                        LastName = createdEmployee.LastName,
                        MiddleName = createdEmployee.MiddleName,
                        PairId = createdEmployee.Id
                    };

                    HttpResponseMessage getData = await _client.PostAsJsonAsync(FaceRecognitionConstants.PersonBaseUrl, person);

                    if (!getData.IsSuccessStatusCode)
                    {
                        if (createdEmployee.ProfilePictureImageName != "default_image.jpg") _uow.ImageService.DeleteImage(createdEmployee);
                        await _uow.EmployeeService.Delete(createdEmployee.Id);
                        return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = "User Not Created" });
                    }

                    _uow.EmailService.SendEmail(new EmailDto(createdEmployee.EmailAddress, "Account Credentials", EmailBody.CreateCredentialsEmailBody(createdEmployee.Code, tempPassword)));
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "User Created", Value = createdEmployee });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromForm] EmployeeDto employee)
        {
            try
            {
                employee.FirstName = employee.FirstName.ToUpper();
                employee.MiddleName = (employee.MiddleName != "" && employee.MiddleName != null) ? employee.MiddleName.ToUpper() : "";
                employee.LastName = employee.LastName.ToUpper();
                List<string> validationErrors = await _uow.EmployeeHandler.CanUpdate(employee);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    EmployeeDto oldEmployee = await _uow.EmployeeService.FindById(employee.Id);
                    employee.ProfilePictureImageName = oldEmployee.ProfilePictureImageName;
                    employee.Password = oldEmployee.Password;
                    employee.AccessToken = oldEmployee.AccessToken;
                    employee.RefreshToken = oldEmployee.RefreshToken;
                    employee.RefreshTokenExpiryTime = oldEmployee.RefreshTokenExpiryTime;
                    employee.ResetPasswordExpiry = oldEmployee.ResetPasswordExpiry;
                    employee.ResetPasswordToken = oldEmployee.ResetPasswordToken;

                    EmployeeDto tempEmployee = new()
                    {
                        Id = oldEmployee.Id,
                        FirstName = oldEmployee.FirstName,
                        MiddleName = oldEmployee.MiddleName,
                        LastName = oldEmployee.LastName,
                        EmailAddress = oldEmployee.EmailAddress,
                        Code = oldEmployee.Code,
                        EmployeeRoleName = oldEmployee.EmployeeRoleName,
                        ProfilePictureImageName= oldEmployee.ProfilePictureImageName,
                        Password = oldEmployee.Password,
                        AccessToken= oldEmployee.AccessToken,
                        RefreshToken= oldEmployee.RefreshToken,
                        RefreshTokenExpiryTime= oldEmployee.RefreshTokenExpiryTime,
                        ResetPasswordExpiry= oldEmployee.ResetPasswordExpiry,
                        ResetPasswordToken= oldEmployee.ResetPasswordToken
                    };

                    if(oldEmployee.ProfilePictureImageName != "default_image.jpg" && employee.ImageFile != null) _uow.ImageService.DeleteImage(oldEmployee);
                    if (employee.ImageFile != null) employee.ProfilePictureImageName = _uow.ImageService.SaveImage(employee.ImageFile);

                    EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(employee.EmployeeRoleName);
                    EmployeeDto updatedEmployee = await _uow.EmployeeService.Update(employee, role);

                    if (updatedEmployee.FirstName != tempEmployee.FirstName || updatedEmployee.MiddleName != tempEmployee.MiddleName || updatedEmployee.LastName != tempEmployee.LastName)
                    {

                        PersonDto person = new()
                        {
                            FirstName = updatedEmployee.FirstName,
                            LastName = updatedEmployee.LastName,
                            MiddleName = updatedEmployee.MiddleName,
                            PairId = updatedEmployee.Id
                        };

                        HttpResponseMessage getData = await _client.PutAsJsonAsync(FaceRecognitionConstants.PersonBaseUrl, person);

                        if (!getData.IsSuccessStatusCode)
                        {
                            role = await _uow.EmployeeRoleService.Find(tempEmployee.EmployeeRoleName);
                            await _uow.EmployeeService.Update(tempEmployee, role);
                            return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = "Something went wrong" });
                        }
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "User Updated", Value = updatedEmployee });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromBody] EmployeeDto employee)
        {
            try
            {

                List<string> validationErrors = await _uow.EmployeeHandler.CanUpdatePassword(employee);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    EmployeeDto updatedEmployee = await _uow.EmployeeService.FindByCode(employee.Code);
                    updatedEmployee.Password = _uow.AuthenticationService.HashPassword(employee.Password);

                    EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(updatedEmployee.EmployeeRoleName);
                    await _uow.EmployeeService.Update(updatedEmployee, role);

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Password Updated", Value = employee });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] EmployeeDto employee)
        {
            try
            {
                List<string> validationErrors = await _uow.EmployeeHandler.CanUpdateProfilePicture(employee);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    if (employee.ProfilePictureImageName != "default_image.jpg") _uow.ImageService.DeleteImage(employee);
                    employee.ProfilePictureImageName = _uow.ImageService.SaveImage(employee.ImageFile);

                    EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(employee.EmployeeRoleName);
                    await _uow.EmployeeService.Update(employee, role);

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Profile Picture Updated", Value = employee });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                List<string> validationErrors = await _uow.EmployeeHandler.CanDelete(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    EmployeeDto employee = await _uow.EmployeeService.FindById(id);

                    HttpResponseMessage getData = await _client.DeleteAsync($"{FaceRecognitionConstants.PersonBaseUrl}/{id}");

                    if (!getData.IsSuccessStatusCode)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = "Something went wrong" });
                    }

                    if (employee.ProfilePictureImageName != "default_image.jpg") _uow.ImageService.DeleteImage(employee);
                    await _uow.EmployeeService.Delete(id);

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "User Deleted" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("delete-employees")]
        public async Task<IActionResult> DeleteEmployees([FromBody] DeleteRangeDto deleteRange)
        {
            try
            {
                List<string> validationErrors = await _uow.EmployeeHandler.CanDelete(deleteRange);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    HttpResponseMessage getData = await _client.PutAsJsonAsync($"{FaceRecognitionConstants.PersonBaseUrl}/delete-people", deleteRange);

                    if (!getData.IsSuccessStatusCode)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = "Something went wrong" });
                    }

                    await _uow.EmployeeService.Delete(deleteRange);

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Users Deleted" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> ResetPasswordEmail(string email)
        {
            try
            {
                List<string> validationErrors = await _uow.EmployeeHandler.CanEmail(email);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    EmployeeDto employee = await _uow.EmployeeService.FindByEmail(email);

                    byte[] tokenBytes = RandomNumberGenerator.GetBytes(64);
                    string emailToken = Convert.ToBase64String(tokenBytes);
                    employee.ResetPasswordToken = emailToken;
                    employee.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
                    _uow.EmailService.SendEmail(new EmailDto(email, "Reset Password", EmailBody.ResetEmailBody(email, emailToken)));
                    EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(employee.EmployeeRoleName);
                    await _uow.EmployeeService.Update(employee, role);

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Reset Password Email Sent" });
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPasssword([FromBody] ResetPasswordDto resetPassword)
        {
            try
            {
                List<string> validationErrors = await _uow.EmployeeHandler.CanResetPassword(resetPassword);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    EmployeeDto employee = await _uow.EmployeeService.FindByEmail(resetPassword.Email);
                    employee.Password = _uow.AuthenticationService.HashPassword(resetPassword.NewPassword);
                    EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(employee.EmployeeRoleName);
                    await _uow.EmployeeService.Update(employee, role);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Password Reset Successful" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}
