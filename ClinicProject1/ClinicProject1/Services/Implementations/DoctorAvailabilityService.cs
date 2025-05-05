using AutoMapper;
using ClinicProject1.DTOs.AvailabilityDTOs;
using ClinicProject1.Models.Entities;
using ClinicProject1.Repositories.Interfaces;
using ClinicProject1.Services.Interfaces;

namespace ClinicProject1.Services.Implementations
{
    public class DoctorAvailabilityService : IDoctorAvailabilityService
    {
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        private readonly IAdminRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorAvailabilityService(
            IDoctorAvailabilityRepository availabilityRepository,
            IAdminRepository doctorRepository,
            IMapper mapper)
        {
            _availabilityRepository = availabilityRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<DoctorAvailabilityDto> GetAvailability(int doctorId)
        {
            var doctor = await _doctorRepository.GetDoctorById(doctorId);
            if (doctor == null)
                throw new KeyNotFoundException("Doctor not found");

            var availability = await _availabilityRepository.GetAvailabilityByDoctorId(doctorId);
            if (availability == null)
                return null;

            return _mapper.Map<DoctorAvailabilityDto>(availability);
        }

        public async Task<bool> AssignAvailability(int doctorId, AssignAvailabilityDto dto)
        {
            if (dto.AvailableDays.Count != 2)
                throw new ArgumentException("Exactly two days must be selected");

            var doctor = await _doctorRepository.GetDoctorById(doctorId);
            if (doctor == null)
                throw new KeyNotFoundException("Doctor not found");

            var existingAvailability = await _availabilityRepository.GetAvailabilityByDoctorId(doctorId);

            if (existingAvailability != null)
            {
                await _availabilityRepository.DeleteAvailability(existingAvailability);
            }

            var newAvailability = new DoctorAvailability
            {
                DoctorId = doctorId,
                Day1 = dto.AvailableDays[0],
                Day2 = dto.AvailableDays[1],
                StartTime = "05:00 PM",
                EndTime = "10:00 PM",
                IsAvailable = true
            };

            return await _availabilityRepository.CreateAvailability(newAvailability);
        }
    }
}
