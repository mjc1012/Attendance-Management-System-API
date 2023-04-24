using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Management_System_Data.Repositories
{
    public class AttendanceLogTypeRepository : IAttendanceLogTypeRepository
    {
        private readonly DataContext _context;
        public AttendanceLogTypeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AttendanceLogType> Find(string name)
        {
            try
            {
                return await _context.AttendanceLogTypes.AsNoTracking().Where(p => p.Name == name).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<AttendanceLogType> Find(int id)
        {
            try
            {
                return await _context.AttendanceLogTypes.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> RetrieveId(DateTime date, Employee employee, AttendanceLog log)
        {
            try
            {
                int count = await _context.AttendanceLogs.Where(p => p.Employee == employee && p.TimeLog.Year == date.Year && p.TimeLog.Month == date.Month && p.TimeLog.Day == date.Day && p.Id != log.Id).CountAsync();
                if (count == 1)
                {
                    return 2;
                }
                else if( count == 0)
                {
                    return 1;
                }
                return -1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
