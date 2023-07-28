using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models.Achievement;
using BusinessLogic.Models.Role;
using BusinessLogic.Models.User;
using FitJourney_BackEnd.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitJourney_BackEnd.Repositories;

public class AchievementRepository : IAchievementRepository
{
  private readonly FitJourneyDbContext _context;

    public AchievementRepository(FitJourneyDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ListAchievementModel>> GetAchievements()
    { 
        List<ListAchievementModel> achievements = await _context.Set<Achievement>().Select(achievement => new ListAchievementModel()
        {
            AchievementID = achievement.AchievementId,
            User = new ListUserModel()
            {
                ID = achievement.User.UserId,
                Name = achievement.User.Name,
                DateOfBirth = achievement.User.Dateofbirth,
                Genre = achievement.User.Genre,
                Email = achievement.User.Email,
                Username = achievement.User.Username,
                Password = achievement.User.Password,
                Role = new RoleModel()
                {
                    ID = achievement.User.Role.RoleId,
                    Name = achievement.User.Role.RoleName
                }
            },
        }).ToListAsync();

        if (achievements.Count == 0)
        {
            throw new Exception("No Achievements found!");
        }

        return achievements;
    }
    
    public async Task DeleteAchievement(int id)
    {
        var achievement = await _context.Achievements.FirstOrDefaultAsync(a => a.AchievementId == id);

        if (achievement == null)
        {
            throw new ArgumentException("Achievement not found");
        }

        _context.Achievements.Remove(achievement);
        await _context.SaveChangesAsync();
    }

}