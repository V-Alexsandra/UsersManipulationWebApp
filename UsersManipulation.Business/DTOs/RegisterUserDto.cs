﻿namespace UsersManipulation.Business.DTOs
{
    public class RegisterUserDto
    {
        public string? Name { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? RepeatPassword { get; set; } = null!;
    }
}
