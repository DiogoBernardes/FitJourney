using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models.Exercises;
using BusinessLogic.Models.Sports;
using FitJourney_BackEnd.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitJourney_BackEnd.Repositories;

public class ExercisesRepository : IExercisesRepository
{
    private readonly FitJourneyDbContext _context;

    public ExercisesRepository(FitJourneyDbContext context)
    {
        _context = context;
    }

    public async Task<List<ListExercisesModel>> GetExercises()
    {
        List<ListExercisesModel> exercises = await _context.Set<Exercise>().Select(exercise => new ListExercisesModel()
        {
            ExerciseID = exercise.ExerciseId,
            ExerciseName = exercise.ExerciseName,
            ExerciseDescription = exercise.ExerciseDescription,
            Sport = new ListSportsModel()
            {
                SportID = exercise.Sport.SportId,
                SportName = exercise.Sport.SportName
            }
        }).ToListAsync();

        if (exercises.Count == 0)
        {
            throw new Exception("No Exercises registered!");
        }

        return exercises;
    }
    
    public async Task<ListExercisesModel> CreateExercise(CreateExercisesModel exercise)
    {
        var existingExercise = await GetExerciseByName(exercise.ExerciseName);

        if (existingExercise != null)
        {
            throw new ArgumentException("The exercise is already added!");
        }

        _context.Set<Exercise>().Add(new Exercise()
        {
            ExerciseName = exercise.ExerciseName,
            ExerciseDescription = exercise.ExerciseDescription,
            SportId = exercise.SportID,
        });
        await _context.SaveChangesAsync();
        return await GetExerciseByName(exercise.ExerciseName) ?? throw new InvalidOperationException("Impossible to create a new exercise, try again later!");
    }
    
    
    public async Task UpdateExercise(UpdateExerciseModel exercise)
    {
        var existingExercise = await _context.Exercises.FirstOrDefaultAsync(e => e.ExerciseId == exercise.ExerciseID);

        if (existingExercise == null)
        {
            throw new ArgumentException("Exercise not found!");
        }

        if (existingExercise.ExerciseName != null && existingExercise.ExerciseName.Equals(exercise.ExerciseName))
            
        {
            throw new ArgumentException("The exercise is already added, try another one!");
        }

        existingExercise.ExerciseName = exercise.ExerciseName;
        existingExercise.ExerciseDescription = exercise.ExerciseDescription;
        existingExercise.SportId = exercise.SportID;

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteExercise(int id)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.ExerciseId == id);

        if (exercise == null)
        {
            throw new ArgumentException("Exercise not found");
        }

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();
    }
    public async Task<ListExercisesModel?> GetExerciseByName(string name)
    {
        return _context.Set<Exercise>().Select(exercise => new ListExercisesModel()
        {
            ExerciseID = exercise.ExerciseId,
            ExerciseName = exercise.ExerciseName,
            ExerciseDescription = exercise.ExerciseDescription,
            Sport = new ListSportsModel()
            {
                SportID = exercise.Sport.SportId,
                SportName = exercise.Sport.SportName
            }
        }).FirstOrDefault(e => e.ExerciseName.Equals(name));
    }
    
}