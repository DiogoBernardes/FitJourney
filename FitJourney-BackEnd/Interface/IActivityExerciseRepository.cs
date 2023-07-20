using BusinessLogic.Models.ActivityExercise;

namespace FitJourney_BackEnd.Interface;

public interface IActivityExerciseRepository
{
    Task<List<ListActivityExerciseModel>> GetActivityExercises();
    Task<ListActivityExerciseModel> CreateActivityExercise(CreateActivityExerciseModel activityExercise);
    Task UpdateActivityExercise(UpdateActivityExerciseModel activityExercise);
    Task DeleteActivityExercise(int id);
    Task<ListActivityExerciseModel?> GetActivityExerciseById(int id);
}