using DataAccess.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable(TableNames.Flights);

        builder.HasKey(f => f.Id);

        builder.Property(f => f.DepartureDate).IsRequired();
        builder.Property(f => f.ArrivalDate).IsRequired();
        builder.Property(f => f.TotalPrice).IsRequired();

        builder.HasOne(f => f.From)
            .WithMany()
            .HasForeignKey(f => f.FromAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.To)
            .WithMany()
            .HasForeignKey(f => f.ToAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.TicketingProvider)
            .WithMany()
            .HasForeignKey(f => f.TicketingProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Booking)
            .WithMany()
            .HasForeignKey(f => f.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
