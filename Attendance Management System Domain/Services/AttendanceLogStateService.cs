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
    public class AttendanceLogStateService : IAttendanceLogStateService
    {

        private readonly IAttendanceLogStateRepository _attendanceLogStateRepository;
        private readonly IMapper _mapper;

        public AttendanceLogStateService(IAttendanceLogStateRepository attendanceLogStateRepository, IMapper mapper)
        {
            _mapper = mapper;
            _attendanceLogStateRepository = attendanceLogStateRepository;
        }
        public async Task<AttendanceLogStateDto> Find(int id)
        {
            try
            {
                return _mapper.Map<AttendanceLogStateDto>(await _attendanceLogStateRepository.Find(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AttendanceLogStateDto> Find(string name)
        {
            try
            {
                return _mapper.Map<AttendanceLogStateDto>(await _attendanceLogStateRepository.Find(name));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
