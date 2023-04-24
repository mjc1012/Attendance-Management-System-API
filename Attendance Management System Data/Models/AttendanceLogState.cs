using System.ComponentModel.DataAnnotations;

namespace Attendance_Management_System_Data.Models
{
    public class AttendanceLogState
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<AttendanceLog> AttendanceLogs { get; set; }
    }
}
