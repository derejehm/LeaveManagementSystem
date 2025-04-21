
using LeaveManagementSystem.Web.Models.LeaveAllocations;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public interface ILeaveAllocationServices
    {
        Task AllocateLeave(string employeeId);
        Task EditAllocation(LeaveAllocationEditVM leaveAllocationEditVM);
        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId);
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);
        Task<List<EmployeeListVM>> GetEmployees();

    }
}
