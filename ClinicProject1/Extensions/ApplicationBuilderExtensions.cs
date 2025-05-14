using ClinicProject1.Data;
using ClinicProject1.Models.Entities;
using ClinicProject1.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ClinicProject1.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ClinicDbContext>();
                //await ClearDatabase(context);
                await context.Database.MigrateAsync();

                if (!await context.Users.AnyAsync(u => u.Username == "Admin1"))
                {
                    await SeedAdmin(context);
                    await SeedDoctors(context);
                    await SeedPatients(context);
                    await SeedAppointments(context);
                    await SeedMedicalRecords(context);

                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database");
            }
        }

        private static async Task SeedAdmin(ClinicDbContext context)
        {
            var admin = new User
            {
                Username = "Admin1",
                Photo = "admin-profile.jpg",
                Password = "Admin@1234",
                Role = Role.Admin,
                FirstName = "Admin",
                LastName = "1",
                Email = "admin@clinic.com",
                PhoneNumber = "1234567890"
            };
            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }

        private static async Task SeedDoctors(ClinicDbContext context)
        {
            var doctors = new List<(User User, Doctor Doctor, DoctorAvailability Availability)>
            {
                new(
                    new User
                    {
                        Username = "Doctor1",
                        Photo = "dr1-Profile.jpg",
                        Password = "Doctor@123",
                        Role = Role.Doctor,
                        FirstName = "Doctor",
                        LastName = "1",
                        Email = "Doctor1@clinic.com",
                        PhoneNumber = "1234567891"
                    },
                    new Doctor { Specialization = Specialization.Cardiologist },
                    new DoctorAvailability
                    {
                        Day1 = WorkDays.Sunday,
                        Day2 = WorkDays.Wednesday,
                        IsAvailable = true
                    }
                ),
                new(
                    new User
                    {
                        Username = "Doctor2",
                        Photo = "dr2-profile.jpg",
                        Password = "Doctor@123",
                        Role = Role.Doctor,
                        FirstName = "Doctor",
                        LastName = "2",
                        Email = "Doctor2@clinic.com",
                        PhoneNumber = "1234567892"
                    },
                    new Doctor { Specialization = Specialization.Dentist },
                    new DoctorAvailability
                    {
                        Day1 = WorkDays.Monday,
                        Day2 = WorkDays.Thursday,
                        IsAvailable = true
                    }
                ),
                new(
                    new User
                    {
                        Username = "Doctor3",
                        Photo = "dr3-profile.jpg",
                        Password = "Doctor@123",
                        Role = Role.Doctor,
                        FirstName = "Doctor",
                        LastName = "3",
                        Email = "Doctor3@clinic.com",
                        PhoneNumber = "1234567893"
                    },
                    new Doctor { Specialization = Specialization.Neurologist },
                    new DoctorAvailability
                    {
                        Day1 = WorkDays.Saturday,
                        Day2 = WorkDays.Tuesday,
                        IsAvailable = true
                    }
                )
            };

            foreach (var (user, doctor, availability) in doctors)
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                doctor.DoctorId = user.UserId;
                await context.Doctors.AddAsync(doctor);
                await context.SaveChangesAsync();

                availability.DoctorId = doctor.DoctorId;
                await context.DoctorAvailability.AddAsync(availability);
            }

            await context.SaveChangesAsync();
            }

        private static async Task SeedPatients(ClinicDbContext context)
        {
            var patients = new List<(User User, Patient Patient)>
            {
                new(
                    new User
                    {
                        Username = "Patient1",
                        Photo = "patient1.jpg",
                        Password = "Patient@123",
                        Role = Role.Patient,
                        FirstName = "Patient",
                        LastName = "1",
                        Email = "Patient1@clinic.com",
                        PhoneNumber = "1987654321"
                    },
                    new Patient
                    {
                        Age = 35,
                        Gender = Gender.Male,
                        ChronicDiseases = "Hypertension",
                        MedicalComplaint = "Routine checkup"
                    }
                ),
                new(
                    new User
                    {
                        Username = "Patient2",
                        Photo = "patient2.jpg",
                        Password = "Patient@123",
                        Role = Role.Patient,
                        FirstName = "Patient",
                        LastName = "2",
                        Email = "Patient2@clinic.com",
                        PhoneNumber = "1987654322"
                    },
                    new Patient
                    {
                        Age = 28,
                        Gender = Gender.Female,
                        ChronicDiseases = "Asthma",
                        MedicalComplaint = "Allergy symptoms"
                    }
                ),
                new(
                    new User
                    {
                        Username = "Patient3",
                        Photo = "patient3.jpg",
                        Password = "Patient@123",
                        Role = Role.Patient,
                        FirstName = "Patient",
                        LastName = "3",
                        Email = "Patient3@clinic.com",
                        PhoneNumber = "1987654323"
                    },
                    new Patient
                    {
                        Age = 45,
                        Gender = Gender.Male,
                        ChronicDiseases = "Diabetes",
                        MedicalComplaint = "Annual physical"
                    }
                )
            };

            foreach (var (user, patient) in patients)
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                patient.PatientId = user.UserId;
                await context.Patients.AddAsync(patient);
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedAppointments(ClinicDbContext context)
        {
            var doctors = await context.Doctors.Take(2).ToListAsync();
            var patients = await context.Patients.Take(2).ToListAsync();

            var appointments = new List<Appointment>
            {
                new()
                {
                    PatientId = patients[0].PatientId,
                    DoctorId = doctors[0].DoctorId,
                    AppointmentDay = WorkDays.Sunday,
                    Time = AppointmentTimes.Time_0800,
                    Status = AppointmentStatus.Pending
                },
                new()
                {
                    PatientId = patients[1].PatientId,
                    DoctorId = doctors[0].DoctorId,
                    AppointmentDay = WorkDays.Wednesday,
                    Time = AppointmentTimes.Time_0900,
                    Status = AppointmentStatus.Pending
                },
                new()
                {
                    PatientId = patients[0].PatientId,
                    DoctorId = doctors[1].DoctorId,
                    AppointmentDay = WorkDays.Monday,
                    Time = AppointmentTimes.Time_0830,
                    Status = AppointmentStatus.Approved
                },
                new()
                {
                    PatientId = patients[1].PatientId,
                    DoctorId = doctors[1].DoctorId,
                    AppointmentDay = WorkDays.Thursday,
                    Time = AppointmentTimes.Time_0700,
                    Status = AppointmentStatus.Canceled
                }
            };

            await context.Appointments.AddRangeAsync(appointments);
            await context.SaveChangesAsync();
        }

        private static async Task SeedMedicalRecords(ClinicDbContext context)
        {
            var appointments = await context.Appointments
                .Where(a => a.Status == AppointmentStatus.Approved)
                .Take(1)
                .ToListAsync();

            var medicalRecords = new List<MedicalRecord>
            {
                new()
                {
                    PatientId = appointments[0].PatientId,
                    DoctorId = appointments[0].DoctorId,
                    Diagnosis = "Hypertension under control",
                    Prescription = "Continue current medication"
                }
            };

            await context.MedicalRecords.AddRangeAsync(medicalRecords);
            await context.SaveChangesAsync();
        }

        //private static async Task ClearDatabase(ClinicDbContext context)
        //{
        //    context.MedicalRecords.RemoveRange(context.MedicalRecords);
        //    context.Appointments.RemoveRange(context.Appointments);
        //    context.DoctorAvailability.RemoveRange(context.DoctorAvailability);
        //    context.Patients.RemoveRange(context.Patients);
        //    context.Doctors.RemoveRange(context.Doctors);
        //    context.Users.RemoveRange(context.Users);

        //    await context.SaveChangesAsync();
        //}
    }
}
