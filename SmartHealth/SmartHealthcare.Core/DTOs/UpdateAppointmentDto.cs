using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHealthcare.Core.Enums;

namespace SmartHealthcare.Core.DTOs
{
    public class UpdateAppointmentDto
    {
        public AppointmentStatus Status { get; set; }
        public DateTime? NewScheduledTime { get; set; }
    }
}

