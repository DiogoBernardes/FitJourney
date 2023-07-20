using BusinessLogic.Models.Exercises;
using BusinessLogic.Models.Sports;
using BusinessLogic.Models.User;

namespace BusinessLogic.Models.Activities;

public class ListActivitiesModel
{
    public int ActivityID { get; set; }
    public string ActivityName { get; set; }
    public ListUserModel User { get; set; }
    public ListSportsModel Sport { get; set; }
    public ListExercisesModel Exercise { get; set; }
    public double? Duration { get; set; }
    public double? CaloriesBurned { get; set; }
    public DateOnly? ActivityDate { get; set; }
}