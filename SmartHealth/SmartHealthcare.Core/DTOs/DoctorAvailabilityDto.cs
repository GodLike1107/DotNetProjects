using System;

namespace SmartHealthcare.Core.DTOs
{
    public class DoctorAvailabilityDto
    {
        public DateTime AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Notes { get; set; }
    }
}