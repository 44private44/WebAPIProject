using System;
using System.Collections.Generic;

namespace APIFirstProject.Entities.DataModels;

public partial class Skill
{
    public long SkillId { get; set; }

    public string? SkillName { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? Flag { get; set; }

    public virtual ICollection<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill>();

    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
