using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Interview.Domain.Entities.IdentityAuth;

namespace Interview.Domain.EntityFrameworkConfigurations
{
    public class CustomUserClaimConfiguration : IEntityTypeConfiguration<CustomUserClaim>
    {
        public void Configure(EntityTypeBuilder<CustomUserClaim> builder)
        {
            builder.HasKey(u => u.Id);
            builder.ToTable("CustomUserClaim");

            builder.HasOne<CustomUser>()
         .WithMany()
         .HasForeignKey(ut => ut.UserId)
         .HasPrincipalKey(cu => cu.Id);


        }
    }
}
