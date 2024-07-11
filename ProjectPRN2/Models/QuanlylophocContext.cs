using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectPRN2.Models;

public partial class QuanlylophocContext : DbContext
{
    public QuanlylophocContext()
    {
    }

    public QuanlylophocContext(DbContextOptions<QuanlylophocContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LopHoc> LopHocs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI\\BASKERVILLE;Initial Catalog=quanlylophoc; Trusted_Connection=SSPI;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LopHoc>(entity =>
        {
            entity.HasKey(e => e.IdlopHoc).HasName("PK__LopHoc__DB74F85B1E78464E");

            entity.ToTable("LopHoc");

            entity.Property(e => e.IdlopHoc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IDLopHoc");
            entity.Property(e => e.Gvcn)
                .HasMaxLength(255)
                .HasColumnName("GVCN");
            entity.Property(e => e.SlhocSinh).HasColumnName("SLHocSinh");
            entity.Property(e => e.TenLopHoc).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
