using LeaveManagementSystem.Web.Commen;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Models.LeaveType;
using LeaveManagementSystem.Web.Services.LeaveAllocations;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationServices _leaveAllocationServices,ILeaveTypesService _leaveTypesService) : Controller
    {
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employees = await _leaveAllocationServices.GetEmployees();
            return View(employees);
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? Id)
        {
             await _leaveAllocationServices.AllocateLeave(Id);
            return RedirectToAction(nameof(Details),new {userId=Id});
        }

        public async Task<IActionResult> EditAllocation(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }

           var allocation= await _leaveAllocationServices.GetEmployeeAllocation(Id.Value);
            if(allocation == null)
            {
                return NotFound();
            }   

            return View(allocation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM leaveAllocationEditVM)
        { 
            if(await _leaveTypesService.DaysExceedMaximum(leaveAllocationEditVM.LeaveType.Id, leaveAllocationEditVM.Days))
            {
                ModelState.AddModelError("Days", "The allocation exceeds the maximum leave type value");

            }

            if (ModelState.IsValid)
            {
                await _leaveAllocationServices.EditAllocation(leaveAllocationEditVM);
                return RedirectToAction(nameof(Details), new { userId = leaveAllocationEditVM.Employee.Id });
            }
            var days = leaveAllocationEditVM.Days;

            leaveAllocationEditVM = await _leaveAllocationServices.GetEmployeeAllocation(leaveAllocationEditVM.Id);
            leaveAllocationEditVM.Days = days;


            return View(leaveAllocationEditVM);    

            
        }

        public async Task<IActionResult> Details(string? userId)
        {
           var employeeVm= await _leaveAllocationServices.GetEmployeeAllocations(userId);
            return View(employeeVm);
        }

       
    }
}
