using DataAccess.Constants;
using Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable(TableNames.Cities);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.IATACode).IsRequired();

        builder.HasOne(c => c.Country)
            .WithMany()
            .HasForeignKey(c => c.CountryId);

        builder.HasData(City.GetAll());
    }
}
