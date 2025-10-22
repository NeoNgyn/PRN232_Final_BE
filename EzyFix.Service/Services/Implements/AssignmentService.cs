using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.Requests.Assignments;
using EzyFix.DAL.Data.Responses.Assignments;
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
    public class AssignmentService : BaseService<AssignmentService>, IAssignmentService
    {
        public AssignmentService(
           IUnitOfWork<AppDbContext> unitOfWork,
           ILogger<AssignmentService> logger,
           IMapper mapper,
           IHttpContextAccessor httpContextAccessor)
           : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<AssignmentResponse> CreateAssignmentAsync(CreateAssignmentRequest createDto)
        {
            try
            {
                // Kiểm tra sinh viên tồn tại
                var student = await _unitOfWork.GetRepository<Student>()
                    .SingleOrDefaultAsync(predicate: s => s.StudentId == createDto.StudentId);
                if (student == null)
                {
                    throw new BadRequestException($"Không tìm thấy sinh viên với ID: {createDto.StudentId}");
                }

                // Kiểm tra bài kiểm tra tồn tại
                var exam = await _unitOfWork.GetRepository<Exam>()
                    .SingleOrDefaultAsync(predicate: e => e.ExamId == createDto.ExamId);
                if (exam == null)
                {
                    throw new BadRequestException($"Không tìm thấy bài kiểm tra với ID: {createDto.ExamId}");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var assignment = _mapper.Map<Assignment>(createDto);

                    assignment.AssignmentId = Guid.NewGuid();
                    assignment.CreatedAt = DateTime.UtcNow;
                    assignment.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.GetRepository<Assignment>().InsertAsync(assignment);
                    await _unitOfWork.CommitAsync();

                    // Lấy assignment vừa tạo kèm theo thông tin liên quan
                    var createdAssignment = await _unitOfWork.GetRepository<Assignment>()
                        .SingleOrDefaultAsync(predicate: a => a.AssignmentId == assignment.AssignmentId);

                    return _mapper.Map<AssignmentResponse>(createdAssignment);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo bài tập mới: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAssignmentAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var assignment = await _unitOfWork.GetRepository<Assignment>()
                        .SingleOrDefaultAsync(predicate: a => a.AssignmentId == id);

                    if (assignment == null)
                    {
                        throw new NotFoundException($"Không tìm thấy bài tập với ID: {id} để xóa.");
                    }

                    // Xóa các GradingResults liên quan trước
                    var gradingResults = await _unitOfWork.GetRepository<GradingResult>()
                        .GetListAsync(predicate: gr => gr.AssignmentId == id);

                    if (gradingResults.Any())
                    {
                        foreach (var result in gradingResults)
                        {
                            _unitOfWork.GetRepository<GradingResult>().DeleteAsync(result);
                        }
                    }

                    _unitOfWork.GetRepository<Assignment>().DeleteAsync(assignment);
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa bài tập {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AssignmentResponse>> GetAllAssignmentsAsync()
        {
            try
            {
                var assignments = await _unitOfWork.GetRepository<Assignment>()
                    .GetListAsync();
                return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách bài tập: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<AssignmentResponse?> GetAssignmentByIdAsync(Guid id)
        {
            try
            {
                var assignment = await _unitOfWork.GetRepository<Assignment>()
                    .SingleOrDefaultAsync(
                        predicate: a => a.AssignmentId == id);

                if (assignment == null)
                {
                    throw new NotFoundException($"Không tìm thấy bài tập với ID: {id}");
                }
                return _mapper.Map<AssignmentResponse>(assignment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy bài tập theo ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AssignmentResponse>> GetAssignmentsByExamAsync(Guid examId)
        {
            try
            {
                // Kiểm tra bài kiểm tra tồn tại
                var exam = await _unitOfWork.GetRepository<Exam>()
                    .SingleOrDefaultAsync(predicate: e => e.ExamId == examId);
                if (exam == null)
                {
                    throw new NotFoundException($"Không tìm thấy bài kiểm tra với ID: {examId}");
                }

                var assignments = await _unitOfWork.GetRepository<Assignment>()
                    .GetListAsync(predicate: a => a.ExamId == examId);

                return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách bài tập theo bài kiểm tra {ExamId}: {Message}", examId, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AssignmentResponse>> GetAssignmentsByStudentAsync(Guid studentId)
        {
            try
            {
                // Kiểm tra sinh viên tồn tại
                var student = await _unitOfWork.GetRepository<Student>()
                    .SingleOrDefaultAsync(predicate: s => s.StudentId == studentId);
                if (student == null)
                {
                    throw new NotFoundException($"Không tìm thấy sinh viên với ID: {studentId}");
                }

                var assignments = await _unitOfWork.GetRepository<Assignment>()
                    .GetListAsync(predicate: a => a.StudentId == studentId);

                return _mapper.Map<IEnumerable<AssignmentResponse>>(assignments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách bài tập theo sinh viên {StudentId}: {Message}", studentId, ex.Message);
                throw;
            }
        }

        public async Task<AssignmentResponse> UpdateAssignmentAsync(Guid id, UpdateAssignmentRequest updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var assignment = await _unitOfWork.GetRepository<Assignment>()
                        .SingleOrDefaultAsync(predicate: a => a.AssignmentId == id);

                    if (assignment == null)
                    {
                        throw new NotFoundException($"Không tìm thấy bài tập với ID: {id} để cập nhật.");
                    }

                    // Kiểm tra sinh viên tồn tại
                    var student = await _unitOfWork.GetRepository<Student>()
                        .SingleOrDefaultAsync(predicate: s => s.StudentId == updateDto.StudentId);
                    if (student == null)
                    {
                        throw new BadRequestException($"Không tìm thấy sinh viên với ID: {updateDto.StudentId}");
                    }

                    // Kiểm tra bài kiểm tra tồn tại
                    var exam = await _unitOfWork.GetRepository<Exam>()
                        .SingleOrDefaultAsync(predicate: e => e.ExamId == updateDto.ExamId);
                    if (exam == null)
                    {
                        throw new BadRequestException($"Không tìm thấy bài kiểm tra với ID: {updateDto.ExamId}");
                    }

                    _mapper.Map(updateDto, assignment);
                    assignment.UpdatedAt = DateTime.UtcNow;

                    _unitOfWork.GetRepository<Assignment>().UpdateAsync(assignment);
                    await _unitOfWork.CommitAsync();

                    // Lấy assignment vừa cập nhật
                    var updatedAssignment = await _unitOfWork.GetRepository<Assignment>()
                        .SingleOrDefaultAsync(predicate: a => a.AssignmentId == id);

                    return _mapper.Map<AssignmentResponse>(updatedAssignment);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật bài tập {Id}: {Message}", id, ex.Message);
                throw;
            }
        }



    }
}
