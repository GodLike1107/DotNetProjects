using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthcare.Core.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string MedicalHistory { get; set; }

        // Link with ASP.NET Identity User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Navigation property for appointments
        public ICollection<Appointment> Appointments { get; set; }
    }
}

