using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
           builder.HasData(
                new IdentityRole
                {
                    Id = "454892ac-73a6-4cf5-9c40-c49e3cba660d",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },

                new IdentityRole
                {
                    Id = "85529c34-d8ab-4d52-a5e0-48875a4ba457",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },

                new IdentityRole
                {
                    Id = "71e66fc7-6328-4782-98d2-34233e3822e5",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                });

        }
    }
}
