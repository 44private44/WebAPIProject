using System;
using System.Collections.Generic;

namespace APIFirstProject.Entities.DataModels;

public partial class MissionTheme
{
    public long MissionThemeId { get; set; }

    public string? Title { get; set; }

    public byte Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? Flag { get; set; }

    public virtual ICollection<Mission> Missions { get; set; } = new List<Mission>();
}
