using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Repositories;
using Attendance_Management_System_Domain.Contracts;
using Attendance_Management_System_Domain.Handlers;
using Attendance_Management_System_Domain.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management_System_Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly IAttendanceLogRepository _attendanceLogRepository;
        private readonly IAttendanceLogStateRepository _attendanceLogStateRepository;
        private readonly IAttendanceLogStatusRepository _attendanceLogStatusRepository;
        private readonly IAttendanceLogTypeRepository _attendanceLogTypeRepository;
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        public readonly IEmployeeRepository _employeeRepository;
        public UnitOfWork(IMapper mapper, IAttendanceLogRepository attendanceLogRepository, IAttendanceLogStateRepository attendanceLogStateRepository,
            IAttendanceLogStatusRepository attendanceLogStatusRepository, IAttendanceLogTypeRepository attendanceLogTypeRepository,
            IEmployeeRoleRepository employeeRoleRepository, IEmployeeRepository employeeRepository) 
        { 
            _mapper= mapper;
            _attendanceLogRepository= attendanceLogRepository;
            _attendanceLogStateRepository= attendanceLogStateRepository;
            _attendanceLogStatusRepository= attendanceLogStatusRepository;
            _attendanceLogTypeRepository = attendanceLogTypeRepository;
            _employeeRoleRepository = employeeRoleRepository;
            _employeeRepository= employeeRepository;
        }

        public IAttendanceLogService AttendanceLogService => new AttendanceLogService(_attendanceLogRepository, _mapper);
        public IAttendanceLogStateService AttendanceLogStateService => new AttendanceLogStateService(_attendanceLogStateRepository, _mapper);
        public IAttendanceLogStatusService AttendanceLogStatusService => new AttendanceLogStatusService(_attendanceLogStatusRepository, _mapper);
        public IAttendanceLogTypeService AttendanceLogTypeService => new AttendanceLogTypeService(_attendanceLogTypeRepository, _mapper);
        public IEmployeeRoleService EmployeeRoleService => new EmployeeRoleService(_employeeRoleRepository, _mapper);
        public IEmployeeService EmployeeService => new EmployeeService(_employeeRepository, _mapper);
        public IAuthenticationService AuthenticationService => new AuthenticationService(EmployeeService);
        public IEmailService EmailService => new EmailService();
        public IImageService ImageService => new ImageService();
        public IAttendanceLogHandler AttendanceLogHandler => new AttendanceLogHandler(AttendanceLogService,AttendanceLogStatusService,AttendanceLogTypeService,EmployeeService);
        public IEmployeeHandler EmployeeHandler => new EmployeeHandler(EmployeeService, EmailService);
    }
}
