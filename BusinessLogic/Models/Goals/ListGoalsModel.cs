using BusinessLogic.Models.Exercises;
using BusinessLogic.Models.Sports;
using BusinessLogic.Models.User;

namespace BusinessLogic.Models.Goals;

public class ListGoalsModel
{
    public int GoalID { get; set; }
    public ListUserModel User { get; set; }
    public ListSportsModel Sport { get; set; }
    public ListExercisesModel Exercise { get; set; }
    public string GoalName { get; set; }
    public string GoalDescription { get; set; }
    public Double? TargetValue { get; set; }
    public string TargetUnit { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public Boolean Achieved { get; set; }
}