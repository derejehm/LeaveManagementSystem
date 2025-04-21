using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class LeaveRequestCreateVM :IValidatableObject
    {
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateOnly StartDate { get; set; }
        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateOnly EndDate { get; set; }

        [DisplayName("Desired Leave Type")]
        [Required]
        public int LeaveTypeId { get; set; }

        [DisplayName("Additional Information")]
        public string? Comments { get; set; }

        public SelectList? LeaveTypes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(StartDate > EndDate)
            {
                yield return new ValidationResult("The Start date must be  before  End date", [nameof(StartDate),nameof(EndDate)]);
            }
        }
    }
}
