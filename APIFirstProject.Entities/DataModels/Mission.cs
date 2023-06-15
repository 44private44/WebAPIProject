using System;
using System.Collections.Generic;

namespace APIFirstProject.Entities.DataModels;

public partial class Mission
{
    public long MissionId { get; set; }

    public long ThemeId { get; set; }

    public long CityId { get; set; }

    public long CountryId { get; set; }

    public string Title { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string MissionType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? OrganizationName { get; set; }

    public string? OrganizationDetail { get; set; }

    public string Availability { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? SeatLeft { get; set; }

    public DateTime? Deadline { get; set; }

    public int? Flag { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<FavouriteMission> FavouriteMissions { get; set; } = new List<FavouriteMission>();

    public virtual ICollection<GoalMission> GoalMissions { get; set; } = new List<GoalMission>();

    public virtual ICollection<MissionApplication> MissionApplications { get; set; } = new List<MissionApplication>();

    public virtual ICollection<MissionDocument> MissionDocuments { get; set; } = new List<MissionDocument>();

    public virtual ICollection<MissionInvite> MissionInvites { get; set; } = new List<MissionInvite>();

    public virtual ICollection<MissionMedium> MissionMedia { get; set; } = new List<MissionMedium>();

    public virtual ICollection<MissionRating> MissionRatings { get; set; } = new List<MissionRating>();

    public virtual ICollection<MissionSkill> MissionSkills { get; set; } = new List<MissionSkill>();

    public virtual ICollection<Story> Stories { get; set; } = new List<Story>();

    public virtual MissionTheme Theme { get; set; } = null!;

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
