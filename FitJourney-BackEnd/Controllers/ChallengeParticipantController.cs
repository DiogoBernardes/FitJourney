using BusinessLogic.Models.ChallengeParticipant;
using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJourney_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChallengeParticipantController : ControllerBase
{
    private readonly IChallengeParticipantRepository _challengeParticipantRepository;

        public ChallengeParticipantController(IChallengeParticipantRepository challengeParticipantRepository)
        {
            _challengeParticipantRepository = challengeParticipantRepository;
        }

        // GET: api/challengeParticipant/GetChallengeParticipant
        [HttpGet("GetChallengeParticipant")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetChallengeParticipants()
        {
            try
            {
                var ChallengeParticipants = await _challengeParticipantRepository.GetChallengeParticipants();
                return Ok(ChallengeParticipants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving activity exercises: {ex.Message}");
            }
        }
// POST: api/challengeParticipant/AddChallengeParticipant
        [HttpPost("AddChallengeParticipant")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateChallengeParticipant([FromBody] CreateChallengeParticipantModel newChallengeParticipant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verify if the activity exercise already exists
                    var existingChallengeParticipant = await
                        _challengeParticipantRepository.GetChallengeParticipantByID(newChallengeParticipant.ChallengeParticipantID);
                    
                    // Check if the existing activity exercise is associated with the same user
                    if (existingChallengeParticipant != null && existingChallengeParticipant.Challenge.ChallengeID == newChallengeParticipant.ChallengeID &&
                        existingChallengeParticipant.Challenge.User.ID == newChallengeParticipant.ParticipantID)
                    {
                        ModelState.AddModelError("ChallengeID", "You already have a regist on this challenge!");
                        return BadRequest(ModelState);
                    }

                    var createdChallengeParticipant = await _challengeParticipantRepository.CreateChallengeParticipant(newChallengeParticipant);

                    if (createdChallengeParticipant == null)
                    {
                        return StatusCode(500, "Failed to add a participant to the challenge. Please try again later.");
                    }

                    return CreatedAtAction(nameof(GetChallengeParticipantByID), new { id = createdChallengeParticipant.ChallengeParticipantID }, createdChallengeParticipant);
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
        


        
        // DELETE: api/challengeParticipant/DeleteChallengeParticipant
        [HttpDelete("DeleteChallengeParticipant/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteChallengeParticipant(int id)
        {
            try
            {
                await _challengeParticipantRepository.DeleteChallengeParticipant(id);
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

        // GET: api/challengeParticipant/GetChallengeParticipantByID
        [HttpGet("GetChallengeParticipantByID/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetChallengeParticipantByID(int id)
        {
            try
            {
                var challengeParticipant = await _challengeParticipantRepository.GetChallengeParticipantByID(id);

                if (challengeParticipant == null)
                {
                    return NotFound();
                }

                return Ok(challengeParticipant);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving activity exercise: {ex.Message}");
            }
        }
}