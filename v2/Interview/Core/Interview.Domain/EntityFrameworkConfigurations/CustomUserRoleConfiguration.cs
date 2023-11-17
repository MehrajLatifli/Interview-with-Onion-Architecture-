using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Interview.Domain.Entities.IdentityAuth;

namespace Interview.Domain.EntityFrameworkConfigurations
{


    public class CustomUserRoleConfiguration : IEntityTypeConfiguration<CustomUserRole>
    {
        public void Configure(EntityTypeBuilder<CustomUserRole> builder)
        {
            builder.HasKey(r => new { r.UserId, r.RoleId });
            builder.ToTable("CustomUserRoles");


            builder.HasOne<CustomUser>()
          .WithMany()
         .HasForeignKey(ul => ul.UserId)
         .HasPrincipalKey(cu => cu.Id);

            builder.HasOne<CustomRole>()
           .WithMany()
           .HasForeignKey(ul => ul.RoleId)
           .HasPrincipalKey(cu => cu.Id);
        }
    }

}
