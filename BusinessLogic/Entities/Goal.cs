using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Goal
{
    public int GoalId { get; set; }

    public int? UserId { get; set; }

    public int? SportId { get; set; }

    public int? ExerciseId { get; set; }

    public string? GoalName { get; set; }

    public string? GoalDescription { get; set; }

    public double? TargetValue { get; set; }

    public string? TargetUnit { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool Achieved { get; set; }

    public virtual Exercise? Exercise { get; set; }

    public virtual Sport? Sport { get; set; }

    public virtual User? User { get; set; }
}
