﻿namespace Application.DTOs.Users
{
    public class AuthenticateResult
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
