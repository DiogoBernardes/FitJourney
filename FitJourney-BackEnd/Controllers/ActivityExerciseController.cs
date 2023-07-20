using BusinessLogic.Models.ActivityExercise;
using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitJourney_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityExerciseController : ControllerBase
    {
        private readonly IActivityExerciseRepository _activityExerciseRepository;

        public ActivityExerciseController(IActivityExerciseRepository activityExerciseRepository)
        {
            _activityExerciseRepository = activityExerciseRepository;
        }

        // GET: api/activityExercise/GetActivityExercises
        [HttpGet("GetActivityExercises")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActivityExercises()
        {
            try
            {
                var activitiesActivityExercises = await _activityExerciseRepository.GetActivityExercises();
                return Ok(activitiesActivityExercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving activity exercises: {ex.Message}");
            }
        }

        // POST: api/activityExercise/AddActivityExercise
        [HttpPost("AddActivityExercise")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateActivityExercise([FromBody] CreateActivityExerciseModel newActivityExercise)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verify if the activity exercise already exists
                    var existingActivityExercise = await _activityExerciseRepository.GetActivityExerciseById(newActivityExercise.ActivityExerciseID);

                    // Check if the existing activity exercise is associated with the same user
                    if (existingActivityExercise != null && existingActivityExercise.Activity.ActivityID== newActivityExercise.ActivityID &&
                        existingActivityExercise.Activity.Exercise.ExerciseID == newActivityExercise.ExerciseID)
                    {
                        ModelState.AddModelError("ActivityID", "You already have an exercise with the same name on this activity!");
                        return BadRequest(ModelState);
                    }

                    var createdActivityExercise = await _activityExerciseRepository.CreateActivityExercise(newActivityExercise);

                    if (createdActivityExercise == null)
                    {
                        return StatusCode(500, "Failed to create activity exercise. Please try again later.");
                    }

                    return CreatedAtAction(nameof(GetActivityExerciseById), new { id = createdActivityExercise.ActivityExerciseID }, createdActivityExercise);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error creating activity exercise: {ex.Message}");
                }
            }

            return BadRequest(ModelState);
        }


        // PUT: api/activityExercise/UpdateActivityExercise
        [HttpPut("UpdateActivityExercise/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateActivityExercise(int id,[FromBody] UpdateActivityExerciseModel updatedActivityExercise)
        {
            if (id != updatedActivityExercise.ActivityExerciseID)
            {
                return BadRequest("Activity exercise Not Found!");
            }
        
            // Verify if the activity exercise already exists
            var existingActivityExercise = await _activityExerciseRepository.GetActivityExerciseById(updatedActivityExercise.ActivityExerciseID);

            // Check if the existing activity exercise is associated with the same user
            if (existingActivityExercise != null && existingActivityExercise.Activity.ActivityID== updatedActivityExercise.ActivityID &&
                existingActivityExercise.Activity.Exercise.ExerciseID == updatedActivityExercise.ExerciseID)
            {
                ModelState.AddModelError("ActivityID", "You already have an exercise with the same name on this activity!");
                return BadRequest(ModelState);
            }
            // Update the activity
            await _activityExerciseRepository.UpdateActivityExercise(updatedActivityExercise);

            return NoContent();
        }

        // DELETE: api/activityExercise/DeleteActivityExercise
        [HttpDelete("DeleteActivityExercise/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteActivityExercise(int id)
        {
            try
            {
                await _activityExerciseRepository.DeleteActivityExercise(id);
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

        // GET: api/activityExercise/GetActivityExerciseById
        [HttpGet("GetActivityExerciseById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActivityExerciseById(int id)
        {
            try
            {
                var activityExercise = await _activityExerciseRepository.GetActivityExerciseById(id);

                if (activityExercise == null)
                {
                    return NotFound();
                }

                return Ok(activityExercise);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving activity exercise: {ex.Message}");
            }
        }
    }
}
