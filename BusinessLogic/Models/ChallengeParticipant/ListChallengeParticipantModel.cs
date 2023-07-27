using BusinessLogic.Models.Challenges;
using BusinessLogic.Models.User;

namespace BusinessLogic.Models.ChallengeParticipant;

public class ListChallengeParticipantModel
{
    public int ChallengeParticipantID { get; set; }
    public ListChallengeModel Challenge { get; set; }
    public ListUserModel Participant { get; set; }
}