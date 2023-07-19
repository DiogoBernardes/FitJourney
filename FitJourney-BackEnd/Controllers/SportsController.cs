using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Models.Sports;
using FitJourney_BackEnd.Interface;
using Microsoft.AspNetCore.Authorization;

namespace FitJourney_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SportsController : ControllerBase
{
    private readonly ISportsRepository _sportsRepository;

    public SportsController(ISportsRepository sportsRepository)
    {
        _sportsRepository = sportsRepository;
    }
    
     // GET: api/user
    [HttpGet("GetAllSports")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSports()
    {
        var sports = await _sportsRepository.GetSports();
        if (sports.Count == 0)
        {
            BadRequest("No Sports Found!");
        }
        return Ok(sports);
    }
    
    // POST: api/user
    [HttpPost ("AddSport")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSport ([FromBody] CreateSportModel newSport)
    {
        if (ModelState.IsValid)
        {
            // Verify if email already exist
            var existingSport = await _sportsRepository.GetSportByName(newSport.SportName);
            if (existingSport != null)
            {
                ModelState.AddModelError("Sport", "Sport already exists");
                return BadRequest(ModelState);
            }
            
            var createdSport = await _sportsRepository.CreateSport(newSport);
            return CreatedAtAction(nameof(GetSportsByName), new { name = createdSport.SportName }, createdSport);
        }

        return BadRequest(ModelState);
    }

    // PUT: api/user/{id}
    [HttpPut("UpdateSport/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateSportModel updatedSport)
    {
        if (id != updatedSport.SportID)
        {
            return BadRequest("Sport Not Found!");
        }
        
        var existingSport = await _sportsRepository.GetSportByName(updatedSport.SportName);

        if (existingSport != null && existingSport.SportID != id)
        {
            return Conflict("Sport already exists!");
        }
        
        await _sportsRepository.UpdateSport(updatedSport);

        return NoContent();
    }
    
    // DELETE: api/user/{id}
    [HttpDelete("DeleteSport/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _sportsRepository.DeleteSport(id);

        return NoContent();
    }

    // GET: api/user
    [HttpGet("GetSportByName/{name}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSportsByName(string name)
    {
        var sports = await _sportsRepository.GetSportByName(name);
        return Ok(sports);
    }
    

}