using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveType;

namespace LeaveManagementSystem.Web.AutoMapperProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();
            CreateMap<LeaveTypeCreateVM,LeaveType>();  
            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap();

        }
    }
    
}
