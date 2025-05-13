using ClinicProject1.Models.Entities;
using ClinicProject1.Services.Interfaces;
using AutoMapper;
using ClinicProject1.Repositories.Interfaces;
using System.Text;
using ClinicProject1.Models.Enums;
using ClinicProject1.Models.DTOs.DoctorDTOs;
using ClinicProject1.Models.DTOs.ReportsDTOs;

namespace ClinicProject1.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminService(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
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

        #region Reports
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
        #endregion Reports
    }
}
