namespace BusinessLogic.Models.Goals;

public class CreateGoalModel
{
    public int GoalID { get; set; }
    public int UserID { get; set; }
    public int SportID { get; set; }
    public int ExerciseID { get; set; }
    public string GoalName { get; set; }
    public string GoalDescription { get; set; }
    public Double TargetValue { get; set; }
    public string TargetUnit { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Boolean Achieved { get; set; }
}