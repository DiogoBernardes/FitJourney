namespace BusinessLogic.Models.Challenges;

public class CreateChallengeModel
{
    public int OwnerID { get; set; }
    public int SportID { get; set; }
    public int ExerciseID { get; set; }
    public string ChallengeName { get; set; }
    public string ChallengeDescription { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public Boolean? ChallengeAchieved { get; set; }
}