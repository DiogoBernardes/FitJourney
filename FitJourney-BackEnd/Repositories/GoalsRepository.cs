using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models.Exercises;
using BusinessLogic.Models.Goals;
using BusinessLogic.Models.Role;
using BusinessLogic.Models.Sports;
using BusinessLogic.Models.User;
using FitJourney_BackEnd.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitJourney_BackEnd.Repositories;

public class GoalsRepository : IGoalsRepository
{
    private readonly FitJourneyDbContext _context;

    public GoalsRepository(FitJourneyDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ListGoalsModel>> GetGoals()
    { 
        List<ListGoalsModel> goals = await _context.Set<Goal>().Select(goal => new ListGoalsModel()
        {
            GoalID = goal.GoalId,
            User = new ListUserModel()
            {
                ID = goal.User.UserId,
                Name = goal.User.Name,
                DateOfBirth = goal.User.Dateofbirth,
                Genre = goal.User.Genre,
                Email = goal.User.Email,
                Username = goal.User.Username,
                Password = goal.User.Password,
                Role = new RoleModel()
                {
                    ID = goal.User.Role.RoleId,
                    Name = goal.User.Role.RoleName
                }
            },
            Sport = new ListSportsModel()
            {
                SportID = goal.Sport.SportId,
                SportName = goal.Sport.SportName,
            },
            Exercise = new ListExercisesModel()
            {
                ExerciseID = goal.Exercise.ExerciseId,
                ExerciseName = goal.Exercise.ExerciseName,
                ExerciseDescription = goal.Exercise.ExerciseDescription,
                Sport = new ListSportsModel()
                {
                    SportID = goal.Exercise.Sport.SportId,
                    SportName = goal.Exercise.Sport.SportName
                }
            },
            GoalName = goal.GoalName,
            GoalDescription = goal.GoalDescription,
            TargetValue = goal.TargetValue,
            TargetUnit = goal.TargetUnit,
            StartDate = goal.StartDate,
            EndDate = goal.EndDate,
            Achieved = goal.Achieved
        }).ToListAsync();

        if (goals.Count == 0)
        {
            throw new Exception("No goals registered!");
        }

        return goals;
    }
    
    public async Task<ListGoalsModel> CreateGoals(CreateGoalModel goal)
    {
        var existingGoal = await GetGoalByName(goal.GoalName);

        if (existingGoal != null)
        {
            throw new ArgumentException("The Goal is already registed, try another one!");
        }
        _context.Set<Goal>().Add(new Goal()
        {
            GoalId = goal.GoalID,
            UserId = goal.UserID,
            SportId = goal.SportID,
            ExerciseId = goal.ExerciseID,
            GoalName = goal.GoalName,
            GoalDescription = goal.GoalDescription,
            TargetValue = goal.TargetValue,
            TargetUnit = goal.TargetUnit,
            StartDate = goal.StartDate,
            EndDate = goal.EndDate,
            Achieved = goal.Achieved
        });
        
        await _context.SaveChangesAsync();
        return await GetGoalByName(goal.GoalName) ?? throw new InvalidOperationException("Impossible to create a new goal, try again later!");
    }
    
    public async Task UpdateGoal(UpdateGoalModel goal)
    {
        var existingGoal = await _context.Goals.FirstOrDefaultAsync(g => g.GoalId == goal.GoalID);

        if (existingGoal == null)
        {
            throw new ArgumentException("Goal not found!");
        }

        if (existingGoal.GoalName != null && existingGoal.GoalName.Equals(goal.GoalName) && existingGoal.UserId == goal.UserID)
        {
            throw new ArgumentException("The challenge is already registed, try another one!");
        }

        existingGoal.GoalId = goal.GoalID;
        existingGoal.UserId = goal.UserID;
        existingGoal.SportId = goal.SportID;
        existingGoal.ExerciseId = goal.ExerciseID;
        existingGoal.GoalName = goal.GoalName;
        existingGoal.GoalDescription = goal.GoalDescription;
        existingGoal.TargetValue = goal.TargetValue;
        existingGoal.TargetUnit = goal.TargetUnit;
        existingGoal.StartDate = goal.StartDate;
        existingGoal.EndDate = goal.EndDate;
        existingGoal.Achieved = goal.Achieved;

            await _context.SaveChangesAsync();
    }
    
    public async Task DeleteGoal(int id)
    {
        var goal = await _context.Goals.FirstOrDefaultAsync(g => g.GoalId == id);
        
        if (goal == null)
        {
            throw new ArgumentException("Goal not found");
        }

        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();
    }
    public async Task<ListGoalsModel?> GetGoalByName(string name)
    {
        return _context.Set<Goal>().Select(goal => new ListGoalsModel()
        {
            GoalID = goal.GoalId,
            User = new ListUserModel()
            {
                ID = goal.User.UserId,
                Name = goal.User.Name,
                DateOfBirth = goal.User.Dateofbirth,
                Genre = goal.User.Genre,
                Email = goal.User.Email,
                Username = goal.User.Username,
                Password = goal.User.Password,
                Role = new RoleModel()
                {
                    ID = goal.User.Role.RoleId,
                    Name = goal.User.Role.RoleName
                }
            },
            Sport = new ListSportsModel()
            {
                SportID = goal.Sport.SportId,
                SportName = goal.Sport.SportName,
            },
            Exercise = new ListExercisesModel()
            {
                ExerciseID = goal.Exercise.ExerciseId,
                ExerciseName = goal.Exercise.ExerciseName,
                ExerciseDescription = goal.Exercise.ExerciseDescription,
                Sport = new ListSportsModel()
                {
                    SportID = goal.Exercise.Sport.SportId,
                    SportName = goal.Exercise.Sport.SportName
                }
            },
            GoalName = goal.GoalName,
            GoalDescription = goal.GoalDescription,
            TargetValue = goal.TargetValue,
            TargetUnit = goal.TargetUnit,
            StartDate = goal.StartDate,
            EndDate = goal.EndDate,
            Achieved = goal.Achieved
            
        }).FirstOrDefault(g => g.GoalName.Equals(name));
    }
}