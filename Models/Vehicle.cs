using System;
using Microsoft.EntityFrameworkCore;
using VehicleTrackingSystem.API.Models;

namespace VehicleTrackingSystem.API.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Model { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int DealerId { get; set; }

        public Dealer Dealer { get; set; } = null!;

        public Device? Device { get; set; }
    }
}