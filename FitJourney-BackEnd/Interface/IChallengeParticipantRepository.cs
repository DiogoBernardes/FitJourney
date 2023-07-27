using BusinessLogic.Models.ChallengeParticipant;

namespace FitJourney_BackEnd.Interface;

public interface IChallengeParticipantRepository
{

    Task<List<ListChallengeParticipantModel>> GetChallengeParticipants();
    Task<ListChallengeParticipantModel> CreateChallengeParticipant(CreateChallengeParticipantModel challengeParticipant);
    Task DeleteChallengeParticipant(int id);
    Task<ListChallengeParticipantModel?> GetChallengeParticipantByID(int id);

}