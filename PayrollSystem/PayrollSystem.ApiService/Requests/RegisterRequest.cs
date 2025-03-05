﻿namespace PayrollSystem.ApiService.Requests;



public sealed class RegisterRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
