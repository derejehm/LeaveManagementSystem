using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .HasData(
                new IdentityRole
                {
                    Id = "454892ac-73a6-4cf5-9c40-c49e3cba660d",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },

                new IdentityRole
                {
                    Id= "85529c34-d8ab-4d52-a5e0-48875a4ba457",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },

                new IdentityRole
                {
                    Id= "71e66fc7-6328-4782-98d2-34233e3822e5",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                });

            
            var hasher = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "91c0052b-1143-4621-b11b-137416a78c73",
                    Email = "hailemariam013@gmail.com",
                    NormalizedEmail = "HAILEMARIAM013@GMAIL.COM",
                    UserName = "hailemariam013@gmail.com",
                    NormalizedUserName= "HAILEMARIAM013@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "Dereje@2025"),
                    EmailConfirmed =true,
                    FirstName="Default",
                    LastName = "Admin",
                    DateOfBirth=new DateOnly(1995, 1, 1),
                });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "454892ac-73a6-4cf5-9c40-c49e3cba660d",
                    UserId = "91c0052b-1143-4621-b11b-137416a78c73"
                }
                );


        }

        public DbSet<LeaveType> LeaveTypes { get; set; }
    }
}
