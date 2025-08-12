using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthcare.Core.Entities
{
    public class MedicalRecord
    {
        public int MedicalRecordId { get; set; }
        public int PatientId { get; set; }
        public int? DoctorId { get; set; } // Optional: Which doctor added it
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FileUrl { get; set; } // If uploading a file
        public DateTime CreatedAt { get; set; }

        public Patient Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }

}
