using LeaveManagementSystem.Web.Models.LeaveType;
using LeaveManagementSystem.Web.Models.Periods;

namespace LeaveManagementSystem.Web.Models.LeaveAllocations
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }

        [Display(Name = "Number of Days")]
        public int Days { get; set; }

        [Display(Name = "Allocatioin Period")]
        public PeriodVM Period { get; set; } = new PeriodVM();

        public LeaveTypeReadOnlyVM? LeaveType { get; set; }= new LeaveTypeReadOnlyVM();
    }
}
