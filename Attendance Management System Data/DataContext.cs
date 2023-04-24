﻿using Attendance_Management_System_Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;

namespace Attendance_Management_System_Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceLogType>().HasData(
                new AttendanceLogType
                {
                    Id = 1,
                    Name = "TimeIn",
                },
                new AttendanceLogType
                {
                    Id = 2,
                    Name = "TimeOut",
                });

            modelBuilder.Entity<AttendanceLogState>().HasData(
                new AttendanceLogState
                {
                    Id = 1,
                    Name = "Within Work Time",
                },
                new AttendanceLogState
                {
                    Id = 2,
                    Name = "Not Within Work Time",
                },
                 new AttendanceLogState
                 {
                     Id = 3,
                     Name = "N/A",
                 });

            modelBuilder.Entity<AttendanceLogStatus>().HasData(
               new AttendanceLogStatus
               {
                   Id = 1,
                   Name = "Present",
               },
               new AttendanceLogStatus
               {
                   Id = 2,
                   Name = "Absent",
               });

            modelBuilder.Entity<EmployeeRole>().HasData(
                new EmployeeRole
                {
                    Id = 1,
                    Name = "Admin",
                },
                new EmployeeRole
                {
                    Id = 2,
                    Name = "User",
                });

            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   Id = 1,
                   FirstName = "ADMIN",
                   MiddleName = "",
                   LastName = "ADMIN",
                   EmailAddress = "",
                   Code = "ADMIN",
                   ProfilePictureImageName = "default_image.jpg",
                   Password = "ADMIN",
                   EmployeeRoleId = 1
               });



        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeRole> EmployeeRoles { get; set; }

        public DbSet<AttendanceLog> AttendanceLogs { get; set; }

        public DbSet<AttendanceLogType> AttendanceLogTypes { get; set; }
        public DbSet<AttendanceLogStatus> AttendanceLogStatuses { get; set; }

        public DbSet<AttendanceLogState> AttendanceLogStates { get; set; }
    }
}
