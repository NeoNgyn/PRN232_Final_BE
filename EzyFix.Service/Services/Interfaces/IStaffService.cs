using EzyFix.DAL.Data.Requests.Paging;
using EzyFix.DAL.Data.Requests.Staff;
using EzyFix.DAL.Data.Responses.Paging;
using EzyFix.DAL.Data.Responses.Staff;

namespace EzyFix.BLL.Services.Interfaces
{
    // define cac method CRUD cho Staff
    public interface IStaffService
    {
        // B2: Tao method CRUD cho Staff
        Task<CreateStaffResponse> CreateStaff(CreateStaffRequest createStaffRequest);
        Task<CreateStaffResponse> GetStaffById(Guid id);
        Task<IEnumerable<CreateStaffResponse>> GetStaffs();
        Task<UpdateStaffResponse> UpdateStaff(Guid id, UpdateStaffRequest updateStaffRequest);
        Task<bool> DeleteStaff(Guid id);
        Task<AssignStaffResponse> AssignStaff(Guid id ,AssignStaffRequest assignStaffRequest);
        Task<RemoveStaffResponse> RemoveStaff(Guid id ,RemoveStaffRequest removeStaffRequest);
        Task<PagingResponse<CreateStaffResponse>> GetPagedStaffs(PagingRequest pagingRequest);
    }
}
