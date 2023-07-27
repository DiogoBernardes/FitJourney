using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Challenge
{
    public int ChallengeId { get; set; }

    public int? OwnerId { get; set; }

    public int? SportId { get; set; }

    public int? ExerciseId { get; set; }

    public string? ChallengeName { get; set; }

    public string? ChallengeDescription { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? Achieved { get; set; }

    public virtual ICollection<Challengeparticipant> Challengeparticipants { get; set; } = new List<Challengeparticipant>();

    public virtual Exercise? Exercise { get; set; }

    public virtual User? Owner { get; set; }

    public virtual Sport? Sport { get; set; }
}
