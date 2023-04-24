using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Domain.Contracts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Services
{
    public class AttendanceLogStatusService : IAttendanceLogStatusService
    {

        private readonly IAttendanceLogStatusRepository _attendanceLogStatusRepository;
        private readonly IMapper _mapper;

        public AttendanceLogStatusService(IAttendanceLogStatusRepository attendanceLogStatusRepository, IMapper mapper)
        {
            _mapper = mapper;
            _attendanceLogStatusRepository = attendanceLogStatusRepository;
        }
        public async Task<AttendanceLogStatusDto> Find(int id)
        {
            try
            {
                return _mapper.Map<AttendanceLogStatusDto>(await _attendanceLogStatusRepository.Find(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceLogStatusDto> Find(string name)
        {
            try
            {
                return _mapper.Map<AttendanceLogStatusDto>(await _attendanceLogStatusRepository.Find(name));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
