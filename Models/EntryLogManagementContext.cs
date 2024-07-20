using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EntryManagement.Models;

public partial class EntryLogManagementContext : DbContext
{

    public virtual DbSet<AbsentReport> AbsentReports { get; set; }

    public virtual DbSet<Alert> Alerts { get; set; }

    public virtual DbSet<EntryLog> EntryLogs { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }


    // chuỗi kết nối tới database
    private string connect = "Data Source=DESKTOP-Q51CKKR\\SQLEXPRESS01;Initial Catalog=EntryLogManagement;Integrated Security=True;Trust Server Certificate=True";

    // thực hiện cấu hình
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connect);

        // tự động load
        optionsBuilder.UseLazyLoadingProxies();

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbsentReport>(entity =>
        {
            entity.HasKey(e => e.AbsentReportId).HasName("PK__AbsentRe__258DB924E376B7AC");

            entity.ToTable("AbsentReport");

            entity.Property(e => e.AbsentReportId).HasColumnName("AbsentReportID");
            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Parent).WithMany(p => p.AbsentReports)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Student).WithMany(p => p.AbsentReports).HasForeignKey(d => d.StudentId);
        });

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.AlertId).HasName("PK__Alert__EBB16AED5FF88098");

            entity.ToTable("Alert");

            entity.Property(e => e.AlertId).HasColumnName("AlertID");
            entity.Property(e => e.AlertTime).HasColumnType("datetime");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.Alerts).HasForeignKey(d => d.StudentId);
        });

        modelBuilder.Entity<EntryLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__EntryLog__5E5499A85C9F3273");

            entity.ToTable("EntryLog");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.LogTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Out");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.EntryLogs)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_EntryLog_StudentID");
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.HasKey(e => e.ParentId).HasName("PK__Parent__D339510FCBF0D5F5");

            entity.ToTable("Parent");

            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(25);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52A79FE9ED37B");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Class).HasMaxLength(5);
            entity.Property(e => e.DayOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Gender).HasMaxLength(5);
            entity.Property(e => e.Name).HasMaxLength(25);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            
            
            entity.HasOne(d => d.Parent).WithMany(p => p.Students)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__Student__ParentI__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC7805CFA5");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__UserRole__8AFACE3AC702802C");

            entity.ToTable("UserRole");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
