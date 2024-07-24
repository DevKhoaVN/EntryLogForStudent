using System;
using System.Collections.Generic;

namespace EntryManagement.Models;

public partial class Parent
{
    public int ParentId { get; set; }

    public string Name { get; set; } 

    public string Email { get; set; } 

    public int Phone { get; set; } 
    public string Address { get; set; } 

    public virtual ICollection<AbsentReport> Absentreports { get; set; } = new List<AbsentReport>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
