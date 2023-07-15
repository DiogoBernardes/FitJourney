using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public DateOnly? Dateofbirth { get; set; }

    public string? Genre { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<Challenge> Challenges { get; set; } = new List<Challenge>();

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Challenge> ChallengesNavigation { get; set; } = new List<Challenge>();
}
