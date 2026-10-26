using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleTrackingSystem.API.Data;
using VehicleTrackingSystem.API.DTOs;
using VehicleTrackingSystem.API.Models;


namespace VehicleTrackingSystem.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dealer> Dealers => Set<Dealer>();

        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        public DbSet<Device> Devices => Set<Device>();

        public DbSet<DeviceLocation> DeviceLocations
            => Set<DeviceLocation>();

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dealer -> Vehicles
            modelBuilder.Entity<Dealer>()
                .HasMany(d => d.Vehicles)
                .WithOne(v => v.Dealer)
                .HasForeignKey(v => v.DealerId);

            // Vehicle -> Device
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Device)
                .WithOne(d => d.Vehicle)
                .HasForeignKey<Device>(d => d.VehicleId);

            // Device -> Locations
            modelBuilder.Entity<Device>()
                .HasMany(d => d.Locations)
                .WithOne(l => l.Device)
                .HasForeignKey(l => l.DeviceId);

            // DeviceId must be unique
            modelBuilder.Entity<Device>()
                .HasIndex(d => d.DeviceId)
                .IsUnique();

            // Username must be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}