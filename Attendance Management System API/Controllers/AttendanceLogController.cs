using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using Attendance_Management_System_Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using static Attendance_Management_System_Data.Constants;

namespace Attendance_Management_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceLogController : Controller
    {
        private readonly IUnitOfWork _uow;
        public AttendanceLogController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAttendanceLogs()
        {
            try
            {
                List<AttendanceLogDto> responseData = await _uow.AttendanceLogService.RetrieveAll();
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetAttendanceLogs(int employeeId)
        {
            try
            {
                List<AttendanceLogDto> responseData = await _uow.AttendanceLogService.RetrieveData(employeeId);
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendanceLog([FromBody] AttendanceLogDto log)
        {
            try
            {
                List<string> validationErrors = await _uow.AttendanceLogHandler.CanAdd(log);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    if (log.Base64String == "" || log.Base64String == null) log.ImageName = "default_image.jpg";
                    else log.ImageName = _uow.ImageService.SaveImage(log.Base64String);

                    EmployeeDto employee = await _uow.EmployeeService.FindByCode(log.EmployeeCode);

                    EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(employee.EmployeeRoleName);

                    AttendanceLogTypeDto type;
                    DateTime requestTimeLog = DateTime.ParseExact(log.TimeLog, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    if (log.AttendanceLogTypeName == null || log.AttendanceLogTypeName == "")
                    {
                        int logTypeId = await _uow.AttendanceLogTypeService.RetrieveId(requestTimeLog, employee, log);
                        type = await _uow.AttendanceLogTypeService.Find(logTypeId);
                    }
                    else
                    {
                        type = await _uow.AttendanceLogTypeService.Find(log.AttendanceLogTypeName);
                    }

                    AttendanceLogStatusDto status;
                    if (log.AttendanceLogStatusName == null || log.AttendanceLogStatusName == "")
                    {
                        status = await _uow.AttendanceLogStatusService.Find(1);
                    }
                    else
                    {
                        status = await _uow.AttendanceLogStatusService.Find(log.AttendanceLogStatusName);
                    }

                    AttendanceLogStateDto state;
                    if (status.Id == 1)
                    {
                        if (TimeSpan.Compare(requestTimeLog.TimeOfDay, new TimeSpan(9, 30, 0)) == 1 && TimeSpan.Compare(requestTimeLog.TimeOfDay, new TimeSpan(18, 30, 0)) == -1)
                        {
                            state = await _uow.AttendanceLogStateService.Find(1);
                        }
                        else
                        {
                            state = await _uow.AttendanceLogStateService.Find(2);
                        }
                    }
                    else
                    {
                        state = await _uow.AttendanceLogStateService.Find(3);
                    }


                    AttendanceLogDto createdLog = await _uow.AttendanceLogService.Create(log, state, status, type, employee, role);

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Attendance Log Created", Value = createdLog });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAttendanceLog([FromBody] AttendanceLogDto log)
        {
            try
            {
                List<string> validationErrors = await _uow.AttendanceLogHandler.CanUpdate(log);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    EmployeeDto employee = await _uow.EmployeeService.FindByCode(log.EmployeeCode);
                    AttendanceLogTypeDto type = await _uow.AttendanceLogTypeService.Find(log.AttendanceLogTypeName);
                    AttendanceLogStatusDto status = await _uow.AttendanceLogStatusService.Find(log.AttendanceLogStatusName);

                    AttendanceLogStateDto state;
                    DateTime requestTimeLog = DateTime.ParseExact(log.TimeLog, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    if (status.Id == 1)
                    {
                        if (TimeSpan.Compare(requestTimeLog.TimeOfDay, new TimeSpan(9, 30, 0)) == 1 && TimeSpan.Compare(requestTimeLog.TimeOfDay, new TimeSpan(18, 30, 0)) == -1)
                        {
                            state = await _uow.AttendanceLogStateService.Find(1);
                        }
                        else
                        {
                            state = await _uow.AttendanceLogStateService.Find(2);
                        }
                    }
                    else
                    {
                        state = await _uow.AttendanceLogStateService.Find(3);
                    }
                    EmployeeRoleDto role = await _uow.EmployeeRoleService.Find(employee.EmployeeRoleName);

                    await _uow.AttendanceLogService.Update(log, state, status, type, employee, role);

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Attendance Log Updated", Value = employee });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendanceLog(int id)
        {
            try
            {
                List<string> validationErrors = await _uow.AttendanceLogHandler.CanDelete(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {
                    AttendanceLogDto log = await _uow.AttendanceLogService.Find(id);
                    if (log.ImageName != "default_image.jpg") _uow.ImageService.DeleteImage(log);
                    await _uow.AttendanceLogService.Delete(id);

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Attendance Log Deleted" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("delete-logs")]
        public async Task<IActionResult> DeleteEmployees([FromBody] DeleteRangeDto deleteRange)
        {
            try
            {
                List<string> validationErrors = await _uow.AttendanceLogHandler.CanDelete(deleteRange);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                else
                {

                    await _uow.AttendanceLogService.Delete(deleteRange);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = "Attendance Logs Deleted" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}
