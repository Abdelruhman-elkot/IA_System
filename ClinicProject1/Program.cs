using ClinicProject1;
using ClinicProject1.Data;
using ClinicProject1.Extensions;
using ClinicProject1.MicroService;
using ClinicProject1.Repositories.Implementations;
using ClinicProject1.Repositories.Interfaces;
using ClinicProject1.Services.Implementations;
using ClinicProject1.Services.Interfaces;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    var wkhtmltopdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wkhtmltopdf");
    var libPath = Path.Combine(wkhtmltopdfPath, "wkhtmltox.dll");

    if (File.Exists(libPath))
    {
        var context = new CustomAssemblyLoadContext();
        context.LoadUnmanagedLibrary(libPath);
    }
    else
    {
        Console.WriteLine($"Warning: wkhtmltox.dll not found at {libPath}");
    }
}

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

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


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
builder.Services.AddScoped<IPdfService, PdfService>();

builder.Services.AddScoped<WebSocketService>();
builder.Services.AddScoped<WhatsAppService>();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Seed the database
    await app.SeedDatabase();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();