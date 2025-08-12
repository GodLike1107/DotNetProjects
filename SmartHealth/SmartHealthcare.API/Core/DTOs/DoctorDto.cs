namespace SmartHealthcare.Core.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; } // Optional during POST
        public string Specialization { get; set; }
        public string Qualification { get; set; }
        public int Experience { get; set; }
        public string Availability { get; set; }

        // For creating doctor users
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
