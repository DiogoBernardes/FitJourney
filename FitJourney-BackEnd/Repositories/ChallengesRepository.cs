using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models.Challenges;
using BusinessLogic.Models.Exercises;
using BusinessLogic.Models.Role;
using BusinessLogic.Models.Sports;
using BusinessLogic.Models.User;
using FitJourney_BackEnd.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitJourney_BackEnd.Repositories;

public class ChallengesRepository : IChallengesRepository
{
    private readonly FitJourneyDbContext _context;

    public ChallengesRepository(FitJourneyDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ListChallengeModel>> GetChallenges()
    { 
        List<ListChallengeModel> challenges = await _context.Set<Challenge>().Select(challenge => new ListChallengeModel()
        {
            ChallengeID = challenge.ChallengeId,
            User = new ListUserModel(){
                ID = challenge.Owner.UserId,
                Name = challenge.Owner.Name,
                DateOfBirth = challenge.Owner.Dateofbirth,
                Genre = challenge.Owner.Genre,
                Email = challenge.Owner.Email,
                Username = challenge.Owner.Username,
                Password = challenge.Owner.Password,
                Role = new RoleModel()
                {
                    ID = challenge.Owner.Role.RoleId,
                    Name = challenge.Owner.Role.RoleName
                }
            },
            Sport = new ListSportsModel()
            {
                SportID = challenge.Sport.SportId,
                SportName = challenge.Sport.SportName,
            },
            Exercise = new ListExercisesModel()
            {
                ExerciseID = challenge.Exercise.ExerciseId,
                ExerciseName = challenge.Exercise.ExerciseName,
                ExerciseDescription = challenge.Exercise.ExerciseDescription,
                Sport = new ListSportsModel()
                {
                    SportID = challenge.Exercise.Sport.SportId,
                    SportName = challenge.Exercise.Sport.SportName
                }
            },
            ChallengeName = challenge.ChallengeName,
            ChallengeDescription = challenge.ChallengeDescription,
            StartDate = challenge.StartDate,
            EndDate = challenge.EndDate,
            ChallengeAchieved = challenge.Achieved
        }).ToListAsync();

        if (challenges.Count == 0)
        {
            throw new Exception("No challenges registered!");
        }

        return challenges;
    }

    public async Task<ListChallengeModel> CreateChallenge(CreateChallengeModel challenge)
    {
        var existingChallenge = await GetChallengeByName(challenge.ChallengeName);

        if (existingChallenge != null)
        {
            throw new ArgumentException("The Challenge is already registed, try another one!");
        }
        _context.Set<Challenge>().Add(new Challenge()
        {
          OwnerId = challenge.OwnerID,
          SportId = challenge.SportID,
          ExerciseId = challenge.ExerciseID,
          ChallengeName = challenge.ChallengeName,
          ChallengeDescription = challenge.ChallengeDescription,
          StartDate = challenge.StartDate,
          EndDate = challenge.EndDate,
          Achieved = challenge.ChallengeAchieved
        });
        
        await _context.SaveChangesAsync();
        return await GetChallengeByName(challenge.ChallengeName) ?? throw new InvalidOperationException("Impossible to create a new challenge, try again later!");
    }

    public async Task UpdateChallenge(UpdateChallengeModel challenge)
    {
        var existingChallenge = await _context.Challenges.FirstOrDefaultAsync(c => c.ChallengeId == challenge.ChallengeID);

        if (existingChallenge == null)
        {
            throw new ArgumentException("Challenge not found!");
        }

        if (existingChallenge.ChallengeName != null && existingChallenge.ChallengeName.Equals(challenge.ChallengeName))
        {
            throw new ArgumentException("The challenge is already registed, try another one!");
        }

        existingChallenge.ChallengeId = challenge.ChallengeID;
        existingChallenge.OwnerId = challenge.OwnerID;
        existingChallenge.SportId = challenge.SportID;
        existingChallenge.ExerciseId = challenge.ExerciseID;
        existingChallenge.ChallengeName = challenge.ChallengeName;
        existingChallenge.ChallengeDescription = challenge.ChallengeDescription;
        existingChallenge.StartDate = challenge.StartDate;
        existingChallenge.EndDate = challenge.EndDate;
        existingChallenge.Achieved = challenge.ChallengeAchieved;

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteChallenge(int id)
    {
        var challenge = await _context.Challenges.FirstOrDefaultAsync(c => c.ChallengeId == id);

        if (challenge == null)
        {
            throw new ArgumentException("Challenge not found");
        }

        _context.Challenges.Remove(challenge);
        await _context.SaveChangesAsync();
    }
    
    public async Task<ListChallengeModel?> GetChallengeByName(string name)
    {
        return _context.Set<Challenge>().Select(challenge => new ListChallengeModel()
        {
            ChallengeID = challenge.ChallengeId,
            User = new ListUserModel(){
                ID = challenge.Owner.UserId,
                Name = challenge.Owner.Name,
                DateOfBirth = challenge.Owner.Dateofbirth,
                Genre = challenge.Owner.Genre,
                Email = challenge.Owner.Email,
                Username = challenge.Owner.Username,
                Password = challenge.Owner.Password,
                Role = new RoleModel()
                {
                    ID = challenge.Owner.Role.RoleId,
                    Name = challenge.Owner.Role.RoleName
                }
            },
            Sport = new ListSportsModel()
            {
                SportID = challenge.Sport.SportId,
                SportName = challenge.Sport.SportName,
            },
            Exercise = new ListExercisesModel()
            {
                ExerciseID = challenge.Exercise.ExerciseId,
                ExerciseName = challenge.Exercise.ExerciseName,
                ExerciseDescription = challenge.Exercise.ExerciseDescription,
                Sport = new ListSportsModel()
                {
                    SportID = challenge.Exercise.Sport.SportId,
                    SportName = challenge.Exercise.Sport.SportName
                }
            },
            ChallengeName = challenge.ChallengeName,
            ChallengeDescription = challenge.ChallengeDescription,
            StartDate = challenge.StartDate,
            EndDate = challenge.EndDate,
            ChallengeAchieved = challenge.Achieved
        }).FirstOrDefault(c => c.ChallengeName.Equals(name));
    }
}