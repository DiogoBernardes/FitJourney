using System;
using System.Collections.Generic;

namespace BusinessLogic.Entities;

public partial class Challengeparticipant
{
    public int Challengeparticipant1 { get; set; }

    public int ChallengeId { get; set; }

    public int? ParticipantId { get; set; }

    public virtual Challenge? Challenge { get; set; }

    public virtual User? Participant { get; set; }
}
