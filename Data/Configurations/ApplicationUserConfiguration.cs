using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(
                new ApplicationUser
                {
                    Id = "91c0052b-1143-4621-b11b-137416a78c73",
                    Email = "hailemariam013@gmail.com",
                    NormalizedEmail = "HAILEMARIAM013@GMAIL.COM",
                    UserName = "hailemariam013@gmail.com",
                    NormalizedUserName = "HAILEMARIAM013@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "Dereje@2025"),
                    EmailConfirmed = true,
                    FirstName = "Default",
                    LastName = "Admin",
                    DateOfBirth = new DateOnly(1995, 1, 1),
                });

          
        }
    }
}
