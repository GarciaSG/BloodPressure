using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microting.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace BloodPressureApp.API.Models;

public partial class BloodpressureContext : DbContext
{
    public BloodpressureContext()
    {
    }

    public BloodpressureContext(DbContextOptions<BloodpressureContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BloodPressure> Bloodpressures { get; set; }

    public virtual DbSet<Users> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<BloodPressure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("bloodpressures")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.UserId, "UserPressure_idx");

            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.RecordedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Bloodpressures)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserPressure");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Password)
                .HasMaxLength(60)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
