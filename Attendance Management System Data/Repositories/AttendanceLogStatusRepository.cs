using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Management_System_Data.Repositories
{
    public class AttendanceLogStatusRepository : IAttendanceLogStatusRepository
    {
        private readonly DataContext _context;
        public AttendanceLogStatusRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AttendanceLogStatus> Find(int id)
        {
            try
            {
                return await _context.AttendanceLogStatuses.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceLogStatus> Find(string name)
        {
            try
            {
                return await _context.AttendanceLogStatuses.AsNoTracking().Where(p => p.Name == name).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
