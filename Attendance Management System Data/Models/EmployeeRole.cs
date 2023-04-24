using System.ComponentModel.DataAnnotations;

namespace Attendance_Management_System_Data.Models
{
    public class EmployeeRole
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
