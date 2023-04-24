using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Management_System_Data.Repositories
{
    public class EmployeeRoleRepository : IEmployeeRoleRepository
    {
        private readonly DataContext _context;
        public EmployeeRoleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<EmployeeRole> Find(string name)
        {
            try
            {
                return await _context.EmployeeRoles.AsNoTracking().Where(p => p.Name == name).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
