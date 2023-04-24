using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Models;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Attendance_Management_System_Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;
        public EmployeeRepository(DataContext context)
        { 
            _context = context;
        }

        public async Task<List<Employee>> RetrieveAll()
        {
            try
            {
                return await _context.Employees.AsNoTracking().OrderBy(p => p.LastName).ThenBy(p => p.MiddleName).ThenBy(p => p.FirstName).Include(p => p.AttendanceLogs).Include(p => p.EmployeeRole).ToListAsync();
            }
            catch(Exception )
            {
                throw ;
            }
        }

        public async Task<bool> RefreshTokenExists(string refreshToken)
        {
            try
            {
                return await _context.Employees.AnyAsync(p => p.RefreshToken == refreshToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> NameExists(Employee employee)
        {
            try
            {
                return await _context.Employees.AnyAsync(p => p.FirstName == employee.FirstName && p.MiddleName == employee.MiddleName && p.LastName == employee.MiddleName && p.Id != employee.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Employee>> RetrieveData(List<int> ids)
        {
            try
            {
                return await _context.Employees.AsNoTracking().Where(p => ids.Contains(p.Id)).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Employee>> RetrieveAbsentEmployees(DateTime date, int logTypeId)
        {
            try
            {

                return await _context.Employees.AsNoTracking().Where(x => !x.AttendanceLogs.Any(p => p.TimeLog.Year == date.Year && p.TimeLog.Month == date.Month && p.TimeLog.Day == date.Day && p.AttendanceLogTypeId == logTypeId)).Include(p => p.EmployeeRole).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Employee> FindById(int id)
        {
            try
            {
                return await _context.Employees.AsNoTracking().Where(p => p.Id == id).Include(p => p.AttendanceLogs).Include(p => p.EmployeeRole).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw ;
            }
        }

        public async Task<Employee> FindByName(string firstname, string middlename, string lastname)
        {
            try
            {
                return await _context.Employees.AsNoTracking().Where(p => p.FirstName == firstname && p.MiddleName == middlename
                && p.LastName == lastname).Include(p => p.AttendanceLogs).Include(p => p.EmployeeRole).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<Employee> FindByCode(string code)
        {
            try
            {
                return await _context.Employees.AsNoTracking().Where(p => p.Code == code).Include(p => p.AttendanceLogs).Include(p => p.EmployeeRole).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Employee> FindByEmail(string email)
        {
            try
            {
                return await _context.Employees.AsNoTracking().Where(p => p.EmailAddress == email).Include(p => p.AttendanceLogs).Include(p => p.EmployeeRole).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> EmployeeExists(string code, string password)
        {
            try
            {
                return await _context.Employees.AnyAsync(p => p.Code == code && p.Password == password);
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task<bool> CodeExists(Employee employee)
        {
            try
            {
                return await _context.Employees.AnyAsync(p => p.Code == employee.Code && p.Id != employee.Id);
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task<bool> EmailAddressExists(Employee employee)
        {
            {
                try
                {
                    return await _context.Employees.AnyAsync(p => p.EmailAddress == employee.EmailAddress && p.Id != employee.Id);
                }
                catch (Exception )
                {
                    throw ;
                }
            }
        }


        public async Task<Employee> Create(Employee employee, EmployeeRole role)
        {
            try
            {
                employee.EmployeeRole= role;
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Employee> Update(Employee employee, EmployeeRole role)
        {
            try
            {
                employee.EmployeeRole = role;
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception )
            {
                throw ;
            }
        }


        public async Task Delete(int id)
        {
            try
            {
                Employee employee = await FindById(id);
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task Delete(List<int> ids)
        {
            try
            {
                List<Employee> employees = await RetrieveData(ids);
                _context.Employees.RemoveRange(employees);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
