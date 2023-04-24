using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Management_System_Data.Repositories
{
    public class AttendanceLogStateRepository : IAttendanceLogStateRepository
    {
        private readonly DataContext _context;
        public AttendanceLogStateRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AttendanceLogState> Find(int id)
        {
            try
            {
                return await _context.AttendanceLogStates.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceLogState> Find(string name)
        {
            try
            {
                return await _context.AttendanceLogStates.AsNoTracking().Where(p => p.Name == name).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
