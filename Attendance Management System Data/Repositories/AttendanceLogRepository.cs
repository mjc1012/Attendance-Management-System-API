using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Management_System_Data.Repositories
{
    public class AttendanceLogRepository : IAttendanceLogRepository
    {
        private readonly DataContext _context;
        public AttendanceLogRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<List<AttendanceLog>> RetrieveAll()
        {
            try
            {
                return await _context.AttendanceLogs.AsNoTracking().OrderByDescending(p => p.TimeLog).ThenBy(p => p.Employee.LastName).ThenBy(p => p.Employee.MiddleName).ThenBy(p => p.Employee.FirstName).Include(p => p.AttendanceLogType).Include(p => p.AttendanceLogStatus).Include(p => p.AttendanceLogState).Include(p => p.Employee).ToListAsync();
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task<List<AttendanceLog>> RetrieveData(List<int> ids)
        {
            try
            {
                return await _context.AttendanceLogs.AsNoTracking().Where(p => ids.Contains(p.Id)).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AttendanceLog>> RetrieveData(int employeeId)
        {
            try
            {
                return await _context.AttendanceLogs.AsNoTracking().Where(p => p.Employee.Id == employeeId).OrderByDescending(p => p.TimeLog).ThenBy(p => p.Employee.LastName).ThenBy(p => p.Employee.MiddleName).ThenBy(p => p.Employee.FirstName).Include(p => p.AttendanceLogType).Include(p => p.AttendanceLogStatus).Include(p => p.AttendanceLogState).Include(p => p.Employee).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceLog> Find(int id)
        {
            try
            {
                return await _context.AttendanceLogs.AsNoTracking().Where(p => p.Id == id).Include(p => p.AttendanceLogType).Include(p => p.AttendanceLogState).Include(p => p.AttendanceLogStatus).Include(p => p.Employee).FirstOrDefaultAsync();
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task<AttendanceLog> Create(AttendanceLog log, AttendanceLogState state, AttendanceLogStatus status, AttendanceLogType type, Employee employee, EmployeeRole role)
        {
            try
            {
                log.AttendanceLogState = state;
                log.AttendanceLogStatus = status;
                log.AttendanceLogType = type;
                log.Employee= employee;
                log.Employee.EmployeeRole= role;
                _context.AttendanceLogs.Update(log);
                await _context.SaveChangesAsync();
                return log;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task Create(AttendanceLog log)
        {
            try
            {
                _context.AttendanceLogs.Update(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(AttendanceLog log, AttendanceLogState state, AttendanceLogStatus status, AttendanceLogType type, Employee employee, EmployeeRole role)
        {
            try
            {
                log.AttendanceLogState = state;
                log.AttendanceLogStatus = status;
                log.AttendanceLogType = type;
                log.Employee= employee;
                log.Employee.EmployeeRole = role;
                _context.AttendanceLogs.Update(log);
                await _context.SaveChangesAsync();
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
                AttendanceLog log = await Find(id);
                _context.AttendanceLogs.Remove(log);
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
                List<AttendanceLog> logsDelete = await RetrieveData(ids);
                _context.AttendanceLogs.RemoveRange(logsDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Exists(AttendanceLog log, AttendanceLogStatus status, AttendanceLogType type, Employee employee)
        {
            try
            {
                return await _context.AttendanceLogs.AnyAsync(p =>  p.AttendanceLogStatus == status && p.AttendanceLogType == type && p.Employee == employee && p.Id != log.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
