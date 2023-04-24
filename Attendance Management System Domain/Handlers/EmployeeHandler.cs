using Attendance_Management_System_Data;
using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using Attendance_Management_System_Domain.Contracts;
using Attendance_Management_System_Domain.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Attendance_Management_System_Data.Constants;

namespace Attendance_Management_System_Domain.Handlers
{
    public class EmployeeHandler : IEmployeeHandler
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        public EmployeeHandler(IEmployeeService employeeService, IEmailService emailService)
        {
            _employeeService = employeeService;
            _emailService = emailService;
        }

        public async Task<List<string>> CanAdd(EmployeeDto employee)
        {
            var validationErrors = new List<string>();

            if (employee == null)
            {
                validationErrors.Add(EmployeeConstants.EntryInvalid);
            }
            else
            {
                try
                {
                    _emailService.SendEmail(new EmailDto(employee.EmailAddress, "Checking Email", EmailBody.CheckEmailBody()));
                }
                catch (Exception)
                {
                    validationErrors.Add(EmailConstants.CannotReachEmail);
                }

                Match match = Regex.Match(employee.FirstName, "[^a-z]", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    validationErrors.Add(EmployeeConstants.FirstNameContainsDigitsOrSpecialChar);
                }
                match = Regex.Match(employee.MiddleName, "[^a-z]", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    validationErrors.Add(EmployeeConstants.MiddleNameContainsDigitsOrSpecialChar);
                }
                match = Regex.Match(employee.LastName, "[^a-z]", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    validationErrors.Add(EmployeeConstants.LastNameContainsDigitsOrSpecialChar);
                }
                if (await _employeeService.EmailAddressExists(employee))
                {
                    validationErrors.Add(EmployeeConstants.EmailAddressExists);
                }
                if(await _employeeService.CodeExists(employee))
                {
                    validationErrors.Add(EmployeeConstants.CodeExists);
                }
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(EmployeeDto employee)
        {
            var validationErrors = new List<string>();

            if (employee == null)
            {
                validationErrors.Add(EmployeeConstants.EntryInvalid);
            }
            else
            {
                try
                {
                    _emailService.SendEmail(new EmailDto(employee.EmailAddress, "Checking Email", EmailBody.CheckEmailBody()));
                }
                catch (Exception)
                {
                    validationErrors.Add(EmailConstants.CannotReachEmail);
                }

                Match match = Regex.Match(employee.FirstName, "[^a-z]", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    validationErrors.Add(EmployeeConstants.FirstNameContainsDigitsOrSpecialChar);
                }
                match = Regex.Match(employee.MiddleName, "[^a-z]", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    validationErrors.Add(EmployeeConstants.MiddleNameContainsDigitsOrSpecialChar);
                }
                match = Regex.Match(employee.LastName, "[^a-z]", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    validationErrors.Add(EmployeeConstants.LastNameContainsDigitsOrSpecialChar);
                }
                if (await _employeeService.EmailAddressExists(employee))
                {
                    validationErrors.Add(EmployeeConstants.EmailAddressExists);
                }
                if (await _employeeService.CodeExists(employee))
                {
                    validationErrors.Add(EmployeeConstants.CodeExists);
                }
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(int id)
        {
            var validationErrors = new List<string>();

            if (await _employeeService.FindById(id) == null)
            {
                validationErrors.Add(EmployeeConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(DeleteRangeDto deleteRange)
        {
            var validationErrors = new List<string>();

            if (await _employeeService.RetrieveData(deleteRange.Ids) == null)
            {
                validationErrors.Add(EmployeeConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdatePassword(EmployeeDto employee)
        {
            var validationErrors = new List<string>();

            if (employee == null)
            {
                validationErrors.Add(PasswordConstants.EntryInvalid);
            }
            else
            {
                if (employee.Password.Length < 8)
                {
                    validationErrors.Add(PasswordConstants.PasswordLengthError);
                }
                if (!(Regex.IsMatch(employee.Password, "[~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]") && Regex.IsMatch(employee.Password, "[a-z]") && Regex.IsMatch(employee.Password, "[A-Z]") && Regex.IsMatch(employee.Password, "[0-9]")))
                {
                    validationErrors.Add(PasswordConstants.PasswordCharacterError);
                }
                if (await _employeeService.FindByCode(employee.Code) == null)
                {
                    validationErrors.Add(EmployeeConstants.DoesNotExist);   
                }
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdateProfilePicture(EmployeeDto employee)
        {
            var validationErrors = new List<string>();

            if (employee == null)
            {
                validationErrors.Add(EmployeeConstants.EntryInvalid);
            }
            else
            {
                if (await _employeeService.FindByCode(employee.Code) == null)
                {
                    validationErrors.Add(EmployeeConstants.DoesNotExist);
                }
            }

            return validationErrors;
        }

        public async Task<List<string>> CanEmail(string email)
        {
            var validationErrors = new List<string>();

            if (await _employeeService.FindByEmail(email) == null)
            {
                validationErrors.Add(EmployeeConstants.EmailAddressDoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanResetPassword(ResetPasswordDto resetPassword)
        {
            var validationErrors = new List<string>();

            if (resetPassword == null)
            {
                validationErrors.Add(PasswordConstants.EntryInvalid);
            }
            else
            {
                if (resetPassword.NewPassword.Length < 8)
                {
                    validationErrors.Add(PasswordConstants.PasswordLengthError);
                }
                if (!(Regex.IsMatch(resetPassword.NewPassword, "[~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]") && Regex.IsMatch(resetPassword.NewPassword, "[a-z]") && Regex.IsMatch(resetPassword.NewPassword, "[A-Z]") && Regex.IsMatch(resetPassword.NewPassword, "[0-9]")))
                {
                    validationErrors.Add(PasswordConstants.PasswordCharacterError);
                }
                string newToken = resetPassword.EmailToken.Replace(" ", "+");

                EmployeeDto employee = await _employeeService.FindByEmail(resetPassword.Email);
                var tokenCode = employee.ResetPasswordToken;
                var emailTokenExpiry = employee.ResetPasswordExpiry;

                if (employee == null)
                {
                    validationErrors.Add(EmployeeConstants.EmailAddressDoesNotExist);
                }

                if (employee == null || tokenCode != newToken || emailTokenExpiry < DateTime.Now)
                {
                    validationErrors.Add(PasswordConstants.RefreshTokenError);
                }
            }

            return validationErrors;
        }
    }
}
