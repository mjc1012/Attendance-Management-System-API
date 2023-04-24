
namespace Attendance_Management_System_Data.Dtos
{
    public class AttendanceLogDto
    {
        public int Id { get; set; }

        public string TimeLog { get; set; }
        public string ImageName { get; set; }

        public string Base64String { get; set; }

        public string AttendanceLogStateName { get; set; }

        public string AttendanceLogTypeName { get; set; }

        public string AttendanceLogStatusName { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeFirstName { get; set; }

        public string EmployeeMiddleName { get; set; }

        public string EmployeeLastName { get; set; }

        public bool ToDelete { get; set; }
    }
}
