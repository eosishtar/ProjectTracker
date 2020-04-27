using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectTracker.Entities
{
    public partial class PTContext : DbContext
    {
        public PTContext()
        {
        }

        public PTContext(DbContextOptions<PTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<Effort> Effort { get; set; }
        public virtual DbSet<Note> Note { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Resource> Resource { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Task> Task { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=ProjectTracker;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.Property(e => e.FileName).IsUnicode(false);

                entity.Property(e => e.FileType).IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Documents_Task");
            });

            modelBuilder.Entity<Effort>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(e => e.ContactNumber).IsUnicode(false);

                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.Effort)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.EffortId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_Effort");

                entity.HasOne(d => d.Note)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.NoteId)
                    .HasConstraintName("FK_Task_Note");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_Project");

                entity.HasOne(d => d.Resource)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.ResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_Resource");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_Status");
            });
        }
    }
}
