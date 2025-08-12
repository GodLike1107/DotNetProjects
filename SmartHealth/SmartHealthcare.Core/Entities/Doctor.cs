using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthcare.Core.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public string Specialization { get; set; }
        public string Qualification { get; set; }       // 🆕 e.g. MBBS, MD
        public int Experience { get; set; }             // 🆕 In years
        public string Availability { get; set; }        // 🆕 e.g. "Mon-Fri, 10AM–5PM"

        // Link with ASP.NET Identity User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Navigation property for appointments
        public ICollection<Appointment> Appointments { get; set; }
    }
}
