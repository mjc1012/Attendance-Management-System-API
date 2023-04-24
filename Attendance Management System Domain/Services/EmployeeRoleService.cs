using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Repositories;
using Attendance_Management_System_Domain.Contracts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Services
{
    public class EmployeeRoleService : IEmployeeRoleService
    {

        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        private readonly IMapper _mapper;

        public EmployeeRoleService(IEmployeeRoleRepository employeeRoleRepository, IMapper mapper)
        {
            _mapper = mapper;
            _employeeRoleRepository = employeeRoleRepository;
        }

        public async Task<EmployeeRoleDto> Find(string name)
        {
            try
            {
                return _mapper.Map<EmployeeRoleDto>(await _employeeRoleRepository.Find(name));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
