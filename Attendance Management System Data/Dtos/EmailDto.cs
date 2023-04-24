namespace Attendance_Management_System_Data.Dtos
{
    public class EmailDto
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailDto(string to, string subject, string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }
    }
}
