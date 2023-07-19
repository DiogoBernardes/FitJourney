using BusinessLogic.Models.Exercises;
using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJourney_BackEnd.Controllers;
[Route ("api/[controller]")]
[ApiController]
public class ExercisesController : ControllerBase
{

    private readonly IExercisesRepository _exercisesRepository;

    public ExercisesController(IExercisesRepository exercisesRepository)
    {
        _exercisesRepository = exercisesRepository;
    }
    
    // GET: api/exercise
    [HttpGet("GetAllExercises")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetExercises()
    {
        var exercises = await _exercisesRepository.GetExercises();
        return Ok(exercises);
    }
    
    // POST: api/exercise
    [HttpPost ("AddExercise")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateExercise([FromBody] CreateExercisesModel newExercise)
    {
        if (ModelState.IsValid)
        {
            // Verify if name already exist
            var existingExercise = await _exercisesRepository.GetExerciseByName(newExercise.ExerciseName);
            if (existingExercise != null)
            {
                ModelState.AddModelError("Exercise", "Exercise already exists");
                return BadRequest(ModelState);
            }
            
            var createdExercise = await _exercisesRepository.CreateExercise(newExercise);
            return CreatedAtAction(nameof(GetExerciseByName), new { name = createdExercise.ExerciseName }, createdExercise);
        }

        return BadRequest(ModelState);
    }
    
    
    // PUT: api/exercise/{id}
    [HttpPut("UpdateExercise/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateExercise(int id, [FromBody] UpdateExerciseModel updatedExercise)
    {
        if (id != updatedExercise.ExerciseID)
        {
            return BadRequest("Exercise Not Found!");
        }
        
        var existingExercise = await _exercisesRepository.GetExerciseByName(updatedExercise.ExerciseName);

        if (updatedExercise != null && updatedExercise.ExerciseID != id)
        {
            return Conflict("Exercise already exists!");
        }
        
        await _exercisesRepository.UpdateExercise(updatedExercise);

        return NoContent();
    }
    
    // DELETE: api/exercise/{id}
    [HttpDelete("DeleteExercise/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        await _exercisesRepository.DeleteExercise(id);

        return NoContent();
    }

    
    // GET: api/exercise
    [HttpGet("GetExerciseByName/{name}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetExerciseByName(string name)
    {
        var exercise = await _exercisesRepository.GetExerciseByName(name);
        return Ok(exercise);
    }
}