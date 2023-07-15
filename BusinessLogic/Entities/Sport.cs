using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Sport
{
    public int SportId { get; set; }

    public string? SportName { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<Challenge> Challenges { get; set; } = new List<Challenge>();

    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
}
