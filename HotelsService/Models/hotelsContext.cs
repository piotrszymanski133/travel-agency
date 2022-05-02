using System;
using System.Collections.Generic;
using HotelsService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HotelsService
{
    public partial class hotelsContext : DbContext
    {
        public hotelsContext()
        {
        }

        public hotelsContext(DbContextOptions<hotelsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Destination> Destinations { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Eventroom> Eventrooms { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<Hotelroom> Hotelrooms { get; set; } = null!;
        public virtual DbSet<Hotelroomtype> Hotelroomtypes { get; set; } = null!;
        public virtual DbSet<Tour> Tours { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=postgres;Database=hotels;Username=user;Password=example");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Destination>(entity =>
            {
                entity.ToTable("destinations");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .HasColumnName("country");

            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("events");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.Creationtime)
                    .HasColumnName("creationtime")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.StartDate)
                    .HasColumnName("startdate");

                entity.Property(e => e.EndDate)
                    .HasColumnName("enddate");

                entity.Property(e => e.HotelId)
                    .HasMaxLength(36)
                    .HasColumnName("hotel_id");

                entity.Property(e => e.Type)
                    .HasMaxLength(16)
                    .HasColumnName("type");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("hotel_id");
            });

            modelBuilder.Entity<Eventroom>(entity =>
            {
                entity.ToTable("eventrooms");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.RoomtypeId).HasColumnName("roomtype_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Eventrooms)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("event_id");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("hotels");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("id");

                entity.Property(e => e.DestinationId).HasColumnName("destination_id");

                entity.Property(e => e.Food)
                    .HasMaxLength(36)
                    .HasColumnName("food");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Stars).HasColumnName("stars");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("destination_id");
            });

            modelBuilder.Entity<Hotelroom>(entity =>
            {
                entity.ToTable("hotelrooms");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.HotelId)
                    .HasMaxLength(36)
                    .HasColumnName("hotel_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.RoomtypeId).HasColumnName("roomtype_id");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Hotelrooms)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("hotel_id");

                entity.HasOne(d => d.Roomtype)
                    .WithMany(p => p.Hotelrooms)
                    .HasForeignKey(d => d.RoomtypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("roomtype_id");
            });

            modelBuilder.Entity<Hotelroomtype>(entity =>
            {
                entity.ToTable("hotelroomtypes");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CapacityPeople).HasColumnName("capacity_people");

                entity.Property(e => e.Name)
                    .HasMaxLength(36)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.ToTable("tours");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("id");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .HasColumnName("country");

                entity.Property(e => e.Food)
                    .HasMaxLength(36)
                    .HasColumnName("food");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Rating).HasColumnName("rating");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
