namespace BusinessLogic.Models.Activities;

public class UpdateActivityModel
{
    public int ActivityID { get; set; }
    public string ActivityName { get; set; }
    public int UserID { get; set; }
    public int SportID { get; set; }
    public int ExerciseID { get; set; }
    public double? Duration { get; set; }
    public double? CaloriesBurned { get; set; }
    public DateOnly? ActivityDate { get; set; }
}