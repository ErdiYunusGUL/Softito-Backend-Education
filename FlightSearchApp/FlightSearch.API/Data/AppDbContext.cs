using FlightSearch.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.API.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Airline> Airlines { get; set; }
    public DbSet<Airport> Airports { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Price Precision
        modelBuilder.Entity<Flight>()
            .Property(f => f.Price)
            .HasColumnType("decimal(18,2)");

        // Flight -> Airport relationships
        modelBuilder.Entity<Flight>()
            .HasOne(f => f.DepartureAirport)
            .WithMany()
            .HasForeignKey(f => f.DepartureAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.ArrivalAirport)
            .WithMany()
            .HasForeignKey(f => f.ArrivalAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Data
        modelBuilder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "Turkey", Code = "TR" }
        );

        modelBuilder.Entity<City>().HasData(
            new City { Id = 1, Name = "Istanbul", CountryId = 1 },
            new City { Id = 2, Name = "Ankara", CountryId = 1 },
            new City { Id = 3, Name = "Izmir", CountryId = 1 },
            new City { Id = 4, Name = "Antalya", CountryId = 1 }
        );

        modelBuilder.Entity<Airline>().HasData(
            new Airline { Id = 1, Name = "Turkish Airlines" },
            new Airline { Id = 2, Name = "Pegasus Airlines" },
            new Airline { Id = 3, Name = "SunExpress" }
        );

        modelBuilder.Entity<Airport>().HasData(
            new Airport { Id = 1, Name = "Istanbul Airport", CityId = 1, Code = "IST" },
            new Airport { Id = 2, Name = "Sabiha Gokcen Airport", CityId = 1, Code = "SAW" },
            new Airport { Id = 3, Name = "Esenboga Airport", CityId = 2, Code = "ESB" },
            new Airport { Id = 4, Name = "Adnan Menderes Airport", CityId = 3, Code = "ADB" },
            new Airport { Id = 5, Name = "Antalya Airport", CityId = 4, Code = "AYT" }
        );

        modelBuilder.Entity<Flight>().HasData(
            new Flight { Id = 1, AirlineId = 1, DepartureAirportId = 1, ArrivalAirportId = 3, DepartureTime = new DateTime(2026, 8, 1, 10, 0, 0), Price = 1500m },
            new Flight { Id = 2, AirlineId = 2, DepartureAirportId = 2, ArrivalAirportId = 4, DepartureTime = new DateTime(2026, 8, 2, 14, 30, 0), Price = 1200m },
            new Flight { Id = 3, AirlineId = 1, DepartureAirportId = 3, ArrivalAirportId = 1, DepartureTime = new DateTime(2026, 8, 3, 9, 15, 0), Price = 1600m },
            new Flight { Id = 4, AirlineId = 3, DepartureAirportId = 4, ArrivalAirportId = 5, DepartureTime = new DateTime(2026, 8, 1, 18, 45, 0), Price = 900m },
            new Flight { Id = 5, AirlineId = 2, DepartureAirportId = 5, ArrivalAirportId = 2, DepartureTime = new DateTime(2026, 8, 2, 21, 0, 0), Price = 1100m }
        );
    }
}
