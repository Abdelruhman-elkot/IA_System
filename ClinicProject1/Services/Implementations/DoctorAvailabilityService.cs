using AutoMapper;
using ClinicProject1.Models.DTOs.AvailabilityDTOs;
using ClinicProject1.Models.Entities;
using ClinicProject1.Models.Enums;
using ClinicProject1.Repositories.Interfaces;
using ClinicProject1.Services.Interfaces;

namespace ClinicProject1.Services.Implementations
{
    public class DoctorAvailabilityService : IDoctorAvailabilityService
    {
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAdminRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorAvailabilityService(
            IDoctorAvailabilityRepository availabilityRepository,
            IAppointmentRepository appointmentRepository,
            IAdminRepository doctorRepository,
            IMapper mapper)
        {
            _availabilityRepository = availabilityRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<DoctorAvailabilityDto> GetAvailability(int doctorId)
        {
            var doctor = await _doctorRepository.GetDoctorById(doctorId);
            if (doctor == null)
                throw new KeyNotFoundException("Doctor not found");

            var availability = await _availabilityRepository.GetAvailabilityByDoctorId(doctorId);
            if (availability == null)
                return null;

            var dto = new DoctorAvailabilityDto
            {
                DoctorId = doctorId,
                AvailableDays = new List<String> { availability.Day1.ToString(), availability.Day2.ToString() },
                WorkingHours = $"{availability.StartTime} - {availability.EndTime}",
                IsAvailable = availability.IsAvailable,
                AvailableTimeSlots = new Dictionary<string, List<string>>()
            };

            var appointments = await _appointmentRepository.GetAppointmentsByDoctorId(doctorId);

            foreach (var day in dto.AvailableDays)
            {
                var dayAppointments = appointments
                    .Where(a => a.AppointmentDay.ToString() == day)
                    .ToList();

                var allTimeSlots = Enum.GetValues(typeof(AppointmentTimes))
                    .Cast<AppointmentTimes>()
                    .ToList();

                var availableTimes = allTimeSlots
                    .Where(time => !dayAppointments.Any(a =>
                        a.Time == time &&
                        (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Approved)))
                    .Select(time => FormatTime(time))
                    .ToList();

                dto.AvailableTimeSlots.Add(day.ToString(), availableTimes);
            }

            return dto;
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

        private string FormatTime(AppointmentTimes time)
        {
            return time switch
            {
                AppointmentTimes.Time_0500 => "05:00 PM",
                AppointmentTimes.Time_0530 => "05:30 PM",
                AppointmentTimes.Time_0600 => "06:00 PM",
                AppointmentTimes.Time_0630 => "06:30 PM",
                AppointmentTimes.Time_0700 => "07:00 PM",
                AppointmentTimes.Time_0730 => "07:30 PM",
                AppointmentTimes.Time_0800 => "08:00 PM",
                AppointmentTimes.Time_0830 => "08:30 PM",
                AppointmentTimes.Time_0900 => "09:00 PM",
                AppointmentTimes.Time_0930 => "09:30 PM"
            };
        }
    }
}
