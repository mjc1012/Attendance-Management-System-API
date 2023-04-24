using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using Attendance_Management_System_Data.Repositories;
using Attendance_Management_System_Domain.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Services
{
    public class AttendanceLogService : IAttendanceLogService
    {
        private readonly IAttendanceLogRepository _attendanceLogRepository;
        private readonly IMapper _mapper;

        public AttendanceLogService(IAttendanceLogRepository attendanceLogRepository, IMapper mapper)
        {
            _attendanceLogRepository = attendanceLogRepository;
            _mapper = mapper;
        }

        public async Task<List<AttendanceLogDto>> RetrieveAll()
        {
            try
            {
                return _mapper.Map<List<AttendanceLogDto>>(await _attendanceLogRepository.RetrieveAll());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceLogDto> Find(int id)
        {
            try
            {
                return _mapper.Map<AttendanceLogDto>(await _attendanceLogRepository.Find(id));
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<AttendanceLogDto>> RetrieveData(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<AttendanceLogDto>>(await _attendanceLogRepository.RetrieveData(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<List<AttendanceLogDto>> RetrieveData(int id)
        {
            try
            {
                return _mapper.Map<List<AttendanceLogDto>>(await _attendanceLogRepository.RetrieveData(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceLogDto> Create(AttendanceLogDto log, AttendanceLogStateDto state, AttendanceLogStatusDto status, AttendanceLogTypeDto type, EmployeeDto employee, EmployeeRoleDto role)
        {
            try
            {
                return _mapper.Map<AttendanceLogDto>(await _attendanceLogRepository.Create(_mapper.Map<AttendanceLog>(log), _mapper.Map<AttendanceLogState>(state), _mapper.Map<AttendanceLogStatus>(status), _mapper.Map<AttendanceLogType>(type), _mapper.Map<Employee>(employee), _mapper.Map<EmployeeRole>(role)));
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(AttendanceLog log)
        {
            try
            {

                await _attendanceLogRepository.Create(log);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(AttendanceLogDto log, AttendanceLogStateDto state, AttendanceLogStatusDto status, AttendanceLogTypeDto type, EmployeeDto employee, EmployeeRoleDto role)
        {
            try
            {

                await _attendanceLogRepository.Update(_mapper.Map<AttendanceLog>(log), _mapper.Map<AttendanceLogState>(state), _mapper.Map<AttendanceLogStatus>(status), _mapper.Map<AttendanceLogType>(type), _mapper.Map<Employee>(employee), _mapper.Map<EmployeeRole>(role));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Exists(AttendanceLogDto log, AttendanceLogStatusDto status, AttendanceLogTypeDto type, EmployeeDto employee)
        {
            try
            {

                return await _attendanceLogRepository.Exists(_mapper.Map<AttendanceLog>(log), _mapper.Map<AttendanceLogStatus>(status), _mapper.Map<AttendanceLogType>(type), _mapper.Map<Employee>(employee));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await _attendanceLogRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(DeleteRangeDto deleteRange)
        {
            try
            {
                await _attendanceLogRepository.Delete(deleteRange.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
