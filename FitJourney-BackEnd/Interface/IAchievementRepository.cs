using BusinessLogic.Models.Achievement;

namespace FitJourney_BackEnd.Interface;

public interface IAchievementRepository
{
    Task<List<ListAchievementModel>> GetAchievements();
    Task DeleteAchievement(int id);
}