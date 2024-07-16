using System;
using System.Collections.Generic;

namespace EntryManagement.Models;

public partial class Alert
{
    public int AlertId { get; set; }

    public int StudentId { get; set; }

    public DateTime AlertTime { get; set; }

    public virtual Student Student { get; set; } = null!;
}
