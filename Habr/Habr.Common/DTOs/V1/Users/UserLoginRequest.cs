﻿namespace Habr.Common.DTOs.V1.Users;

public class UserLoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}