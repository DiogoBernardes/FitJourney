using BusinessLogic.Models.Goals;

namespace FitJourney_BackEnd.Interface;

public interface IGoalsRepository
{
    Task<List<ListGoalsModel>> GetGoals();
    Task<ListGoalsModel> CreateGoals(CreateGoalModel goal);
    Task UpdateGoal(UpdateGoalModel goal);
    Task DeleteGoal(int id);
    Task<ListGoalsModel?> GetGoalByName(string name);
}