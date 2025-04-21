using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class IdentityUserRoleConfiguration :IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {

            builder.HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = "454892ac-73a6-4cf5-9c40-c49e3cba660d",
                        UserId = "91c0052b-1143-4621-b11b-137416a78c73"
                    });
        }
    }
}
