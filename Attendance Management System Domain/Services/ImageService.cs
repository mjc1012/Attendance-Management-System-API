using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Domain.Contracts;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using static Attendance_Management_System_Data.Constants;

namespace Attendance_Management_System_Domain.Services
{
    public class ImageService : IImageService
    {

        public ImageService(){}

        public string SaveImage(string base64String)
        {
            try
            {
                var path = PathConstants.AttendancePicturesPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);

                string uniqueString = Guid.NewGuid().ToString();

                var newFileName = uniqueString + ".jpg";
                var fileWithPath = Path.Combine(path, newFileName);
                image.Save(fileWithPath);
                ms.Close();
                return newFileName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string SaveImage(IFormFile imageFile)
        {
            try
            {
                var path = PathConstants.ProfilePicturesPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                var ext = Path.GetExtension(imageFile.FileName);
                
                string uniqueString = Guid.NewGuid().ToString();

                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return newFileName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteImage(AttendanceLogDto attendanceLog)
        {
            try
            {

                var path = Path.Combine(PathConstants.AttendancePicturesPath, attendanceLog.ImageName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteImage(EmployeeDto employee)
        {
            try
            {

                var path = Path.Combine(PathConstants.ProfilePicturesPath, employee.ProfilePictureImageName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
