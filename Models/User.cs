using System;
using System.Collections.Generic;

namespace EntryManagement.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }
    
    public int? StudentID { get; set; } 

    public virtual UserRole Role { get; set; } = null!;
}
