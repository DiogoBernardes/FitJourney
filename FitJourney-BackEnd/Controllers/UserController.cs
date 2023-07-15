using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FitJourney_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    // GET: api/user
    [HttpGet("GetAll")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetUsers();
        return Ok(users);
    }
    
    
    // GET: api/user
    [HttpGet("GetUserByEmail/{email}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        return Ok(user);
    }
    
    // GET: api/user
    [HttpGet("GetRoles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRole()
    {
        var roles = await _userRepository.GetRole();
        return Ok(roles);
    }
    
}