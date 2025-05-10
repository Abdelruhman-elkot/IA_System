using AutoMapper;
using ClinicProject1.Models.DTOs.AppointmentDtos;
using ClinicProject1.Models.Entities;
using ClinicProject1.Models.Enums;
using ClinicProject1.Repositories.Interfaces;
using ClinicProject1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicProject1.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository repository,
            IDoctorAvailabilityRepository availabilityRepository,
            IMapper mapper)
        {
            _appointmentRepository = repository;
            _availabilityRepository = availabilityRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDashboardDto>> GetAllAppointments()
        {
            var appointments = await _appointmentRepository.GetAllAppointments();
            return _mapper.Map<IEnumerable<AppointmentDashboardDto>>(appointments);
        }

        public async Task<AppointmentDashboardDto> GetAppointmentById(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found");

            return _mapper.Map<AppointmentDashboardDto>(appointment);
        }

        public async Task<IEnumerable<AppointmentDashboardDto>> GetAppointmentsByPatientId(int patientId)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByPatientId(patientId);
            return _mapper.Map<IEnumerable<AppointmentDashboardDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDashboardDto>> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByDoctorId(doctorId);
            return _mapper.Map<IEnumerable<AppointmentDashboardDto>>(appointments);
        }

        public async Task<AppointmentDashboardDto> ApproveAppointment(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found");

            appointment.Status = AppointmentStatus.Approved;
            await _appointmentRepository.UpdateAppointment(appointment);
            return _mapper.Map<AppointmentDashboardDto>(appointment);
        }

        public async Task<AppointmentDashboardDto> BookAppointment(CreateAppointmentDto appointmentDto)
        {
            var isAvailable = await IsTimeSlotAvailable(appointmentDto.DoctorId,
                appointmentDto.AppointmentDay, appointmentDto.Time);

            if (!isAvailable)
            {
                throw new ArgumentException("The selected time slot is not available");
            }

            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDay = appointmentDto.AppointmentDay,
                Time = appointmentDto.Time,
                Status = AppointmentStatus.Pending
            };

            await _appointmentRepository.AddAppointment(appointment);
            return _mapper.Map<AppointmentDashboardDto>(appointment);
        }

        public async Task<AppointmentDashboardDto> CancelAppointment(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found");

            appointment.Status = AppointmentStatus.Canceled;
            await _appointmentRepository.UpdateAppointment(appointment);
            return _mapper.Map<AppointmentDashboardDto>(appointment);
        }


        public async Task<AppointmentDashboardDto> RescheduleAppointment(
            int appointmentId, [FromBody] RescheduleAppointmentDto rescheduleDto)
        {
            var existingAppointment = await _appointmentRepository.GetAppointmentById(appointmentId);
            if (existingAppointment == null)
                throw new KeyNotFoundException("Appointment not found");

            var isAvailable = await IsTimeSlotAvailable(
                existingAppointment.DoctorId,
                rescheduleDto.NewAppointmentDay,
                rescheduleDto.NewTime);

            if (!isAvailable)
            {
                throw new ArgumentException("The selected time slot is not available");
            }

            existingAppointment.AppointmentDay = rescheduleDto.NewAppointmentDay;
            existingAppointment.Time = rescheduleDto.NewTime;
            existingAppointment.Status = AppointmentStatus.Pending;

            await _appointmentRepository.UpdateAppointment(existingAppointment);
            return _mapper.Map<AppointmentDashboardDto>(existingAppointment);
        }


        public async Task<List<AppointmentTimes>> GetAvailableTimeSlots(int doctorId, WorkDays day)
        {
            var availability = await _availabilityRepository.GetAvailabilityByDoctorId(doctorId);
            if (availability == null || !availability.IsAvailable)
                return new List<AppointmentTimes>();

            if (day != availability.Day1 && day != availability.Day2)
                return new List<AppointmentTimes>();

            var existingAppointments = await _appointmentRepository.GetAppointmentsByDoctorId(doctorId);
            var allTimeSlots = Enum.GetValues(typeof(AppointmentTimes)).Cast<AppointmentTimes>().ToList();

            return allTimeSlots.Where(time =>
                !existingAppointments.Any(a =>
                    a.AppointmentDay == day &&
                    a.Time == time &&
                    (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Approved)
                )
            ).ToList();
        }


        private async Task<bool> IsTimeSlotAvailable(int doctorId, WorkDays day, AppointmentTimes time)
        {

            var availability = await _availabilityRepository.GetAvailabilityByDoctorId(doctorId);
            if (availability == null || !availability.IsAvailable)
                return false;

            if (day != availability.Day1 && day != availability.Day2)
                return false;

            var existingAppointments = await _appointmentRepository.GetAppointmentsByDoctorId(doctorId);
            var hasConflict = existingAppointments.Any(a =>
                a.AppointmentDay == day &&
                a.Time == time &&
                (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Approved));

            return !hasConflict;
        }
    }
}
