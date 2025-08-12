using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthcare.Core.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string MedicalHistory { get; set; }
        public string UserId { get; set; }
    }
}

