using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthcare.Core.DTOs
{
    public class MedicalRecordDto
    {
        public int? PatientId { get; set; } // Only required for Doctor
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
    }

}
