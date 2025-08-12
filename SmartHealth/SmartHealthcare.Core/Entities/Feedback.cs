using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHealthcare.Core.Entities
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }

        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }  // 1 to 5 stars

        [MaxLength(1000)]
        public string Comments { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
