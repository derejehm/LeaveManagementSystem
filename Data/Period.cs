namespace LeaveManagementSystem.Web.Data
{
    public class Period :BaseEntitiy
    {
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }
    }
}
