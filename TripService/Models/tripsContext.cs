using Microsoft.EntityFrameworkCore;

namespace TripService.Models
{
    public partial class tripsContext : DbContext
    {
        public tripsContext()
        {
        }

        public tripsContext(DbContextOptions<tripsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Orderedtrip> Orderedtrips { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("host=postgres;Database=trips;Username=user;Password=example");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orderedtrip>(entity =>
            {
                entity.HasKey(e => e.TripId)
                    .HasName("orderedtrips_pkey");

                entity.ToTable("orderedtrips");

                entity.Property(e => e.TripId)
                    .ValueGeneratedNever()
                    .HasColumnName("trip_id");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .HasColumnName("country");

                entity.Property(e => e.Food)
                    .HasMaxLength(36)
                    .HasColumnName("food");

                entity.Property(e => e.Persons).HasColumnName("persons");

                entity.Property(e => e.RoomTypeName)
                    .HasMaxLength(32)
                    .HasColumnName("room_type_name");

                entity.Property(e => e.TransportTypeName)
                    .HasMaxLength(16)
                    .HasColumnName("transport_type_name");

                entity.Property(e => e.Username)
                    .HasMaxLength(32)
                    .HasColumnName("username");
                
                entity.Property(e => e.StartDate)
                    .HasColumnName("startdate");

                entity.Property(e => e.EndDate)
                    .HasColumnName("enddate");
                
                entity.Property(e => e.HotelName)
                    .HasMaxLength(255)
                    .HasColumnName("hotel_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
