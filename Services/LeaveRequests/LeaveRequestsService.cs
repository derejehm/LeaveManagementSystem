using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveAllocations;
using LeaveManagementSystem.Web.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public class LeaveRequestsService(IMapper _mapper, IUserService _userService,
        ILeaveAllocationServices _leaveAllocationServices,
        ApplicationDbContext _context) : ILeaveRequestsService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;

          // restore  allocation  days based on request
            await UpdateAllocationDays(leaveRequest, false);
            await _context.SaveChangesAsync();

        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);
            var user = await _userService.GetLoggedInUser();
            leaveRequest.EmployeeId = user.Id;
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;
            _context.Add(leaveRequest);


            // deduct allocation days based on request
            await UpdateAllocationDays(leaveRequest, true);

            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeLeaveRequestListVM> GetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                 .Include(q => q.LeaveType)
                 .ToListAsync();

            var leaveRequest = leaveRequests.Select(q => new LeaveRequestListVM
            {
                Id = q.Id,
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                LeaveType = q.LeaveType.Name,
                NumberOfDays = (q.EndDate.DayNumber - q.StartDate.DayNumber),
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId
            }).ToList();

            var model = new EmployeeLeaveRequestListVM
            {
                ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved),
                PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending),
                RejectedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Rejected),
                TotalRequests = leaveRequests.Count,
                LeaveRequests = leaveRequest
            };

            return model;
        }

        public async Task<List<LeaveRequestListVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userService.GetLoggedInUser();
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            var model = leaveRequests.Select(q => new LeaveRequestListVM
            {
                Id = q.Id,
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                LeaveType = q.LeaveType.Name,
                NumberOfDays = (q.EndDate.DayNumber - q.StartDate.DayNumber),
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId
            }).ToList();

            return model;
        }

        public async Task ReviewLeaveRequest(int id, bool approved)
        {

            var user = await _userService.GetLoggedInUser();
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            leaveRequest.LeaveRequestStatusId = approved ?
                (int)LeaveRequestStatusEnum.Approved :
                (int)LeaveRequestStatusEnum.Rejected;

            leaveRequest.ApprovedById = user.Id;

            if (!approved)
            {
              await UpdateAllocationDays(leaveRequest, false);
            }

            await _context.SaveChangesAsync();
        }



        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);

            var user = await _userService.GetLoggedInUser();
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocation = await _context.LeaveAllocations
                .FirstAsync(q => q.EmployeeId == user.Id
                && q.LeaveTypeId == model.LeaveTypeId
                && q.PeriodId == period.Id);

            return allocation.Days < numberOfDays;
        }

        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        {
            var leaveRequests = await _context.LeaveRequests
             .Include(q => q.LeaveType)
             .FirstAsync(q => q.Id == id);

          
            var user = await _userService.GetUserById(leaveRequests.EmployeeId);

            var model = new ReviewLeaveRequestVM
            {
                Id = leaveRequests.Id,
                StartDate = leaveRequests.StartDate,
                EndDate = leaveRequests.EndDate,
                LeaveType = leaveRequests.LeaveType.Name,
                NumberOfDays = (leaveRequests.EndDate.DayNumber - leaveRequests.StartDate.DayNumber),
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequests.LeaveRequestStatusId,
                RequestComments = leaveRequests.Comments,
                Employee = new EmployeeListVM
                {
                    Id = leaveRequests.EmployeeId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,

                }
            };

            return model;


        }

        private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _leaveAllocationServices.GetCurrentAllocation(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);

            var numberOfDays = CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);

            if (deductDays)
            {
                allocation.Days -= numberOfDays;
            }
            else
            {
                allocation.Days += numberOfDays;
            }

            _context.Entry(allocation).State = EntityState.Modified;

        }

        private int CalculateDays(DateOnly startDate, DateOnly endDate)
        {
            return endDate.DayNumber - startDate.DayNumber;
        }
    }
}
