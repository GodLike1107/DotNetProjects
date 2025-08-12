using SmartHealthcare.Core.Enums;   // ✅ Correct use of enum
using System;

namespace SmartHealthcare.Core.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public DateTime ScheduledTime { get; set; }  // ✅ Good naming, clear intent

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;
        // ✅ Default enum value is good practice

        public string? Notes { get; set; }  // ✅ Nullable string is fine for optional notes

        // ✅ Foreign Key - Doctor
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        // ✅ Foreign Key - Patient
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}