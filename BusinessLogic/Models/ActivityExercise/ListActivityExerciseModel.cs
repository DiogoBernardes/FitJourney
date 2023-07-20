using BusinessLogic.Models.Activities;
using BusinessLogic.Models.Exercises;

namespace BusinessLogic.Models.ActivityExercise;

public class ListActivityExerciseModel
{
    public int ActivityExerciseID { get; set; }
    public ListActivitiesModel Activity { get; set; }
    public ListExercisesModel Exercise { get; set; }
    public double? Distance { get; set; }
    public double? Weight { get; set; }
    
}