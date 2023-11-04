using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Interview.Domain.Entities.IdentityAuth;

namespace Interview.Domain.EntityFrameworkConfigurations
{
    public class CustomUserTokenConfiguration : IEntityTypeConfiguration<CustomUserToken>
    {
        public void Configure(EntityTypeBuilder<CustomUserToken> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.ToTable("CustomUserTokens");

            
            builder.HasOne<CustomUser>()
                .WithMany()  
                .HasForeignKey(ut => ut.UserId)
                .HasPrincipalKey(cu => cu.Id);

        }
    }
}
