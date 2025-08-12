using SmartHealthcare.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthcare.Core.DTOs
{
    public class AppointmentDto
    {
        public int DoctorId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;
        public string? Notes { get; set; }
    }
}
