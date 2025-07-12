namespace NeighborhoodServices.DTOs
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = "";
        public string ClientAppUrl { get; set; } = "";
    }
}
