using System;
using System.Collections.Generic;

namespace APIFirstProject.Entities.DataModels;

public partial class Contactu
{
    public long ContactusId { get; set; }

    public long UserId { get; set; }

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
