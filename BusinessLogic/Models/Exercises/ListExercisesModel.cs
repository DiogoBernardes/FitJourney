using BusinessLogic.Models.Sports;


namespace BusinessLogic.Models.Exercises;

public class ListExercisesModel
{
    public int ExerciseID { get; set; }
    public string ExerciseName { get; set; }
    public string ExerciseDescription { get; set; }
    public ListSportsModel Sport { get; set; }
}