using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



DataServicesRegistertion.AddDataServices(builder.Services, builder.Configuration);
ApplicationServicesRegisteration.AddApplicationServices(builder.Services);

builder.Host.UseSerilog((context, config) => 
     config
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration)
    );  

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminSupervisorOnly", policy => {
        policy.RequireRole(Roles.Administrator, Roles.Supervisor);
        });
});


builder.Services.AddHttpContextAccessor();



builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
