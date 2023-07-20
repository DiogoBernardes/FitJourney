namespace BusinessLogic.Models.ActivityExercise;

public class UpdateActivityExerciseModel
{
    public int ActivityExerciseID { get; set; }
    public int ActivityID { get; set; }
    public int ExerciseID { get; set; }
    public double? Distance { get; set; }
    public double? Weight { get; set; }
}