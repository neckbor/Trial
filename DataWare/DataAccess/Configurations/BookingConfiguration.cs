using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.ExternalBookingId).IsRequired();
        builder.Property(b => b.CreatedAt).IsRequired();
        
        builder.HasOne(b => b.TicketingProvider)
            .WithMany()
            .HasForeignKey(b => b.TicketingProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Status)
            .WithMany()
            .HasForeignKey(b => b.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
