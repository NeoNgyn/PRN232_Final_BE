using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Subjects;
using EzyFix.DAL.Data.Responses.Subjects;
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
    public class SubjectService : BaseService<SubjectService>, ISubjectService
    {
        public SubjectService(
            IUnitOfWork<AppDbContext> unitOfWork,
            ILogger<SubjectService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<SubjectResponse>> GetAllSubjectsAsync()
        {
            try
            {
                var subjects = await _unitOfWork.GetRepository<Subject>().GetListAsync();
                return _mapper.Map<IEnumerable<SubjectResponse>>(subjects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách môn học: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<SubjectResponse?> GetSubjectByIdAsync(Guid id)
        {
            try
            {
                var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: s => s.SubjectId == id);

                if (subject == null)
                {
                    throw new NotFoundException($"Không tìm thấy môn học với ID: {id}");
                }

                return _mapper.Map<SubjectResponse>(subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy môn học theo ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<SubjectResponse> CreateSubjectAsync(CreateSubjectRequest createDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var subject = _mapper.Map<Subject>(createDto);

                    // SỬA: Tự sinh Guid mới cho ID
                    subject.SubjectId = Guid.NewGuid();

                    await _unitOfWork.GetRepository<Subject>().InsertAsync(subject);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<SubjectResponse>(subject);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo môn học mới: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<SubjectResponse> UpdateSubjectAsync(Guid id, UpdateSubjectRequest updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: s => s.SubjectId == id); // SỬA: Cập nhật predicate
                    if (subject == null)
                    {
                        throw new NotFoundException($"Không tìm thấy môn học với ID: {id} để cập nhật.");
                    }

                    _mapper.Map(updateDto, subject);
                    _unitOfWork.GetRepository<Subject>().UpdateAsync(subject);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<SubjectResponse>(subject);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật môn học {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteSubjectAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: s => s.SubjectId == id); // SỬA: Cập nhật predicate
                    if (subject == null)
                    {
                        throw new NotFoundException($"Không tìm thấy môn học với ID: {id} để xóa.");
                    }

                    _unitOfWork.GetRepository<Subject>().DeleteAsync(subject);
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa môn học {Id}: {Message}", id, ex.Message);
                throw;
            }
        }
    }
}