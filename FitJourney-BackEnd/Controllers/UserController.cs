using BusinessLogic.Models.User;
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
    [HttpGet("GetAllUsers")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetUsers();
        return Ok(users);
    }
    
    // POST: api/user
    [HttpPost ("RegistUser")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserModel newUser)
    {
        if (ModelState.IsValid)
        {
            // Verify if email already exist
            var existingEmailUser = await _userRepository.GetUserByEmail(newUser.Email);
            if (existingEmailUser != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return BadRequest(ModelState);
            }
            
            var createdUser = await _userRepository.CreateUser(newUser);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = createdUser.Email }, createdUser);
        }

        return BadRequest(ModelState);
    }

    // PUT: api/user/{id}
    [HttpPut("UpdateUser/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserModel updatedUser)
    {
        if (id != updatedUser.ID)
        {
            return BadRequest("User Not Found!");
        }
        
        var existingUserByEmail = await _userRepository.GetUserByEmail(updatedUser.Email);

        if (existingUserByEmail != null && existingUserByEmail.ID != id)
        {
            return Conflict("Email already exists!");
        }
        
        await _userRepository.UpdateUser(updatedUser);

        return NoContent();
    }
    
    // GET: api/user
    [HttpGet("GetUserByEmail/{email}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        return Ok(user);
    }
    
    // DELETE: api/user/{id}
    [HttpDelete("DeleteUser/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userRepository.DeleteUser(id);

        return NoContent();
    }
    
    // GET: api/user
    [HttpGet("GetUserRoles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRole()
    {
        var roles = await _userRepository.GetRole();
        return Ok(roles);
    }
    
}