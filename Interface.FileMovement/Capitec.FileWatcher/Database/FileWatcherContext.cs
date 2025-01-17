using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Interface.FileMovement.Database
{
    public partial class FileWatcherContext : DbContext
    {
        public FileWatcherContext()
        {
        }

        public FileWatcherContext(DbContextOptions<FileWatcherContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FileMovementError> FileMovementErrors { get; set; }
        public virtual DbSet<FileMovementHistory> FileMovementHistories { get; set; }
        public virtual DbSet<FileMovementSetting> FileMovementSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Bootstrapper.Configuration["ParameterDBConnection"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<FileMovementError>(entity =>
            {
                entity.ToTable("FileMovement_Error");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.ErrorSource)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.FileMovementSettingsId).HasColumnName("FileMovement_Settings_ID");

                entity.HasOne(d => d.FileMovementSettings)
                    .WithMany(p => p.FileMovementErrors)
                    .HasForeignKey(d => d.FileMovementSettingsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FileMovem__FileM__056ECC6A");
            });

            modelBuilder.Entity<FileMovementHistory>(entity =>
            {
                entity.ToTable("FileMovement_History");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateMoved).HasColumnType("datetime");

                entity.Property(e => e.FileMovementSettingsId).HasColumnName("FileMovement_Settings_ID");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FileMovementSettings)
                    .WithMany(p => p.FileMovementHistories)
                    .HasForeignKey(d => d.FileMovementSettingsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FileMovem__FileM__019E3B86");
            });

            modelBuilder.Entity<FileMovementSetting>(entity =>
            {
                entity.ToTable("FileMovement_Settings");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AntiFileMask)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ArchivePath)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CopyPath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Enabled)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FileMask)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MailBcc)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MailTo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Mailcc)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Priority).HasDefaultValueSql("((1))");

                entity.Property(e => e.SourcePath)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
