using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthcare.Core.DTOs
{
    public class PrescriptionDto
    {
        public int AppointmentId { get; set; }
        public string MedicationDetails { get; set; }
        public string Notes { get; set; }
        public string FileUrl { get; set; }
    }

    public class PrescriptionResponseDto
    {
        public int PrescriptionId { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string MedicationDetails { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FileUrl { get; set; }
        public DateTime DateIssued { get; set; }
    }
}

