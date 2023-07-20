using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models.Activities;
using BusinessLogic.Models.ActivityExercise;
using BusinessLogic.Models.Exercises;
using BusinessLogic.Models.Role;
using BusinessLogic.Models.Sports;
using BusinessLogic.Models.User;
using FitJourney_BackEnd.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitJourney_BackEnd.Repositories;

public class ActivityExerciseRepository : IActivityExerciseRepository
{
    private readonly FitJourneyDbContext _context;

    public ActivityExerciseRepository(FitJourneyDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ListActivityExerciseModel>> GetActivityExercises()
    { 
        List<ListActivityExerciseModel> activityExercise = await _context.Set<Activityexercise>().Select(activityExercise => new ListActivityExerciseModel()
        {
           ActivityExerciseID = activityExercise.ActivityExerciseId,
           Activity = new ListActivitiesModel()
           {
               ActivityID = activityExercise.Activity.ActivityId,
               ActivityName = activityExercise.Activity.ActivityName,
               User = new ListUserModel()
               {
                   ID = activityExercise.Activity.User.UserId,
                   Name = activityExercise.Activity.User.Name,
                   DateOfBirth = activityExercise.Activity.User.Dateofbirth,
                   Genre = activityExercise.Activity.User.Genre,
                   Email = activityExercise.Activity.User.Email,
                   Username = activityExercise.Activity.User.Username,
                   Password = activityExercise.Activity.User.Password,
                   Role = new RoleModel()
                   {
                       ID = activityExercise.Activity.User.Role.RoleId,
                       Name = activityExercise.Activity.User.Role.RoleName
                   }
               },
               Sport = new ListSportsModel()
               {
                   SportID = activityExercise.Activity.Sport.SportId,
                   SportName = activityExercise.Activity.Sport.SportName,
               },
               Exercise = new ListExercisesModel()
               {
                   ExerciseID = activityExercise.Activity.Exercise.ExerciseId,
                   ExerciseName = activityExercise.Activity.Exercise.ExerciseName,
                   ExerciseDescription = activityExercise.Activity.Exercise.ExerciseDescription,
                   Sport = new ListSportsModel()
                   {
                       SportID = activityExercise.Activity.Exercise.Sport.SportId,
                       SportName = activityExercise.Activity.Exercise.Sport.SportName
                   }
               }
           },
           Exercise = new ListExercisesModel()
           {
               ExerciseID = activityExercise.Activity.Exercise.ExerciseId,
               ExerciseName = activityExercise.Activity.Exercise.ExerciseName,
               ExerciseDescription = activityExercise.Activity.Exercise.ExerciseDescription,
               Sport = new ListSportsModel()
               {
                   SportID = activityExercise.Activity.Exercise.Sport.SportId,
                   SportName = activityExercise.Activity.Exercise.Sport.SportName
               }
           },
           Distance = activityExercise.Distance,
           Weight = activityExercise.Weight
           
        }).ToListAsync();

        if (activityExercise.Count == 0)
        {
            throw new Exception("No exercises found on this activity!");
        }

        return activityExercise;
    }
    
    public async Task<ListActivityExerciseModel> CreateActivityExercise(CreateActivityExerciseModel activityExercise)
    {
        var existingExerciseOnActivity = await _context.Activityexercises
            .FirstOrDefaultAsync(a => a.Exercise.ExerciseId == activityExercise.ExerciseID && a.Activity.ActivityId == activityExercise.ActivityID);

        if (existingExerciseOnActivity != null)
        {
            throw new ArgumentException("You already have an exercise with the same name on this activity!");
        }
        _context.Set<Activityexercise>().Add(new Activityexercise()
        {
            ActivityId = activityExercise.ActivityID,
            ExerciseId = activityExercise.ExerciseID,
            Distance = activityExercise.Distance,
            Weight = activityExercise.Weight
        });
        
        await _context.SaveChangesAsync();
        return await GetActivityExerciseById(activityExercise.ActivityID) ?? throw new InvalidOperationException("Impossible to add exercise to activity, try again later!");
    }
    
    public async Task UpdateActivityExercise(UpdateActivityExerciseModel activityExercise)
    {
        var existingActivityExercise = await _context.Activityexercises.FirstOrDefaultAsync(ae => ae.ActivityExerciseId == ae.ActivityId);

        if (existingActivityExercise == null)
        {
            throw new ArgumentException("Activity exercise not found!");
        }

        if (existingActivityExercise.ExerciseId != null && existingActivityExercise.ExerciseId == activityExercise.ExerciseID && 
        existingActivityExercise.ActivityId == activityExercise.ActivityID)
        {
            throw new ArgumentException("You already have this exercise on this activity!");
        }

        existingActivityExercise.ActivityExerciseId = activityExercise.ActivityExerciseID;
        existingActivityExercise.ActivityId = activityExercise.ActivityID;
        existingActivityExercise.ExerciseId = activityExercise.ExerciseID;
        existingActivityExercise.Distance = activityExercise.Distance;
        existingActivityExercise.Weight = activityExercise.Weight;

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteActivityExercise(int id)
    {
        var activityExercise = await _context.Activityexercises.FirstOrDefaultAsync(ae => ae.ActivityExerciseId == id);

        if (activityExercise == null)
        {
            throw new ArgumentException("Activity exercise not found");
        }

        _context.Activityexercises.Remove(activityExercise);
        await _context.SaveChangesAsync();
    }
    public async Task<ListActivityExerciseModel?> GetActivityExerciseById(int id)
    {
        return _context.Set<Activityexercise>().Select(activityExercise => new ListActivityExerciseModel()
        {
            
           ActivityExerciseID = activityExercise.ActivityExerciseId,
           Activity = new ListActivitiesModel()
           {
               ActivityID = activityExercise.Activity.ActivityId,
               ActivityName = activityExercise.Activity.ActivityName,
               User = new ListUserModel()
               {
                   ID = activityExercise.Activity.User.UserId,
                   Name = activityExercise.Activity.User.Name,
                   DateOfBirth = activityExercise.Activity.User.Dateofbirth,
                   Genre = activityExercise.Activity.User.Genre,
                   Email = activityExercise.Activity.User.Email,
                   Username = activityExercise.Activity.User.Username,
                   Password = activityExercise.Activity.User.Password,
                   Role = new RoleModel()
                   {
                       ID = activityExercise.Activity.User.Role.RoleId,
                       Name = activityExercise.Activity.User.Role.RoleName
                   }
               },
               Sport = new ListSportsModel()
               {
                   SportID = activityExercise.Activity.Sport.SportId,
                   SportName = activityExercise.Activity.Sport.SportName,
               },
               Exercise = new ListExercisesModel()
               {
                   ExerciseID = activityExercise.Activity.Exercise.ExerciseId,
                   ExerciseName = activityExercise.Activity.Exercise.ExerciseName,
                   ExerciseDescription = activityExercise.Activity.Exercise.ExerciseDescription,
                   Sport = new ListSportsModel()
                   {
                       SportID = activityExercise.Activity.Exercise.Sport.SportId,
                       SportName = activityExercise.Activity.Exercise.Sport.SportName
                   }
               }
           },
           Exercise = new ListExercisesModel()
           {
               ExerciseID = activityExercise.Activity.Exercise.ExerciseId,
               ExerciseName = activityExercise.Activity.Exercise.ExerciseName,
               ExerciseDescription = activityExercise.Activity.Exercise.ExerciseDescription,
               Sport = new ListSportsModel()
               {
                   SportID = activityExercise.Activity.Exercise.Sport.SportId,
                   SportName = activityExercise.Activity.Exercise.Sport.SportName
               }
           },
           Distance = activityExercise.Distance,
           Weight = activityExercise.Weight
        }).FirstOrDefault(ae => ae.ActivityExerciseID == id);
    }
}