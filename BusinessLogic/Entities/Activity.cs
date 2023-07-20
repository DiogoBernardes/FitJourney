using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Activity
{
    public int ActivityId { get; set; }

    public string? ActivityName { get; set; }

    public int? UserId { get; set; }

    public int? SportId { get; set; }

    public int? ExerciseId { get; set; }

    public double? Duration { get; set; }

    public double? CaloriesBurned { get; set; }

    public DateOnly? ActivityDate { get; set; }

    public virtual ICollection<Activityexercise> Activityexercises { get; set; } = new List<Activityexercise>();

    public virtual Exercise? Exercise { get; set; }

    public virtual Sport? Sport { get; set; }

    public virtual User? User { get; set; }
}
