using LeaveManagementSystem.Web.Models.LeaveType;

namespace LeaveManagementSystem.Web.Services.LeaveTypes
{
    public interface ILeaveTypesService
    {
        Task<bool> CheckIfLeaveNameExists(string name);
        Task<bool> CheckIfLeaveNameExistsEdit(LeaveTypeEditVM leaveTypeEdit);
        Task CreateLeaveTypeAsync(LeaveTypeCreateVM leaveTypeCreateVM);
        Task<bool> DaysExceedMaximum(int leaveTypeId, int days);
        Task DeleteLeaveTypeAsync(int id);
        Task<List<LeaveTypeReadOnlyVM>> GetAllLeaveTypesAsync();
        Task<T?> GetLeaveTypeByIdAsync<T>(int id) where T : class;
        bool LeaveTypeExists(int id);
        Task UpdateLeaveTypeAsync(LeaveTypeEditVM leaveTypeEditVM);
    }
}