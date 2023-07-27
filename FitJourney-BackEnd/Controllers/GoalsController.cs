using BusinessLogic.Entities;
using BusinessLogic.Models.Challenges;
using BusinessLogic.Models.Goals;
using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJourney_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoalsController : ControllerBase
{

    public readonly IGoalsRepository _goalsRepository;

    public GoalsController(IGoalsRepository goalsRepository)
    {
        _goalsRepository = goalsRepository;
    }
    
    
    [HttpGet("GetGoals")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<ListGoalsModel>>> GetGoals()
    {
        try
        {
            var goals = await _goalsRepository.GetGoals();
            return Ok(goals);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving challenges: " + ex.Message);
        }
    }
    
    
    [HttpPost("CreateGoal")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ListGoalsModel>> CreateGoal(CreateGoalModel goal)
    {
        try
        {
            var newGoal = await _goalsRepository.CreateGoals(goal);
            return CreatedAtAction(nameof(GetGoalByName), new { name = newGoal.GoalName }, newGoal);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error creating challenge: " + ex.Message);
        }
    }

    
    [HttpPut("UpdateGoal/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateChallenge(int id, UpdateGoalModel goal)
    {
        try
        {
            if (id != goal.GoalID)
            {
                return BadRequest("Goal ID mismatch!");
            }

            await _goalsRepository.UpdateGoal(goal);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating challenge: " + ex.Message);
        }
    }
    
    [HttpDelete("DeleteGoal/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGoal(int id)
    {
        try
        {
            await _goalsRepository.DeleteGoal(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting challenge: " + ex.Message);
        }
    }
    
    [HttpGet("GetGoalByName/{name}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ListGoalsModel>> GetGoalByName(string name)
    {
        try
        {
            var goal = await _goalsRepository.GetGoalByName(name);
            if (goal == null)
            {
                return NotFound("Goal not found!");
            }
            return Ok(goal);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving challenge: " + ex.Message);
        }
    }
}