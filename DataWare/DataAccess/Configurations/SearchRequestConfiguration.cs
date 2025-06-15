using DataAccess.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal class SearchRequestConfiguration : IEntityTypeConfiguration<SearchRequest>
{
    public void Configure(EntityTypeBuilder<SearchRequest> builder)
    {
        builder.ToTable(TableNames.SearchRequests);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.AggregationStarted).IsRequired();
        builder.Property(r => r.AggregationStarted).IsRequired();
        builder.Property(r => r.DepartureDate).IsRequired();
        builder.Property(r => r.PassengerCount).IsRequired();
        builder.Property(r => r.CreatedAtUtc).IsRequired();
        builder.Property(r => r.SearchResultKey).IsRequired();

        builder.HasOne(r => r.From)
            .WithMany()
            .HasForeignKey(r => r.FromAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.To)
            .WithMany()
            .HasForeignKey(r => r.ToAirportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
