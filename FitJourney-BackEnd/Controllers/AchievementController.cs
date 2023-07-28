using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJourney_BackEnd.Controllers;

[Route ("api/[controller]")]
[ApiController]
public class AchievementController : ControllerBase
{
    
    private readonly IAchievementRepository _achievementRepository;

    public AchievementController(IAchievementRepository achievementRepository)
    {
        _achievementRepository = achievementRepository;
    }
    
    
    
    // GET: api/Achievements
    [HttpGet("GetAchievements")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetActivities()
    {
        var achievements = await _achievementRepository.GetAchievements();
        return Ok(achievements);
    }
    
    // DELETE: api/Achievement/DeleteAchievement
    [HttpDelete("DeleteAchievement/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAchievement(int id)
    {
        try
        {
            await _achievementRepository.DeleteAchievement(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting activity exercise: {ex.Message}");
        }
    }
}