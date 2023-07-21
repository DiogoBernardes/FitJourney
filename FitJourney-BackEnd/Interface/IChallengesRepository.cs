using BusinessLogic.Models.Challenges;

namespace FitJourney_BackEnd.Interface;

public interface IChallengesRepository
{
    Task<List<ListChallengeModel>> GetChallenges();
    Task<ListChallengeModel> CreateChallenge(CreateChallengeModel challenge);
    Task UpdateChallenge(UpdateChallengeModel challenge);
    Task DeleteChallenge(int id);
    Task<ListChallengeModel?> GetChallengeByName(string name);
}