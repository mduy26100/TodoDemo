﻿namespace Application.DTOs.Users
{
    public class RegisterDTO
    {
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
