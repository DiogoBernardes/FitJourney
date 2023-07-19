﻿namespace BusinessLogic.Models.User;

public class CreateUserModel
{
    public string Name { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Genre { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int Role_id { get; set; }
}