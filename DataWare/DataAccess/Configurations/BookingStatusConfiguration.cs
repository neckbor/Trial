using Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal class BookingStatusConfiguration : IEntityTypeConfiguration<BookingStatus>
{
    public void Configure(EntityTypeBuilder<BookingStatus> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Code).IsRequired();
        builder.Property(s => s.Name).IsRequired();

        builder.HasData(BookingStatus.GetAll());
    }
}
