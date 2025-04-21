using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.AutoMapperProfiles
{
    public class LeaveRequestAutoMapperProfile: Profile
    {
        public LeaveRequestAutoMapperProfile()
        {
            CreateMap<LeaveRequestCreateVM,LeaveRequest>();
      
        }
    }
 
}
