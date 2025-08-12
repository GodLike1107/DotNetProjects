using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHealthcare.Core.Entities
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }

        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        [Required]
        public string MedicationDetails { get; set; }  // e.g., "Paracetamol 500mg, twice daily"

        public string Notes { get; set; }              // Optional notes for the patient

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string FileUrl { get; set; } // Optional PDF file link
    }
}
