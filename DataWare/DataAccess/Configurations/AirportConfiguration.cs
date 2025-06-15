using DataAccess.Constants;
using Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.ToTable(TableNames.Airports);

        builder.HasKey(a => a.Id);

        builder.Property(a => a.IATACode).IsRequired();
        builder.Property(a => a.Name).IsRequired();

        builder.HasOne(a => a.City)
            .WithMany()
            .HasForeignKey(a => a.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(Airport.GetAll());
    }
}
