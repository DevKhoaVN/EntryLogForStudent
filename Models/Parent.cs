using System;
using System.Collections.Generic;

namespace EntryManagement.Models;

public partial class Parent
{
    public int ParentId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<AbsentReport> Absentreports { get; set; } = new List<AbsentReport>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
