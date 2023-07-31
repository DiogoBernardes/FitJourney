using BusinessLogic.Models.Email;
using BusinessLogic.Models.Goals;
using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJourney_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailRepository _emailRepository;

    public EmailController(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
    }
    
    [HttpPost("SendWelcomeEmail")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> SendWelcomeEmail(SendEmailModel emailModel)
    {
        try
        {
            await _emailRepository.SendWelcomeEmail(emailModel);
            return Ok("Welcome email has been sent with sucess!");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error sending welcome email!");
        }
    }

}