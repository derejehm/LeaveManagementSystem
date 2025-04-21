namespace LeaveManagementSystem.Web.Data
{
    public class LeaveRequestStatus : BaseEntitiy
    {
        [StringLength(50)]
        public string Name { get; set; }
   
    }
  
}