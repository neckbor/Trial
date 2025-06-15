using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal class FlightSegmentConfiguration : IEntityTypeConfiguration<FlightSegment>
{
    public void Configure(EntityTypeBuilder<FlightSegment> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.FlightNumber).IsRequired();
        builder.Property(s => s.DepartureDateUtc).IsRequired();
        builder.Property(s => s.ArrivalDateUtc).IsRequired();

        builder.HasOne(s => s.Flight)
            .WithMany(f => f.Segments)
            .HasForeignKey(s => s.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Airline)
            .WithMany()
            .HasForeignKey(s => s.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.From)
            .WithMany()
            .HasForeignKey(s => s.FromAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.To)
            .WithMany()
            .HasForeignKey(s => s.ToAirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
