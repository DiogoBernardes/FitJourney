using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Achievement
{
    public int AchievementId { get; set; }

    public int? UserId { get; set; }

    public string? AchievementName { get; set; }

    public string? AchievementDescription { get; set; }

    public DateOnly? AchievedDate { get; set; }

    public virtual User? User { get; set; }
}
