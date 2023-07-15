using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Activityexercise
{
    public int ActivityExerciseId { get; set; }

    public int? ActivityId { get; set; }

    public int? ExerciseId { get; set; }

    public double? Distance { get; set; }

    public double? Weight { get; set; }

    public virtual Activity? Activity { get; set; }

    public virtual Exercise? Exercise { get; set; }
}
