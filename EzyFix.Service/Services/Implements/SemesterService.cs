using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Semesters;
using EzyFix.DAL.Data.Responses.Semesters;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EzyFix.BLL.Services.Implements
{
    public class SemesterService : BaseService<SemesterService>, ISemesterService
    {
        public SemesterService(
     IUnitOfWork<AppDbContext> unitOfWork,
     ILogger<SemesterService> logger,
     IMapper mapper,
     IHttpContextAccessor httpContextAccessor) // <-- 1. Thêm tham số vào đây
     : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<SemesterResponse>> GetAllSemestersAsync()
        {
            try
            {
                var semesters = await _unitOfWork.GetRepository<Semester>().GetListAsync();
                return _mapper.Map<IEnumerable<SemesterResponse>>(semesters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách học kỳ: {Message}", ex.Message);
                throw; // Ném lỗi để middleware hoặc controller xử lý
            }
        }

        public async Task<SemesterResponse?> GetSemesterByIdAsync(Guid id)
        {
            try
            {
                var semester = await _unitOfWork.GetRepository<Semester>().SingleOrDefaultAsync(predicate: s => s.SemesterId == id);

                if (semester == null)
                {
                    // Ném một exception cụ thể để controller bắt và trả về 404
                    throw new NotFoundException($"Không tìm thấy học kỳ với ID: {id}");
                }

                return _mapper.Map<SemesterResponse>(semester);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy học kỳ theo ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<SemesterResponse> CreateSemesterAsync(CreateSemesterRequest createDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var semester = _mapper.Map<Semester>(createDto);

                    // 1. Tự sinh Guid mới cho ID
                    semester.SemesterId = Guid.NewGuid();

                    await _unitOfWork.GetRepository<Semester>().InsertAsync(semester);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<SemesterResponse>(semester);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo học kỳ mới: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<SemesterResponse> UpdateSemesterAsync(Guid id, UpdateSemesterRequest updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    // SỬA: Cập nhật predicate
                    var semester = await _unitOfWork.GetRepository<Semester>().SingleOrDefaultAsync(predicate: s => s.SemesterId == id);
                    if (semester == null)
                    {
                        throw new NotFoundException($"Không tìm thấy học kỳ với ID: {id} để cập nhật.");
                    }

                    _mapper.Map(updateDto, semester);
                    _unitOfWork.GetRepository<Semester>().UpdateAsync(semester); // Dùng Update đồng bộ
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<SemesterResponse>(semester);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật học kỳ {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteSemesterAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    // SỬA: Cập nhật predicate
                    var semester = await _unitOfWork.GetRepository<Semester>().SingleOrDefaultAsync(predicate: s => s.SemesterId == id);
                    if (semester == null)
                    {
                        throw new NotFoundException($"Không tìm thấy học kỳ với ID: {id} để xóa.");
                    }

                    _unitOfWork.GetRepository<Semester>().DeleteAsync(semester); // Dùng Delete đồng bộ
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa học kỳ {Id}: {Message}", id, ex.Message);
                throw;
            }
        }
    }
}