namespace SmartHealthcare.Core.DTOs
{
    public class PatientDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string MedicalHistory { get; set; } = null!;
    }
}
