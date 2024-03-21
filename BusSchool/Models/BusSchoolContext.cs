using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BusSchool.Models
{
    public partial class BusSchoolContext : DbContext
    {
        public BusSchoolContext()
        {
        }

        public BusSchoolContext(DbContextOptions<BusSchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContactUsForm> ContactUsForms { get; set; } = null!;
        public virtual DbSet<Driver> Drivers { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Line> Lines { get; set; } = null!;
        public virtual DbSet<Passenger> Passengers { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Seat> Seats { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<Trip> Trips { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<bus> Buses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactUsForm>(entity =>
            {
                entity.Property(e => e.Message)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.Property(e => e.IsAvailable).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber).HasMaxLength(11);

                entity.Property(e => e.Ssn)
                    .HasMaxLength(14)
                    .HasColumnName("SSN");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasIndex(e => e.PassengerIdId, "IX_PassengerId_Id");

                entity.HasIndex(e => e.TripIdId, "IX_TripId_Id");

                entity.Property(e => e.Mark).HasColumnName("Mark"); ;

                entity.Property(e => e.FeedbackMessage).HasMaxLength(500);

                entity.Property(e => e.PassengerIdId).HasColumnName("PassengerId_Id");

                entity.Property(e => e.Timestamp).HasDefaultValueSql("('')");

                entity.Property(e => e.TripIdId).HasColumnName("TripId_Id");

                entity.HasOne(d => d.PassengerId)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.PassengerIdId)
                    .HasConstraintName("FK_dbo.Feedbacks_dbo.Passengers_PassengerId_Id");

                entity.HasOne(d => d.TripId)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.TripIdId)
                    .HasConstraintName("FK_dbo.Feedbacks_dbo.Trips_TripId_Id");
            });

            modelBuilder.Entity<Line>(entity =>
            {
                entity.Property(e => e.From).HasMaxLength(100);

                entity.Property(e => e.To).HasMaxLength(100);
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.HasIndex(e => e.ApplicationUserId, "IX_ApplicationUser_Id");

                entity.Property(e => e.ApplicationUserId)
                    .HasMaxLength(128)
                    .HasColumnName("ApplicationUser_Id");

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.Passengers)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Passengers_dbo.AspNetUsers_User_Id");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasIndex(e => e.BusId, "IX_Bus_Id");

                entity.HasIndex(e => e.PassengerId, "IX_PassengerId");

                entity.Property(e => e.BusId).HasColumnName("Bus_Id");

                entity.Property(e => e.Time).HasDefaultValueSql("('')");

                entity.HasOne(d => d.Bus)
                    .WithMany(p => p.Seats)
                    .HasForeignKey(d => d.BusId)
                    .HasConstraintName("FK_dbo.Seats_dbo.Buses_Bus_Id");

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.Seats)
                    .HasForeignKey(d => d.PassengerId)
                    .HasConstraintName("FK_dbo.Seats_dbo.Passengers_Passenger_Id");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasIndex(e => e.PassengerId, "IX_Passenger_Id");

                entity.HasIndex(e => e.PaymentId, "IX_PaymentId");

                entity.HasIndex(e => e.TripId, "IX_Trip_Id");

                entity.Property(e => e.PassengerId).HasColumnName("Passenger_Id");

                entity.Property(e => e.TripId).HasColumnName("Trip_Id");

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.PassengerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Tickets_dbo.Passengers_Passenger_Id");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_dbo.Tickets_dbo.Payments_Payment_Id");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.TripId)
                    .HasConstraintName("FK_dbo.Tickets_dbo.Trips_Trip_Id");
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasIndex(e => e.BusId, "IX_BusId");

                entity.HasIndex(e => e.LineId, "IX_LineId");

                entity.HasOne(d => d.Bus)
                    .WithMany(p => p.Trips)
                    .HasForeignKey(d => d.BusId)
                    .HasConstraintName("FK_dbo.Trips_dbo.Buses_Bus_Id");

                entity.HasOne(d => d.Line)
                    .WithMany(p => p.Trips)
                    .HasForeignKey(d => d.LineId)
                    .HasConstraintName("FK_dbo.Trips_dbo.Lines_Line_Id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.ProfilePicture).IsUnicode(false);

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Ssn)
                    .HasMaxLength(14)
                    .HasColumnName("SSN")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<bus>(entity =>
            {
                entity.HasIndex(e => e.DriverId, "IX_DriverId");

                entity.Property(e => e.Color).HasMaxLength(250);

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.buses)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_dbo.Buses_dbo.Drivers_Driver_Id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
