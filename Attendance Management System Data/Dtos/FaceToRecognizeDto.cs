namespace Attendance_Management_System_Data.Dtos
{
    public class FaceToRecognizeDto
    {
        public int Id { get; set; }
        public string ImageFile { get; set; }

        public string Base64String { get; set; }

        public string LoggedTime { get; set; }
    }
}
