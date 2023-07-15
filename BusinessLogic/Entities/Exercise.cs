using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Exercise
{
    public int ExerciseId { get; set; }

    public string? ExerciseName { get; set; }

    public string? ExerciseDescription { get; set; }

    public int? SportId { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<Activityexercise> Activityexercises { get; set; } = new List<Activityexercise>();

    public virtual ICollection<Challenge> Challenges { get; set; } = new List<Challenge>();

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual Sport? Sport { get; set; }
}
