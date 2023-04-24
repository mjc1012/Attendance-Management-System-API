using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Attendance_Management_System_Data.Constants;

namespace Attendance_Management_System_Domain.Handlers
{
    public class AttendanceLogHandler : IAttendanceLogHandler
    {
        private readonly IAttendanceLogService _attendanceLogService;
        private readonly IAttendanceLogStatusService _attendanceLogStatusService;
        private readonly IAttendanceLogTypeService _attendanceLogTypeService;
        private readonly IEmployeeService _employeeService;
        public AttendanceLogHandler(IAttendanceLogService attendanceLogService, 
            IAttendanceLogStatusService attendanceLogStatusService, IAttendanceLogTypeService attendanceLogTypeService, IEmployeeService employeeService)
        {
            _attendanceLogService = attendanceLogService;
            _attendanceLogStatusService = attendanceLogStatusService;
            _attendanceLogTypeService = attendanceLogTypeService;
            _employeeService = employeeService;
        }

        public async Task<List<string>> CanAdd(AttendanceLogDto log)
        {
            var validationErrors = new List<string>();

            if (log == null)
            {
                validationErrors.Add(AttendanceLogConstants.EntryInvalid);
            }
            else
            {

                if (!DateTime.TryParseExact(log.TimeLog, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    validationErrors.Add(AttendanceLogConstants.InvalidDate);
                }

                EmployeeDto employee = await _employeeService.FindByCode(log.EmployeeCode);
                
                if (employee == null)
                {
                    validationErrors.Add(EmployeeConstants.DoesNotExist);
                }

                DateTime requestTimeLog = DateTime.ParseExact(log.TimeLog, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                AttendanceLogTypeDto type = await _attendanceLogTypeService.Find(log.AttendanceLogTypeName);
                if (log.AttendanceLogTypeName == null || log.AttendanceLogTypeName == "")
                {
                    int logTypeId = await _attendanceLogTypeService.RetrieveId(requestTimeLog, employee, log);
                    if (logTypeId == -1)
                    {
                        validationErrors.Add(AttendanceLogConstants.LogOverload);
                    }
                    else
                    {
                        type = await _attendanceLogTypeService.Find(logTypeId);
                    }
                }
                else
                {
                    type = await _attendanceLogTypeService.Find(log.AttendanceLogTypeName);
                    if (type == null)
                    {
                        validationErrors.Add(AttendanceLogTypeConstants.DoesNotExist);
                    }
                }

                if (type != null)
                {
                    if (TimeSpan.Compare(requestTimeLog.TimeOfDay, new TimeSpan(18, 30, 0)) == 1 && type.Id == 1)
                    {
                        validationErrors.Add(AttendanceLogConstants.VeryLateTimeIn);
                    }

                    if (TimeSpan.Compare(requestTimeLog.TimeOfDay, new TimeSpan(9, 30, 0)) == -1 && type.Id == 2)
                    {
                        validationErrors.Add(AttendanceLogConstants.VeryEarlyTimeOut);
                    }
                }
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(AttendanceLogDto log)
        {
            var validationErrors = new List<string>();

            if (log == null)
            {
                validationErrors.Add(AttendanceLogConstants.EntryInvalid);
            }
            else
            {
                if (_attendanceLogService.Find(log.Id) == null)
                {
                    validationErrors.Add(AttendanceLogConstants.DoesNotExist);
                }
                if (!DateTime.TryParseExact(log.TimeLog, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    validationErrors.Add(AttendanceLogConstants.InvalidDate);
                }

                AttendanceLogStatusDto status = await _attendanceLogStatusService.Find(log.AttendanceLogStatusName);
                if (status == null)
                {
                    validationErrors.Add(AttendanceLogStatusConstants.DoesNotExist);
                }

                AttendanceLogTypeDto type = await _attendanceLogTypeService.Find(log.AttendanceLogTypeName);
                if (type == null)
                {
                    validationErrors.Add(AttendanceLogTypeConstants.DoesNotExist);
                }

                EmployeeDto employee;
                if (log.EmployeeCode == null || log.EmployeeCode == "")
                {
                    employee = await _employeeService.FindByName(log.EmployeeFirstName, log.EmployeeMiddleName, log.EmployeeLastName);
                }
                else
                {
                    employee = await _employeeService.FindByCode(log.EmployeeCode);
                }

                if (employee == null)
                {
                    validationErrors.Add(EmployeeConstants.DoesNotExist);
                }

                DateTime requestTimeLog = DateTime.ParseExact(log.TimeLog, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                int logTypeId = await _attendanceLogTypeService.RetrieveId(requestTimeLog, employee, log);
                if (logTypeId == -1)
                {
                    validationErrors.Add(AttendanceLogConstants.LogOverload);
                }

                if (TimeSpan.Compare(requestTimeLog.TimeOfDay, new TimeSpan(18, 30, 0)) == 1 && type.Id == 1)
                {
                    validationErrors.Add(AttendanceLogConstants.VeryLateTimeIn);
                }

                if (TimeSpan.Compare(requestTimeLog.TimeOfDay, new TimeSpan(9, 30, 0)) == -1 && type.Id == 2)
                {
                    validationErrors.Add(AttendanceLogConstants.VeryEarlyTimeOut);
                }
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(int id)
        {
            var validationErrors = new List<string>();

            if (await _attendanceLogService.Find(id) == null)
            {
                validationErrors.Add(AttendanceLogConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(DeleteRangeDto deleteRange)
        {
            var validationErrors = new List<string>();

            if (await _attendanceLogService.RetrieveData(deleteRange.Ids) == null)
            {
                validationErrors.Add(AttendanceLogConstants.EntryInvalid);
            }

            return validationErrors;
        }
    }
}
