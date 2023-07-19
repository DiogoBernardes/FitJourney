using BusinessLogic.Models.Exercises;

namespace FitJourney_BackEnd.Interface;

public interface IExercisesRepository
{

    Task<List<ListExercisesModel>> GetExercises();
    Task<ListExercisesModel> CreateExercise(CreateExercisesModel exercise);
    Task UpdateExercise(UpdateExerciseModel exercise);
    Task DeleteExercise(int id);
    Task<ListExercisesModel> GetExerciseByName(string name);
}