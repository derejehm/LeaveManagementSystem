using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task<List<LeaveRequestListVM>> GetEmployeeLeaveRequests();   
        Task<EmployeeLeaveRequestListVM> GetAllLeaveRequests();

        Task CancelLeaveRequest(int leaveRequestId);

        Task ReviewLeaveRequest(int id , bool approved);

        Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model);
        Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id);
    }
}
