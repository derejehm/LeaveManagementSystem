using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data
{
    public class LeaveType
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public String Name { get; set; }
        public int NumberOfDays { get; set; }

        public List<LeaveAllocation>? LeaveAllocations { get; set; } 
    }
}
