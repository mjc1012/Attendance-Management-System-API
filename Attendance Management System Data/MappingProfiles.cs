
using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using AutoMapper;
using System.Globalization;

namespace Attendance_Management_System_Data
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<AttendanceLogState, AttendanceLogStateDto>().ReverseMap();
            CreateMap<AttendanceLogStatus, AttendanceLogStatusDto>().ReverseMap();
            CreateMap<AttendanceLogType, AttendanceLogTypeDto>().ReverseMap();
            CreateMap<EmployeeRole, EmployeeRoleDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                destiny => destiny.EmployeeRoleName,
                opt => opt.MapFrom(origin => origin.EmployeeRole.Name)
                )
                 .ForMember(
                destiny => destiny.ToDelete,
                opt => opt.Ignore()
                )
                 .ForMember(
                destiny => destiny.ImageFile,
                opt => opt.Ignore()
                );
            CreateMap<EmployeeDto, Employee>()
                .ForMember(
                destiny => destiny.AttendanceLogs,
                opt => opt.Ignore()
                )
                .ForMember(
                destiny => destiny.EmployeeRole,
                opt => opt.Ignore()
                );
            CreateMap<AttendanceLog, AttendanceLogDto>()
                .ForMember(
                destiny => destiny.TimeLog,
                opt => opt.MapFrom(origin => origin.TimeLog.ToString("yyyy-MM-dd HH:mm:ss"))
                )
                .ForMember(
                destiny => destiny.AttendanceLogTypeName,
                opt => opt.MapFrom(origin => origin.AttendanceLogType.Name)
                )
                 .ForMember(
                destiny => destiny.AttendanceLogStatusName,
                opt => opt.MapFrom(origin => origin.AttendanceLogStatus.Name)
                )
                  .ForMember(
                destiny => destiny.AttendanceLogStateName,
                opt => opt.MapFrom(origin => origin.AttendanceLogState.Name)
                )
                .ForMember(
                destiny => destiny.EmployeeCode,
                opt => opt.MapFrom(origin => origin.Employee.Code)
                )
                 .ForMember(
                destiny => destiny.EmployeeFirstName,
                opt => opt.MapFrom(origin => origin.Employee.FirstName)
                )
                 .ForMember(
                destiny => destiny.EmployeeMiddleName,
                opt => opt.MapFrom(origin => origin.Employee.MiddleName)
                )
                 .ForMember(
                destiny => destiny.EmployeeLastName,
                opt => opt.MapFrom(origin => origin.Employee.LastName)
                )
                  .ForMember(
                destiny => destiny.ToDelete,
                opt => opt.Ignore()
                );
            CreateMap<AttendanceLogDto, AttendanceLog>()
                .ForMember(
                destiny => destiny.TimeLog,
                opt => opt.MapFrom(origin => DateTime.ParseExact(origin.TimeLog, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
                )
                .ForMember(
                destiny => destiny.EmployeeId,
                opt => opt.Ignore()
                )
                .ForMember(
                destiny => destiny.Employee,
                opt => opt.Ignore()
                )
                .ForMember(
                destiny => destiny.AttendanceLogTypeId,
                opt => opt.Ignore()
                )
                .ForMember(
                destiny => destiny.AttendanceLogType,
                opt => opt.Ignore()
                )
                .ForMember(
                destiny => destiny.AttendanceLogStatusId,
                opt => opt.Ignore()
                )
                .ForMember(
                destiny => destiny.AttendanceLogStatus,
                opt => opt.Ignore()
                );
        }
    }
}
