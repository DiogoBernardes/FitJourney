using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models.Activities;
using BusinessLogic.Models.Exercises;
using BusinessLogic.Models.Role;
using BusinessLogic.Models.Sports;
using BusinessLogic.Models.User;
using FitJourney_BackEnd.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitJourney_BackEnd.Repositories;

public class ActivitiesRepository : IActivitiesRepository
{
    private readonly FitJourneyDbContext _context;

    public ActivitiesRepository(FitJourneyDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ListActivitiesModel>> GetActivities()
    { 
        List<ListActivitiesModel> activities = await _context.Set<Activity>().Select(activity => new ListActivitiesModel()
        {
            ActivityID = activity.ActivityId,
            ActivityName = activity.ActivityName,
            User = new ListUserModel()
            {
                ID = activity.User.UserId,
                Name = activity.User.Name,
                DateOfBirth = activity.User.Dateofbirth,
                Genre = activity.User.Genre,
                Email = activity.User.Email,
                Username = activity.User.Username,
                Password = activity.User.Password,
                Role = new RoleModel()
                {
                    ID = activity.User.Role.RoleId,
                    Name = activity.User.Role.RoleName
                }
            },
            Sport = new ListSportsModel()
            {
                SportID = activity.Sport.SportId,
                SportName = activity.Sport.SportName,
            },
            Exercise = new ListExercisesModel()
            {
                ExerciseID = activity.Exercise.ExerciseId,
                ExerciseName = activity.Exercise.ExerciseName,
                ExerciseDescription = activity.Exercise.ExerciseDescription,
                Sport = new ListSportsModel()
                {
                    SportID = activity.Exercise.Sport.SportId,
                    SportName = activity.Exercise.Sport.SportName
                }
            },
            Duration = activity.Duration,
            CaloriesBurned = activity.CaloriesBurned,
            ActivityDate = activity.ActivityDate
        }).ToListAsync();

        if (activities.Count == 0)
        {
            throw new Exception("No activities found!");
        }

        return activities;
    }

    public async Task<ListActivitiesModel> CreateActivity(CreateActivityModel activity)
    {
        var existingActivityWithSameName = await _context.Activities
            .FirstOrDefaultAsync(a => a.ActivityName == activity.ActivityName && a.UserId == activity.UserID);

        if (existingActivityWithSameName != null)
        {
            throw new ArgumentException("You already have an activity with the same name!");
        }
        _context.Set<Activity>().Add(new Activity()
        {
           ActivityName = activity.ActivityName,
           UserId = activity.UserID,
           SportId = activity.SportID,
           ExerciseId = activity.ExerciseID,
           Duration = activity.Duration,
           CaloriesBurned = activity.CaloriesBurned,
           ActivityDate = activity.ActivityDate
        });
        
        await _context.SaveChangesAsync();
        return await GetActivityByName(activity.ActivityName) ?? throw new InvalidOperationException("Impossible to add activity, try again later!");
    }

    public async Task UpdateActivity(UpdateActivityModel activity)
    {
        var existingActivity = await _context.Activities.FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityID);

        if (existingActivity == null)
        {
            throw new ArgumentException("Activity not found!");
        }

        if (existingActivity.ActivityName != null && existingActivity.ActivityName.Equals(activity.ActivityName) && 
            existingActivity.UserId == activity.UserID)
        {
            throw new ArgumentException("You already have an activity with the same name!");
        }

        existingActivity.ActivityName = activity.ActivityName;
        existingActivity.UserId = activity.UserID;
        existingActivity.SportId = activity.SportID;
        existingActivity.ExerciseId = activity.ExerciseID;
        existingActivity.Duration = activity.Duration;
        existingActivity.CaloriesBurned = activity.CaloriesBurned;
        existingActivity.ActivityDate = activity.ActivityDate;

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteActivity(int id)
    {
        var activity = await _context.Activities.FirstOrDefaultAsync(a => a.ActivityId == id);

        if (activity == null)
        {
            throw new ArgumentException("Activity not found");
        }

        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
    }

    public async Task<ListActivitiesModel?> GetActivityByName(string name)
    {
        return _context.Set<Activity>().Select(activity => new ListActivitiesModel()
        {
            ActivityID = activity.ActivityId,
            ActivityName = activity.ActivityName,
            User = new ListUserModel()
            {
                ID = activity.User.UserId,
                Name = activity.User.Name,
                DateOfBirth = activity.User.Dateofbirth,
                Genre = activity.User.Genre,
                Email = activity.User.Email,
                Username = activity.User.Username,
                Password = activity.User.Password,
                Role = new RoleModel()
                {
                    ID = activity.User.Role.RoleId,
                    Name = activity.User.Role.RoleName
                }
            },
            Sport = new ListSportsModel()
            {
                SportID = activity.Sport.SportId,
                SportName = activity.Sport.SportName,
            },
            Exercise = new ListExercisesModel()
            {
                ExerciseID = activity.Exercise.ExerciseId,
                ExerciseName = activity.Exercise.ExerciseName,
                ExerciseDescription = activity.Exercise.ExerciseDescription,
                Sport = new ListSportsModel()
                {
                    SportID = activity.Exercise.Sport.SportId,
                    SportName = activity.Exercise.Sport.SportName
                }
            },
            Duration = activity.Duration,
            CaloriesBurned = activity.CaloriesBurned,
            ActivityDate = activity.ActivityDate
        }).FirstOrDefault(a => a.ActivityName.Equals(name));
    }
}