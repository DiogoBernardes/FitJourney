using BusinessLogic.Models.User;

namespace BusinessLogic.Models.Achievement;

public class ListAchievementModel
{
    public int AchievementID { get; set; }
    public ListUserModel User { get; set; }
    public string AchievementName { get; set; }
    public string AchievementDescription { get; set; }
    public DateOnly AchievedDate { get; set; }
}