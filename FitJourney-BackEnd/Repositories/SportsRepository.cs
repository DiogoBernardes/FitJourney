using System.ComponentModel;
using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models.Role;
using BusinessLogic.Models.Sports;
using BusinessLogic.Models.User;
using FitJourney_BackEnd.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitJourney_BackEnd.Repositories;

public class SportsRepository : ISportsRepository
{
    private readonly FitJourneyDbContext _context;

    public SportsRepository(FitJourneyDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ListSportsModel>> GetSports()
    { 
        List<ListSportsModel> sports = await _context.Set<Sport>().Select(sport => new ListSportsModel()
        {
            SportID = sport.SportId,
            SportName = sport.SportName,
        }).ToListAsync();

        if (sports.Count == 0)
        {
            throw new Exception("No sports registered!");
        }

        return sports;
    }
    
    public async Task<ListSportsModel> CreateSport(CreateSportModel sport)
    {
        var existingSport = await GetSportByName(sport.SportName);

        if (existingSport != null)
        {
            throw new ArgumentException("The sport is already registed!");
        }
        _context.Set<Sport>().Add(new Sport()
        {
            SportName = sport.SportName,
        });
        
        await _context.SaveChangesAsync();
        return await GetSportByName(sport.SportName) ?? throw new InvalidOperationException("Impossible to create a new sport, try again later!");
    }
    

    public async Task UpdateSport(UpdateSportModel sport)
    {
        var existingSport = await _context.Sports.FirstOrDefaultAsync(s => s.SportId == sport.SportID);

        if (existingSport == null)
        {
            throw new ArgumentException("Sport not found!");
        }

        if (existingSport.SportName != null && existingSport.SportName.Equals(sport.SportName))
        {
            throw new ArgumentException("The Sport is already registed!");
        }

        existingSport.SportName = sport.SportName;

        await _context.SaveChangesAsync();
    }

    
    public async Task DeleteSport (int id)
    {
        var sport = await _context.Sports.FirstOrDefaultAsync(s => s.SportId == id);

        if (sport == null)
        {
            throw new ArgumentException("Sport not found");
        }

        _context.Sports.Remove(sport);
        await _context.SaveChangesAsync();
    }

    public async Task<ListSportsModel> GetSportByName(string name)
    {
        return _context.Set<Sport>().Select(sport => new ListSportsModel()
        {
            SportID = sport.SportId,
            SportName = sport.SportName,
        }).FirstOrDefault(s => s.SportName.Equals(name));
    }
    
}
