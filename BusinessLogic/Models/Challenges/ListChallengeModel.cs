using BusinessLogic.Models.Exercises;
using BusinessLogic.Models.Sports;
using BusinessLogic.Models.User;

namespace BusinessLogic.Models.Challenges;

public class ListChallengeModel
{
    public int ChallengeID { get; set; }
    public ListUserModel User { get; set; }
    public ListSportsModel Sport { get; set; }
    public ListExercisesModel Exercise { get; set; }
    public string ChallengeName { get; set; }
    public string ChallengeDescription { get; set;}
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public Boolean? ChallengeAchieved { get; set; }
}