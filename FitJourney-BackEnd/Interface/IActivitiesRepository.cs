using BusinessLogic.Models.Activities;

namespace FitJourney_BackEnd.Interface;

public interface IActivitiesRepository
{
    Task<List<ListActivitiesModel>> GetActivities();
    Task<ListActivitiesModel> CreateActivity(CreateActivityModel activity);
    Task UpdateActivity(UpdateActivityModel activity);
    Task DeleteActivity(int id);
    Task<ListActivitiesModel?> GetActivityByName(string name);
}