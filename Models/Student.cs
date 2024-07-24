using System;
using System.Collections.Generic;

namespace EntryManagement.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int ParentId { get; set; }

    public string Name { get; set; } 

    public string? Gender { get; set; }

    public DateTime DayOfBirth { get; set; }

    public string Class { get; set; } 

    public string Address { get; set; } 

    public int Phone { get; set; }

    public DateTime JoinDay { get; set; }

    public virtual ICollection<AbsentReport> Absentreports { get; set; } = new List<AbsentReport>();

    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();

    public virtual ICollection<EntryLog> Entrylogs { get; set; } = new List<EntryLog>();

    public virtual Parent Parent { get; set; } = null!;
}
