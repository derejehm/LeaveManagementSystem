using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveType
{
    public class LeaveTypeEditVM
    {
        public int Id { get; set; }

        [Required]
        [Length(5, 150, ErrorMessage = "Name must be between 5 and 150 characters")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 90)]
        [Display(Name = "Maximum Allocation of Days")]
        public int NumberOfDays { get; set; }
    }
}
