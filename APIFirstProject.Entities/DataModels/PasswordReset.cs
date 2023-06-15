using System;
using System.Collections.Generic;

namespace APIFirstProject.Entities.DataModels;

public partial class PasswordReset
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? Linkactivate { get; set; }
}
