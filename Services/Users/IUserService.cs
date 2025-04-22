

namespace LeaveManagementSystem.Web.Services.Users
{
    public interface IUserService
    {
        Task<ApplicationUser> GetLoggedInUser();
        Task<ApplicationUser> GetUserById(string id);
        Task<List<ApplicationUser>> GetEmployee();
    }
}
