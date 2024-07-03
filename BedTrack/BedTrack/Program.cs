using BedTrack.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using BedTrack.Configuration;
using BedTrack.Domain.Logic;
using BedTrack.DAL.Repositories;
using BedTrack.DAL.Interfaces;
using BedTrack.Domain.Interfaces;
using BedTrack.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

var defaultCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.None; // Adjust as per your environment
});

builder.Services.AddIdentityCore<User>()
.AddRoles<IdentityRole<int>>()
.AddEntityFrameworkStores<BedTrackContext>()
.AddDefaultTokenProviders()
.AddApiEndpoints();

builder.Services.AddDbContext<BedTrackContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddControllers();


//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
builder.Services.AddScoped<IClinicLogic, ClinicLogic>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentLogic, DepartmentLogic>();
builder.Services.AddScoped<IClinicDepartmentRepository, ClinicDepartmentRepository>();
builder.Services.AddScoped<IClinicDepartmentLogic, ClinicDepartmentLogic>();
builder.Services.AddScoped<IEvenetLogic, EventLogic>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserEventLogic, UserEventLogic>();
builder.Services.AddScoped<IUserEventRepository, UserEventRepository>();
builder.Services.AddScoped<IBedRepository, BedRepository>();
builder.Services.AddScoped<IBedLogic, BedLogic>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientLogic, PatientLogic>();
builder.Services.AddScoped<IUserPatientRepository, UserPatientRepository>();
builder.Services.AddScoped<IUserPatientLogic, UserPatientLogic>();
builder.Services.AddScoped<IClinicDepartmentBedRepository, ClinicDepartmentBedRepository>();
builder.Services.AddScoped<IClinicDepartmentBedLogic, ClinicDepartmentBedLogic>();


builder.Services.AddCors(p => p.AddPolicy("cors_policy_allow_all", builder =>
{
    builder.WithOrigins("https://localhost:5262").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

builder.Services.Configure<ValidationConfiguration>(builder.Configuration.GetSection("Validation"));
builder.Services.Configure<DBConfiguration>(builder.Configuration.GetSection("ConnectionStrings"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var roles = new[] { "Admin", "Clerk", "Boss", "Doctor", "Nurse" }; // Add roles here
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("cors_policy_allow_all");

app.MapIdentityApi<User>();

app.MapControllers();

app.Run();