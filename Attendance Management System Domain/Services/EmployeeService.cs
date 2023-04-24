using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using Attendance_Management_System_Domain.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        public readonly IEmployeeRepository _employeeRepository;
        public readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper) 
        { 
            _mapper= mapper;
            _employeeRepository= employeeRepository;
        }

        public async Task<List<EmployeeDto>> RetrieveAll()
        {
            try
            {
                return _mapper.Map<List<EmployeeDto>>(await _employeeRepository.RetrieveAll());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeDto>> RetrieveData(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<EmployeeDto>>(await _employeeRepository.RetrieveData(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> NameExists(EmployeeDto employee)
        {
            try
            {
                return await _employeeRepository.NameExists(_mapper.Map<Employee>(employee));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CodeExists(EmployeeDto employee)
        {
            try
            {
                return await _employeeRepository.CodeExists(_mapper.Map<Employee>(employee));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EmailAddressExists(EmployeeDto employee)
        {
            try
            {
                return await _employeeRepository.EmailAddressExists(_mapper.Map<Employee>(employee));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EmployeeDto> FindById(int id)
        {
            try
            {
                return _mapper.Map<EmployeeDto>(await _employeeRepository.FindById(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EmployeeDto> FindByName(string firstname, string middlename, string lastname)
        {
            try
            {
                return _mapper.Map<EmployeeDto>(await _employeeRepository.FindByName(firstname, middlename, lastname));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeDto>> RetrieveAbsentEmployees(DateTime date, int logTypeId)
        {
            try
            {
                return _mapper.Map<List<EmployeeDto>>(await _employeeRepository.RetrieveAbsentEmployees(date, logTypeId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EmployeeDto> FindByCode(string code)
        {
            try
            {
                return _mapper.Map<EmployeeDto>(await _employeeRepository.FindByCode(code));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EmployeeDto> FindByEmail(string email)
        {
            try
            {
                return _mapper.Map<EmployeeDto>(await _employeeRepository.FindByEmail(email));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EmployeeExists(string code, string password)
        {
            try
            {
                return await _employeeRepository.EmployeeExists(code, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RefreshTokenExists(string token)
        {
            try
            {
                return await _employeeRepository.RefreshTokenExists(token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EmployeeDto> Create(EmployeeDto employee, EmployeeRoleDto role)
        {
            try
            {
                return _mapper.Map <EmployeeDto>(await _employeeRepository.Create(_mapper.Map<Employee>(employee), _mapper.Map<EmployeeRole>(role)));
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<EmployeeDto> Update(EmployeeDto employee, EmployeeRoleDto role)
        {
            try
            {
                return _mapper.Map<EmployeeDto>(await _employeeRepository.Update(_mapper.Map<Employee>(employee), _mapper.Map<EmployeeRole>(role)));
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
                await _employeeRepository.Delete(id);
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
                await _employeeRepository.Delete(deleteRange.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
