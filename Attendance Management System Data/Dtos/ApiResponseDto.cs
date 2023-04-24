using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Data.Dtos
{
    public class ApiResponseDto<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Value { get; set; }
    }
}
