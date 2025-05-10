using AutoMapper;
using ClinicProject1.Models.Entities;
using ClinicProject1.Models.Enums;
using ClinicProject1.Models.DTOs.AppointmentDtos;
using ClinicProject1.Models.DTOs.AvailabilityDTOs;
using ClinicProject1.Models.DTOs.DoctorDTOs;
using ClinicProject1.Models.DTOs.MedicalRecordDTOs;
using ClinicProject1.Models.DTOs.ReportsDTOs;

namespace ClinicProject1
{
    public class MappingProfile : Profile
    {

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
                AppointmentTimes.Time_0930 => "09:30 PM",

            };
        }

        public MappingProfile()
        {
            // Doctor mappings
            CreateMap<Doctor, DoctorDashboardDto>()
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization.ToString()))
                .ForMember(dest => dest.AvailableDays, opt => opt.MapFrom(src => src.Availabilities.Any()? 
                    new List<string> { $"{src.Availabilities.First().Day1.ToString()} - {src.Availabilities.First().Day2.ToString()}" }: new List<string>()))
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.Availabilities.Any()? 
                    $"{src.Availabilities.First().StartTime} - {src.Availabilities.First().EndTime}" : null));

            CreateMap<CreateDoctorDto, Doctor>()
                .ForPath(dest => dest.User.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.User.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.User.Username, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForPath(dest => dest.User.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.User.Password, opt => opt.MapFrom(src => src.Password))
                .ForPath(dest => dest.User.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForPath(dest => dest.User.Role, opt => opt.MapFrom(_ => Role.Doctor))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization.ToString()));
            
            CreateMap<UpdateDoctorDto, User>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<DoctorAvailability, DoctorAvailabilityDto>()
                .ForMember(dest => dest.AvailableDays, opt => opt.MapFrom(src =>
                    new List<string> { src.Day1.ToString(), src.Day2.ToString() }))
                .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src =>
                    $"{src.StartTime} - {src.EndTime}"));

            CreateMap<AssignAvailabilityDto, DoctorAvailability>()
                .ForMember(dest => dest.Day1, opt => opt.MapFrom(src => src.AvailableDays[0]))
                .ForMember(dest => dest.Day2, opt => opt.MapFrom(src => src.AvailableDays[1]))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(_ => true));

            // Appointment mappings
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => AppointmentStatus.Pending));

            CreateMap<RescheduleAppointmentDto, Appointment>()
                .ForMember(dest => dest.AppointmentDay, opt => opt.MapFrom(src => src.NewAppointmentDay))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.NewTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => AppointmentStatus.Pending));

            CreateMap<Appointment, AppointmentDashboardDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.User.FirstName + " " + src.Patient.User.LastName))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.User.FirstName + " " + src.Doctor.User.LastName))
                .ForMember(dest => dest.AppointmentDay, opt => opt.MapFrom(src => src.AppointmentDay.ToString()))
                .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => FormatTime(src.Time)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId));

            CreateMap<MedicalRecord, MedicalRecordDto>();

            CreateMap<Doctor, DoctorScheduleReportDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization.ToString()))
                .ForMember(dest => dest.AvailableDays, opt => opt.MapFrom(src => src.Availabilities.Any() ?
                    new List<string> { src.Availabilities.First().Day1.ToString(), src.Availabilities.First().Day2.ToString() } : new List<string>()))
                .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src => src.Availabilities.Any() ?
                        $"{src.Availabilities.First().StartTime} - {src.Availabilities.First().EndTime}" : string.Empty))
                .ForMember(dest => dest.TotalAppointments, opt => opt.MapFrom(src => src.Appointments.Count));

            CreateMap<Patient, PatientVisitReportDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.TotalVisits, opt => opt.MapFrom(src => src.Appointments.Count(a => a.Status == AppointmentStatus.Approved)))
                .ForMember(dest => dest.LastVisitDate, opt => opt.MapFrom(src => src.Appointments .OrderByDescending(a => a.AppointmentId) .FirstOrDefault().AppointmentDay.ToString()))
                .ForMember(dest => dest.MostCommonComplaint, opt => opt.MapFrom(src => src.MedicalComplaint));
        }
    }
}
