using System;
using System.Collections.Generic;

namespace APIFirstProject.Entities.DataModels;

public partial class MissionDatum
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
}
