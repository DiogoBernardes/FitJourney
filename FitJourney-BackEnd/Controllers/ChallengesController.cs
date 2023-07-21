using BusinessLogic.Models.Challenges;
using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitJourney_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengesController : ControllerBase
    {
        private readonly IChallengesRepository _challengesRepository;

        public ChallengesController(IChallengesRepository challengesRepository)
        {
            _challengesRepository = challengesRepository;
        }

        [HttpGet("GetChallenges")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ListChallengeModel>>> GetChallenges()
        {
            try
            {
                var challenges = await _challengesRepository.GetChallenges();
                return Ok(challenges);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving challenges: " + ex.Message);
            }
        }

        [HttpPost("CreateChallenge")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ListChallengeModel>> CreateChallenge(CreateChallengeModel challenge)
        {
            try
            {
                var newChallenge = await _challengesRepository.CreateChallenge(challenge);
                return CreatedAtAction(nameof(GetChallengeByName), new { name = newChallenge.ChallengeName }, newChallenge);
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

        [HttpPut("UpdateChallenge/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateChallenge(int id, UpdateChallengeModel challenge)
        {
            try
            {
                if (id != challenge.ChallengeID)
                {
                    return BadRequest("Challenge ID mismatch!");
                }

                await _challengesRepository.UpdateChallenge(challenge);
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

        [HttpDelete("DeleteChallenge/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteChallenge(int id)
        {
            try
            {
                await _challengesRepository.DeleteChallenge(id);
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

        [HttpGet("GetChallengeByName/{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ListChallengeModel>> GetChallengeByName(string name)
        {
            try
            {
                var challenge = await _challengesRepository.GetChallengeByName(name);
                if (challenge == null)
                {
                    return NotFound("Challenge not found!");
                }
                return Ok(challenge);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving challenge: " + ex.Message);
            }
        }
    }
}
