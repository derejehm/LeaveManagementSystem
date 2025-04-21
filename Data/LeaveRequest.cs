namespace LeaveManagementSystem.Web.Data
{
    public class LeaveRequest : BaseEntitiy
    {
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public string EmployeeId { get; set; }=default!;
        public ApplicationUser? Employee { get; set; }
        public string? ApprovedById { get; set; }
        public ApplicationUser? ApprovedBy { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Comments { get; set; }
        public LeaveRequestStatus? LeaveRequestStatus { get; set; }
        public int LeaveRequestStatusId { get; set; }
    }
   
}
