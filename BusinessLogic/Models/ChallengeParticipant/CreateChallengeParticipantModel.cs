namespace BusinessLogic.Models.ChallengeParticipant;

public class CreateChallengeParticipantModel
{
    public int ChallengeParticipantID { get; set; }
    public int ChallengeID { get; set; }
    public int ParticipantID { get; set; }
}