using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Interview.Domain.Entities.IdentityAuth;

namespace Interview.Domain.EntityFrameworkConfigurations
{
    public class CustomUserLoginConfiguration : IEntityTypeConfiguration<CustomUserLogin>
    {
        public void Configure(EntityTypeBuilder<CustomUserLogin> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.ToTable("CustomUserLogins");

            builder.HasOne<CustomUser>()
           .WithMany()
          .HasForeignKey(ul => ul.UserId)
          .HasPrincipalKey(cu => cu.Id);

        }
    }
}
