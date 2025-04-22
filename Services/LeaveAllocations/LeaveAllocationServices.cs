
using AutoMapper;
using LeaveManagementSystem.Web.Commen;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Services.Periods;
using LeaveManagementSystem.Web.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationServices(ApplicationDbContext _context,
        IUserService _userService,
        IMapper _mapper,
        IPeriodsService  _periodsService
        ) : ILeaveAllocationServices
    {

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var user = await _userService.GetEmployee();
            var employees= _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(user.ToList());

            return employees;

        }

        public async Task AllocateLeave(string employeeId)
        {
            // get all leave types
            var leaveTypes = await _context.LeaveTypes
                .Where(q => q.LeaveAllocations != null && !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();

            // get the current period based on the current year
           
            var period = await _periodsService.GetCurrentPeriod();

            var monthRemaining = period.EndDate.Month - DateTime.Now.Month;

            // foreach leave type create an allocation entity
            foreach (var leaveType in leaveTypes)
            {
                var accuralRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    PeriodId = period.Id,
                    LeaveTypeId = leaveType.Id,
                    Days = (int)Math.Ceiling(accuralRate * monthRemaining),
                };

                // check if the allocation already exists
                var existingAllocation = await _context.LeaveAllocations
                    .AnyAsync(l => l.EmployeeId == employeeId && l.PeriodId == period.Id && l.LeaveTypeId == leaveType.Id);
                if (!existingAllocation)
                {
                    _context.Add(leaveAllocation);
                }
            }

            await _context.SaveChangesAsync();
        }

     

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(String? userId)
        {
            var user=string.IsNullOrEmpty(userId)
                ? await _userService.GetLoggedInUser()
                : await _userService.GetUserById(userId);

            var  allocations = await GetLeaveAllocations(user.Id);
            var allocationVmList=_mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();

            var employeeVm = new EmployeeAllocationVM
            {

                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id,
                DateOfBirth = user.DateOfBirth,
                LeaveAllocations = allocationVmList,
                IsCompletedAllocation = leaveTypesCount == allocations.Count
            };

            return employeeVm;

        }

        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
        {
            var allocation= await _context.LeaveAllocations
                          .Include(q=>q.LeaveType)
                          .Include(q => q.Employee)
                          .FirstOrDefaultAsync(q => q.Id == allocationId);

            var model= _mapper.Map<LeaveAllocationEditVM>(allocation);

            return model;
        }
        public async Task EditAllocation(LeaveAllocationEditVM leaveAllocationEditVM)
        {
            await _context.LeaveAllocations
                .Where(q=> q.Id==leaveAllocationEditVM.Id)
                .ExecuteUpdateAsync(q => q.SetProperty(x => x.Days, leaveAllocationEditVM.Days));
        }

        public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
        {
            var currentDate = DateTime.Now;
            var period = await _periodsService.GetCurrentPeriod();

            var allocation = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Period)
                .FirstAsync(q => q.EmployeeId == employeeId 
                && q.PeriodId == period.Id 
                && q.LeaveTypeId == leaveTypeId);

            return allocation;
        }


        private async Task<List<LeaveAllocation>> GetLeaveAllocations(string? userId)
        {
            //var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var currentDate = DateTime.Now;
        

            var leaveAllocations = await _context.LeaveAllocations
                .Include(l => l.LeaveType)
                .Include(l => l.Period)
                .Where(l => l.EmployeeId == userId && l.Period.EndDate.Year == currentDate.Year)
                .ToListAsync();
            return leaveAllocations;
        }
    }
}
