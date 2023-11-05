using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Interview.Domain.Entities.IdentityAuth;

namespace Interview.Domain.EntityFrameworkConfigurations
{
    public class CustomRoleClaimConfiguration : IEntityTypeConfiguration<CustomRoleClaim>
    {
        public void Configure(EntityTypeBuilder<CustomRoleClaim> builder)
        {
            builder.HasKey(u => u.Id);
            builder.ToTable("CustomRoleClaim");

            builder.HasOne<CustomRole>()
         .WithMany()
         .HasForeignKey(ut => ut.RoleId)
         .HasPrincipalKey(cu => cu.Id);


        }
    }


}
