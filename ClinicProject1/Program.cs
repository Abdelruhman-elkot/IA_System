using ClinicProject1;
using ClinicProject1.Data;
using ClinicProject1.Extensions;
using ClinicProject1.Models.Enums;
using ClinicProject1.Repositories.Implementations;
using ClinicProject1.Repositories.Interfaces;
using ClinicProject1.Services.Implementations;
using ClinicProject1.Services.Interfaces;
using ClinicProject1.Services.MicroServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddDbContext<ClinicDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JWT>();
builder.Services.AddSingleton(jwtOptions);


builder.Services.AddAuthentication().AddJwtBearer("Bearer", options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
    };
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOnly", policy =>
//        policy.RequireRole(nameof(Role.Admin)));
//    options.AddPolicy("DoctorOnly", policy =>
//        policy.RequireRole(nameof(Role.Doctor)));
//    options.AddPolicy("PatientOnly", policy =>
//        policy.RequireRole(nameof(Role.Patient)));
//    options.AddPolicy("AdminOrPatient", policy =>
//        policy.RequireAssertion(context =>
//            context.User.IsInRole(nameof(Role.Admin)) ||
//            context.User.IsInRole(nameof(Role.Patient))));
//});

// Register repositories
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IDoctorAvailabilityRepository, DoctorAvailabilityRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();

// Register services
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDoctorAvailabilityService, DoctorAvailabilityService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();

builder.Services.AddScoped<WhatsAppService>();
builder.Services.AddScoped<WebSocketService>();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var webSocketService = scope.ServiceProvider.GetRequiredService<WebSocketService>();
    webSocketService.Start();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.SeedDatabase();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();