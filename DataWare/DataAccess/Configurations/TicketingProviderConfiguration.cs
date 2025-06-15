using Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

internal class TicketingProviderConfiguration : IEntityTypeConfiguration<TicketingProvider>
{
    public void Configure(EntityTypeBuilder<TicketingProvider> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code).IsRequired();
        builder.Property(p => p.Name).IsRequired();

        builder.HasData(TicketingProvider.GetAll());
    }
}
