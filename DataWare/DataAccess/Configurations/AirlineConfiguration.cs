using DataAccess.Constants;
using Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal class AirlineConfiguration : IEntityTypeConfiguration<Airline>
{
    public void Configure(EntityTypeBuilder<Airline> builder)
    {
        builder.ToTable(TableNames.Airlines);

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).IsRequired();
        builder.Property(a => a.ICAOCode).IsRequired();
        builder.Property(a => a.IATACode).IsRequired();

        builder.HasOne(a => a.Country)
            .WithMany()
            .HasForeignKey(a => a.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(Airline.GetAll());
    }
}
