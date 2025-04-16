using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveType
{
    public class LeaveTypeReadOnlyVM
    {
        public int Id { get; set; }
        public string   Name { get; set; } = string.Empty;

        [Display(Name = "Maximum Allocation of Days")]
        public int NumberOfDays { get; set; }
    }

    
}
