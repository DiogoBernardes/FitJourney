using BusinessLogic.Context;
using BusinessLogic.Entities;
using BusinessLogic.Models.ChallengeParticipant;
using BusinessLogic.Models.Challenges;
using BusinessLogic.Models.Role;
using BusinessLogic.Models.User;
using FitJourney_BackEnd.Interface;
using Microsoft.EntityFrameworkCore;

namespace FitJourney_BackEnd.Repositories;

public class ChallengeParticipantRepository : IChallengeParticipantRepository
{
    private readonly FitJourneyDbContext _context;

    public ChallengeParticipantRepository(FitJourneyDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ListChallengeParticipantModel>> GetChallengeParticipants()
    { 
        List<ListChallengeParticipantModel> challengeParticipants = await _context.Set<Challengeparticipant>().Select(challengeParticipant => new ListChallengeParticipantModel()
        {
            ChallengeParticipantID = challengeParticipant.ChallengeId, 
            Challenge = new ListChallengeModel()
            {
                ChallengeID = challengeParticipant.Challenge.ChallengeId,
                ChallengeName = challengeParticipant.Challenge.ChallengeName,
                ChallengeDescription = challengeParticipant.Challenge.ChallengeDescription,
                StartDate = challengeParticipant.Challenge.StartDate,
                EndDate = challengeParticipant.Challenge.EndDate,
                ChallengeAchieved = challengeParticipant.Challenge.Achieved
            },
            Participant = new ListUserModel()
            {
                ID = challengeParticipant.Participant.UserId,
                Name = challengeParticipant.Participant.Name,
                DateOfBirth = challengeParticipant.Participant.Dateofbirth,
                Genre = challengeParticipant.Participant.Genre,
                Email = challengeParticipant.Participant.Email,
                Username = challengeParticipant.Participant.Username,
                Password = challengeParticipant.Participant.Password,
                Role = new RoleModel()
                {
                    ID = challengeParticipant.Participant.Role.RoleId,
                    Name = challengeParticipant.Participant.Role.RoleName
                }
            }
        }).ToListAsync();

        if (challengeParticipants.Count == 0)
        {
            throw new Exception("No participants registered on this challenge!");
        }

        return challengeParticipants;
    }
    
    public async Task<ListChallengeParticipantModel> CreateChallengeParticipant(CreateChallengeParticipantModel challengeParticipant)
    {
        var existingParticipantOnChallenge = await _context.Challengeparticipants
            .FirstOrDefaultAsync(a => a.Challenge.ChallengeId == challengeParticipant.ChallengeID && a.Participant.UserId == challengeParticipant.ParticipantID);

        if (existingParticipantOnChallenge != null)
        {
            throw new ArgumentException("You already have a regist on this challenge!");
        }
        _context.Set<Challengeparticipant>().Add(new Challengeparticipant()
        {
            ChallengeId = challengeParticipant.ChallengeID,
            ParticipantId = challengeParticipant.ParticipantID
        });
        
        await _context.SaveChangesAsync();
        return await GetChallengeParticipantByID(challengeParticipant.ChallengeParticipantID) ?? throw new InvalidOperationException("Impossible to regist on this challenge, try again later!");
    }
    
    public async Task DeleteChallengeParticipant(int id)
    {
        var challengeParticipant = await _context.Challengeparticipants.FirstOrDefaultAsync(cp => cp.Challengeparticipant1 == id);

        if (challengeParticipant == null)
        {
            throw new ArgumentException("Challenge Participant not found");
        }

        _context.Challengeparticipants.Remove(challengeParticipant);
        await _context.SaveChangesAsync();
    }
    
     public async Task<ListChallengeParticipantModel?> GetChallengeParticipantByID(int id)
    {
        return _context.Set<Challengeparticipant>().Select(challengeParticipant => new ListChallengeParticipantModel()
        {
            ChallengeParticipantID = challengeParticipant.ChallengeId, 
            Challenge = new ListChallengeModel()
            {
                ChallengeID = challengeParticipant.Challenge.ChallengeId,
                ChallengeName = challengeParticipant.Challenge.ChallengeName,
                ChallengeDescription = challengeParticipant.Challenge.ChallengeDescription,
                StartDate = challengeParticipant.Challenge.StartDate,
                EndDate = challengeParticipant.Challenge.EndDate,
                ChallengeAchieved = challengeParticipant.Challenge.Achieved
            },
            Participant = new ListUserModel()
            {
                ID = challengeParticipant.Participant.UserId,
                Name = challengeParticipant.Participant.Name,
                DateOfBirth = challengeParticipant.Participant.Dateofbirth,
                Genre = challengeParticipant.Participant.Genre,
                Email = challengeParticipant.Participant.Email,
                Username = challengeParticipant.Participant.Username,
                Password = challengeParticipant.Participant.Password,
                Role = new RoleModel()
                {
                    ID = challengeParticipant.Participant.Role.RoleId,
                    Name = challengeParticipant.Participant.Role.RoleName
                }
            }
        }).FirstOrDefault(cp => cp.ChallengeParticipantID == id);
    }

}