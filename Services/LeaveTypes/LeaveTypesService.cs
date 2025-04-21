using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveTypes
{
    public class LeaveTypesService(ApplicationDbContext _context, IMapper _mapper) : ILeaveTypesService
    {


        public async Task<List<LeaveTypeReadOnlyVM>> GetAllLeaveTypesAsync()
        {
            var data = await _context.LeaveTypes.ToListAsync();
            var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
            return viewData;
        }
        public async Task<T?> GetLeaveTypeByIdAsync<T>(int id) where T : class
        {
            var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return null;
            }

            var viewData = _mapper.Map<T>(leaveType);

            return viewData;
        }
        public async Task CreateLeaveTypeAsync(LeaveTypeCreateVM leaveTypeCreateVM)
        {
            var leaveType = _mapper.Map<LeaveType>(leaveTypeCreateVM);
             _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateLeaveTypeAsync(LeaveTypeEditVM leaveTypeEditVM)
        {
            var leaveType = _mapper.Map<LeaveType>(leaveTypeEditVM);
            _context.LeaveTypes.Update(leaveType);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLeaveTypeAsync(int id)
        {
            var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType != null)
            {
               _context.Remove(leaveType);
                await _context.SaveChangesAsync();  
            }

        }

        public async Task<bool> DaysExceedMaximum(int leaveTypeId,int days)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(leaveTypeId);
            return leaveType.NumberOfDays < days;

        }

        public bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }

        public async Task<bool> CheckIfLeaveNameExists(string name)
        {
            return await _context.LeaveTypes.AnyAsync(e => e.Name.ToLower().Equals(name.ToLower()));
        }

        public async Task<bool> CheckIfLeaveNameExistsEdit(LeaveTypeEditVM leaveTypeEdit)
        {
            return await _context.LeaveTypes.AnyAsync(e => e.Name.ToLower().Equals(leaveTypeEdit.Name.ToLower()) && e.Id != leaveTypeEdit.Id);
        }
    }


}
