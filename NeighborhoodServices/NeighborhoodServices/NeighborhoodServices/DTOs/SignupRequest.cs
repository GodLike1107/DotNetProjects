﻿namespace NeighborhoodServices.DTOs
{
    public class SignupRequest
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "Customer";
    }
}
