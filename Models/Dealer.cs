using System;
using Microsoft.EntityFrameworkCore;
using VehicleTrackingSystem.API.Models;
using System.Collections.Generic;

namespace VehicleTrackingSystem.API.Models
{
    public class Dealer
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
            = new List<Vehicle>();
    }
}