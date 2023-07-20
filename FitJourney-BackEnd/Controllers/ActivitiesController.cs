using BusinessLogic.Models.Activities;
using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJourney_BackEnd.Controllers;

[Route ("api/[controller]")]
[ApiController]
public class ActivitiesController : ControllerBase
{
    private readonly IActivitiesRepository _activitiesRepository;

    public ActivitiesController(IActivitiesRepository activitiesRepository)
    {
        _activitiesRepository = activitiesRepository;
    }
    
    
    // GET: api/activity
    [HttpGet("GetActivities")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetActivities()
    {
        var activities = await _activitiesRepository.GetActivities();
        return Ok(activities);
    }
    
// POST: api/activity/AddActivity
    [HttpPost("AddActivity")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityModel newActivity)
    {
        if (ModelState.IsValid)
        {
            // Verify if user already has an activity with the same name
            var existingActivity = await _activitiesRepository.GetActivityByName(newActivity.ActivityName);

            // Check if the existing activity is associated with the same user
            if (existingActivity != null && existingActivity.User.ID == newActivity.UserID)
            {
                ModelState.AddModelError("ActivityName", "You already have an activity with the same name!");
                return BadRequest(ModelState);
            }

            // Create the new activity using the repository
            var createdActivity = await _activitiesRepository.CreateActivity(newActivity);
            return CreatedAtAction(nameof(GetActivityByName), new { name = createdActivity.ActivityName }, createdActivity);
        }

        return BadRequest(ModelState);
    }
    
    // PUT: api/activity/{id}
    [HttpPut("UpdateActivity/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateActivity(int id, [FromBody] UpdateActivityModel updatedActivity)
    {
        if (id != updatedActivity.ActivityID)
        {
            return BadRequest("Activity Not Found!");
        }
        
        // Verify if activity name already exists for user
        var existingActivityByName = await _activitiesRepository.GetActivityByName(updatedActivity.ActivityName);
        if (existingActivityByName != null && existingActivityByName.User.ID == updatedActivity.UserID)
        {
            return Conflict("You already have an activity with the same name!");
        }

        // Update the activity
        await _activitiesRepository.UpdateActivity(updatedActivity);

        return NoContent();
    }

    
    // DELETE: api/activity/{id}
    [HttpDelete("DeleteActivity/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        await _activitiesRepository.DeleteActivity(id);

        return NoContent();
    }
    
      
    // GET: api/user
    [HttpGet("GetActivityByName/{name}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetActivityByName(string name)
    {
        var activity = await _activitiesRepository.GetActivityByName(name);
        return Ok(activity);
    }

}