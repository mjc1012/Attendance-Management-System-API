using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Data
{
    public class Constants
    {
        public class BaseConstants
        {
            public const string RetrievedData = "Retrieved Data";
            public const string ErrorList = "Error List";
        }

        public class PathConstants
        {
            public const string AttendancePicturesPath = "D:\\THESIS FINAL OUTPUT\\attendance backend\\Attendance Management System Server\\Attendance Management System Data\\Attendance Pictures";
            public const string ProfilePicturesPath = "D:\\THESIS FINAL OUTPUT\\attendance backend\\Attendance Management System Server\\Attendance Management System Data\\Profile Pictures";
        }

        public class FaceRecognitionConstants
        {
            public const string FaceToRecognizeBaseUrl = "https://localhost:7144/api/FaceToRecognize";
            public const string FaceToTrainBaseUrl = "https://localhost:7144/api/FaceToTrain";
            public const string PersonBaseUrl = "https://localhost:7144/api/Person";
            public const string FaceRecognitionBaseUrl = "https://localhost:7144/api/FaceRecognition";
        }

        public class EmailConstants
        {
            public const string From = "corralmarcjohn@gmail.com";
            public const string SmtpServer = "smtp.gmail.com";
            public const string Password = "zjigoixnywuljwev";
            public const int Port = 465;
            public const bool UseSsl = true;
            public const bool Quit = true;
            public const string Username = "ALLIANCE SOFTWARE INC.";
            public const string CannotReachEmail = "Cannot contact email address";
        }

        public class AttendanceLogConstants
        {
            public const string EntryInvalid = "Attendance log entry is not valid.";
            public const string SuccessAdd = "Attendance log added successfully.";
            public const string SuccessUpdate = "Attendance log updated successfully.";
            public const string SuccessDelete = "Attendance log deleted successfully.";
            public const string DoesNotExist = "Attendance log does not exist.";
            public const string Exists = "Attendance log already exists";
            public const string InvalidDate = "Date is invalid";
            public const string LogOverload = "This employee already logged two times on this date";
            public const string VeryLateTimeIn = "This employee is now absent.";
            public const string VeryEarlyTimeOut = "This employee is very early to time out";
        }

        public class AttendanceLogTypeConstants
        {
            public const string DoesNotExist = "Attendance log type does not exist.";
        }

        public class AttendanceLogStateConstants
        {
            public const string DoesNotExist = "Attendance log state does not exist.";
        }

        public class AttendanceLogStatusConstants
        {
            public const string DoesNotExist = "Attendance log status does not exist.";
        }

        public class EmployeeConstants
        {
            public const string EntryInvalid = "Employee entry is not valid.";
            public const string SuccessAdd = "Employee added successfully.";
            public const string SuccessUpdate = "Employee updated successfully.";
            public const string SuccessDelete = "Employee deleted successfully.";
            public const string DoesNotExist = "Employee does not exist.";
            public const string EmailAddressDoesNotExist = "Employee email address does not exist.";
            public const string EmployeeExists = "Employee already exists";
            public const string CodeExists = "Employee id number already exists";
            public const string EmailAddressExists = "Employee email address already exists";
            public const string FirstNameContainsDigitsOrSpecialChar = "First name contains numbers and/or special characters";
            public const string MiddleNameContainsDigitsOrSpecialChar = "Middle name contains numbers and/or special characters";
            public const string LastNameContainsDigitsOrSpecialChar = "Last name contains numbers and/or special characters";
        }

        public class PasswordConstants
        {
            public const string PasswordLengthError = "Minimum password length should be 8";
            public const string PasswordCharacterError = "Password should contain atleast one Uppercase Letter, one Lowercase Letter, one Number and one Special Character";
            public const string EntryInvalid = "Password entry is not valid.";
            public const string RefreshTokenError = "Refresh token had an error.";
        }
    }
}
