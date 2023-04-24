using Attendance_Management_System_Data;
using Attendance_Management_System_Data.Contracts;
using Attendance_Management_System_Data.Repositories;
using Attendance_Management_System_Domain;
using Attendance_Management_System_Domain.Contracts;
using Attendance_Management_System_Domain.Handlers;
using Attendance_Management_System_Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Attendance_Management_System_Data.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); 
builder.Services.AddScoped<IAttendanceLogStateRepository, AttendanceLogStateRepository>();
builder.Services.AddScoped<IAttendanceLogStatusRepository, AttendanceLogStatusRepository>();
builder.Services.AddScoped <IAttendanceLogTypeRepository, AttendanceLogTypeRepository> ();
builder.Services.AddScoped<IAttendanceLogRepository, AttendanceLogRepository>();
builder.Services.AddScoped <IEmployeeRoleRepository, EmployeeRoleRepository> ();
builder.Services.AddScoped <IEmployeeRepository, EmployeeRepository> ();
builder.Services.AddScoped<IAttendanceLogStateService, AttendanceLogStateService>();
builder.Services.AddScoped<IAttendanceLogStatusService, AttendanceLogStatusService>();
builder.Services.AddScoped<IAttendanceLogTypeService, AttendanceLogTypeService>();
builder.Services.AddScoped<IAttendanceLogService, AttendanceLogService>();
builder.Services.AddScoped<IEmployeeRoleService, EmployeeRoleService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IEmployeeHandler, EmployeeHandler>();
builder.Services.AddScoped<IAttendanceLogHandler, AttendanceLogHandler>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Attendance Management System API"));
    
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryveryverysecret.....")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(PathConstants.AttendancePicturesPath),
    RequestPath = "/AttendancePictures"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(PathConstants.ProfilePicturesPath),
    RequestPath = "/ProfilePictures"
});

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
