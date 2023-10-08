using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Interview.Domain.Entities.Models;

namespace Interview.Domain.EntityFrameworkConfigurations
{
    public class SessionTypeConfiguration : IEntityTypeConfiguration<SessionType>
    {
        public void Configure(EntityTypeBuilder<SessionType> builder)
        {

            builder.HasKey(e => e.Id).HasName("PK__SessionType");

        }
    }
}
