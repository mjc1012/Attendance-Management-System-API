using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using Attendance_Management_System_Domain.Contracts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Attendance_Management_System_Domain.Services
{
    public class AttendanceLogTypeService : IAttendanceLogTypeService
    {

        private readonly IAttendanceLogTypeRepository _attendanceLogTypeRepository;
        private readonly IMapper _mapper;

        public AttendanceLogTypeService(IAttendanceLogTypeRepository attendanceLogTypeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _attendanceLogTypeRepository = attendanceLogTypeRepository;
        }
        public async Task<AttendanceLogTypeDto> Find(int id)
        {
            try
            {
                return _mapper.Map<AttendanceLogTypeDto>(await _attendanceLogTypeRepository.Find(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceLogTypeDto> Find(string name)
        {
            try
            {
                return _mapper.Map<AttendanceLogTypeDto>(await _attendanceLogTypeRepository.Find(name));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> RetrieveId(DateTime date, EmployeeDto employee, AttendanceLogDto log)
        {
            try
            {
                return await _attendanceLogTypeRepository.RetrieveId(date, _mapper.Map<Employee>(employee), _mapper.Map<AttendanceLog>(log));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
