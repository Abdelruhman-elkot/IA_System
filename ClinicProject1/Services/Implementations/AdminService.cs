using ClinicProject1.DTOs.Doctor_DTOs;
using ClinicProject1.Models.Entities;
using ClinicProject1.Services.Interfaces;
using AutoMapper;
using ClinicProject1.Repositories.Interfaces;
using ClinicProject1.DTOs.DoctorDTOs;
using ClinicProject1.Data.Enums;
using ClinicProject1.DTOs.ReportsDTOs;
using System.Text;

namespace ClinicProject1.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IPdfService _pdfService;

        public AdminService(IAdminRepository adminRepository, IMapper mapper, IPdfService pdfService)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _pdfService = pdfService;
        }

        #region Doctor Management
        public async Task<IEnumerable<DoctorDashboardDto>> GetAllDoctors()
        {
            var doctors = await _adminRepository.GetAllDoctors();
            return _mapper.Map<IEnumerable<DoctorDashboardDto>>(doctors);
        }

        public async Task<DoctorDashboardDto> GetDoctorById(int doctorId)
        {
            var doctor = await _adminRepository.GetDoctorById(doctorId);
            if (doctor == null) return null;

        var dto = _mapper.Map<DoctorDashboardDto>(doctor);
        
                if (doctor.Availabilities.Any())
        {
            var availability = doctor.Availabilities.First();
            dto.AvailableDays = new List<string> 
            { 
                availability.Day1.ToString(),
                availability.Day2.ToString()
            };
            dto.Availability = $"{availability.StartTime} - {availability.EndTime}";
        }

        return dto;        }

        public async Task<DoctorDashboardDto> CreateDoctor(CreateDoctorDto doctorDto)
        {
            var baseUsername = $"{doctorDto.FirstName}{doctorDto.LastName}";
            var username = baseUsername;

            var user = new User
            {
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                Username = username,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                PhoneNumber = doctorDto.PhoneNumber,
                Role = Role.Doctor
            };

            var doctor = new Doctor
            {
                Specialization = doctorDto.Specialization,
                User = user
            };

            var createdDoctor = await _adminRepository.CreateDoctor(doctor);
            return _mapper.Map<DoctorDashboardDto>(createdDoctor);
        }

        public async Task<bool> UpdateDoctor(int doctorId, UpdateDoctorDto doctorDto)
        {
            var doctor = await _adminRepository.GetDoctorById(doctorId);
            if (doctor == null) return false;

            doctor.User.PhoneNumber = doctorDto.PhoneNumber;

            return await _adminRepository.UpdateDoctor(doctor);
        }

        public async Task<bool> DeleteDoctor(int doctorId)
        {
            return await _adminRepository.DeleteDoctor(doctorId);
        }
        #endregion

        public async Task<IEnumerable<DoctorScheduleReportDto>> GenerateDoctorSchedulesReport()
        {
            var Reports = await _adminRepository.GetDoctorSchedulesReport();
            return _mapper.Map<IEnumerable<DoctorScheduleReportDto>>(Reports);
        }

        public async Task<IEnumerable<PatientVisitReportDto>> GeneratePatientVisitsReport()
        {
            var Reports = await _adminRepository.GetPatientVisitsReport();
            return _mapper.Map<IEnumerable<PatientVisitReportDto>>(Reports);
        }

        public async Task<byte[]> GenerateDoctorSchedulesPdfReport()
        {
            var data = await GenerateDoctorSchedulesReport();
            //var dataList = data.ToList();
            var html = GenerateDoctorSchedulesHtml(data);
            return _pdfService.GeneratePdf(html);
        }

        public async Task<byte[]> GeneratePatientVisitsPdfReport()
        {
            var data =await GeneratePatientVisitsReport();
            var html = GeneratePatientVisitsHtml(data);
            return _pdfService.GeneratePdf(html);
        }

        private string GenerateDoctorSchedulesHtml(IEnumerable<DoctorScheduleReportDto> data)
        {
            var sb = new StringBuilder();
            sb.Append("<h1>Doctor Schedules Report</h1>");
            sb.Append("<table border='1' cellpadding='5' cellspacing='0' width='100%'>");
            sb.Append("<tr><th>Doctor Name</th><th>Specialization</th><th>Available Days</th><th>Working Hours</th><th>Total Appointments</th></tr>");

            foreach (var doctor in data ?? Enumerable.Empty<DoctorScheduleReportDto>())
            {
                sb.Append($"<tr><td>{doctor.DoctorName ?? "N/A"}</td>" +
                         $"<td>{doctor.Specialization ?? "N/A"}</td>" +
                         $"<td>{(doctor.AvailableDays != null ? string.Join(", ", doctor.AvailableDays) : "N/A")}</td>" +
                         $"<td>{doctor.WorkingHours ?? "N/A"}</td>" +
                         $"<td>{doctor.TotalAppointments}</td></tr>");
            }

            sb.Append("</table>");
            return sb.ToString();
        }

        private string GeneratePatientVisitsHtml(IEnumerable<PatientVisitReportDto> data)
        {
            var sb = new StringBuilder();
            sb.Append("<h1>Patient Visits Report</h1>");
            sb.Append("<table border='1' cellpadding='5' cellspacing='0' width='100%'>");
            sb.Append("<tr><th>Patient Name</th><th>Age</th><th>Gender</th><th>Total Visits</th><th>Last Visit</th><th>Complaint</th></tr>");

            foreach (var patient in data)
            {
                sb.Append($"<tr><td>{patient.PatientName}</td><td>{patient.Age}</td>" +
                          $"<td>{patient.Gender}</td><td>{patient.TotalVisits}</td>" +
                          $"<td>{patient.LastVisitDate}</td><td>{patient.MostCommonComplaint}</td></tr>");
            }

            sb.Append("</table>");
            return sb.ToString();
        }
    }
}
