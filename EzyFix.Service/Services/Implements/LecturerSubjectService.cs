using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.Requests.LecturerSubjects;
using EzyFix.DAL.Data.Responses.LecturerSubjects;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Implements
{
    public class LecturerSubjectService : BaseService<LecturerSubjectService>, ILecturerSubjectService
    {
        public LecturerSubjectService(
            IUnitOfWork<AppDbContext> unitOfWork,
            ILogger<LecturerSubjectService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<LecturerSubjectResponse>> GetAllLecturerSubjectsAsync()
        {
            try
            {
                var lecturerSubjects = await _unitOfWork.GetRepository<LecturerSubject>().GetListAsync(
                    include: q => q.Include(ls => ls.User)
                                  .Include(ls => ls.Subject)
                                  .Include(ls => ls.Semester)
                                  .Include(ls => ls.Exams)
                );
                
                var responses = lecturerSubjects.Select(ls =>
                {
                    var response = _mapper.Map<LecturerSubjectResponse>(ls);
                    response.LecturerName = ls.User?.Name ?? "Unknown Lecturer";
                    response.SubjectName = ls.Subject?.Name ?? "Unknown Subject";
                    response.SemesterName = ls.Semester?.Name ?? "Unknown Semester";
                    response.ExamCount = ls.Exams?.Count ?? 0;
                    return response;
                });
                
                return responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y danh sách phân công gi?ng viên: {Message}", ex.Message);
                throw; // Ném l?i ?? middleware ho?c controller x? lý
            }
        }

        public async Task<LecturerSubjectResponse?> GetLecturerSubjectByIdAsync(Guid id)
        {
            try
            {
                var lecturerSubject = await _unitOfWork.GetRepository<LecturerSubject>().SingleOrDefaultAsync(
                    predicate: ls => ls.LecturerSubjectId == id,
                    include: q => q.Include(ls => ls.User)
                                  .Include(ls => ls.Subject)
                                  .Include(ls => ls.Semester)
                                  .Include(ls => ls.Exams)
                );

                if (lecturerSubject == null)
                {
                    // Ném m?t exception c? th? ?? controller b?t và tr? v? 404
                    throw new NotFoundException($"Không tìm th?y phân công gi?ng viên v?i ID: {id}");
                }

                var response = _mapper.Map<LecturerSubjectResponse>(lecturerSubject);
                response.LecturerName = lecturerSubject.User?.Name ?? "Unknown Lecturer";
                response.SubjectName = lecturerSubject.Subject?.Name ?? "Unknown Subject";
                response.SemesterName = lecturerSubject.Semester?.Name ?? "Unknown Semester";
                response.ExamCount = lecturerSubject.Exams?.Count ?? 0;
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y phân công gi?ng viên theo ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<LecturerSubjectResponse> CreateLecturerSubjectAsync(CreateLecturerSubjectRequest createDto)
        {
            try
            {
                // Validate that LecturerId, SubjectId, and SemesterId exist
                await ValidateLecturerSubjectSemesterExistAsync(createDto.UserId, createDto.SubjectId, createDto.SemesterId);

                // Check for duplicate assignment (same LecturerId, SubjectId, and SemesterId)
                var existingAssignment = await _unitOfWork.GetRepository<LecturerSubject>()
                    .SingleOrDefaultAsync(predicate: ls => ls.UserId == createDto.UserId && 
                                                          ls.SubjectId == createDto.SubjectId && 
                                                          ls.SemesterId == createDto.SemesterId);

                if (existingAssignment != null)
                {
                    throw new BadRequestException($"Gi?ng viên này ?ã ???c phân công môn h?c này trong h?c k? này.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var lecturerSubject = _mapper.Map<LecturerSubject>(createDto);

                    // 1. T? sinh Guid m?i cho ID
                    lecturerSubject.LecturerSubjectId = Guid.NewGuid();
                    lecturerSubject.AssignedAt = DateTime.UtcNow;
                    lecturerSubject.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.GetRepository<LecturerSubject>().InsertAsync(lecturerSubject);
                    await _unitOfWork.CommitAsync();

                    // Get the created assignment with related data
                    var createdAssignment = await _unitOfWork.GetRepository<LecturerSubject>().SingleOrDefaultAsync(
                        predicate: ls => ls.LecturerSubjectId == lecturerSubject.LecturerSubjectId,
                        include: q => q.Include(ls => ls.User)
                                      .Include(ls => ls.Subject)
                                      .Include(ls => ls.Semester)
                                      .Include(ls => ls.Exams)
                    );

                    var response = _mapper.Map<LecturerSubjectResponse>(createdAssignment);
                    response.LecturerName = createdAssignment.User?.Name ?? "Unknown Lecturer";
                    response.SubjectName = createdAssignment.Subject?.Name ?? "Unknown Subject";
                    response.SemesterName = createdAssignment.Semester?.Name ?? "Unknown Semester";
                    response.ExamCount = createdAssignment.Exams?.Count ?? 0;
                    
                    return response;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi t?o phân công gi?ng viên m?i: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<LecturerSubjectResponse> UpdateLecturerSubjectAsync(Guid id, UpdateLecturerSubjectRequest updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    // S?A: C?p nh?t predicate
                    var lecturerSubject = await _unitOfWork.GetRepository<LecturerSubject>().SingleOrDefaultAsync(
                        predicate: ls => ls.LecturerSubjectId == id,
                        include: q => q.Include(ls => ls.User)
                                      .Include(ls => ls.Subject)
                                      .Include(ls => ls.Semester)
                                      .Include(ls => ls.Exams)
                    );
                    
                    if (lecturerSubject == null)
                    {
                        throw new NotFoundException($"Không tìm th?y phân công gi?ng viên v?i ID: {id} ?? c?p nh?t.");
                    }

                    // Validate new references if they are being changed
                    if (updateDto.UserId.HasValue || updateDto.SubjectId.HasValue || updateDto.SemesterId.HasValue)
                    {
                        var newLecturerId = updateDto.UserId ?? lecturerSubject.UserId;
                        var newSubjectId = updateDto.SubjectId ?? lecturerSubject.SubjectId;
                        var newSemesterId = updateDto.SemesterId ?? lecturerSubject.SemesterId;
                        
                        await ValidateLecturerSubjectSemesterExistAsync(newLecturerId, newSubjectId, newSemesterId);

                        // Check for duplicate if combination is being changed
                        if (newLecturerId != lecturerSubject.UserId || 
                            newSubjectId != lecturerSubject.SubjectId || 
                            newSemesterId != lecturerSubject.SemesterId)
                        {
                            var existingAssignment = await _unitOfWork.GetRepository<LecturerSubject>()
                                .SingleOrDefaultAsync(predicate: ls => ls.UserId == newLecturerId && 
                                                                      ls.SubjectId == newSubjectId && 
                                                                      ls.SemesterId == newSemesterId && 
                                                                      ls.LecturerSubjectId != id);

                            if (existingAssignment != null)
                            {
                                throw new BadRequestException($"Gi?ng viên này ?ã ???c phân công môn h?c này trong h?c k? này.");
                            }
                        }
                    }

                    _mapper.Map(updateDto, lecturerSubject);
                    lecturerSubject.UpdatedAt = DateTime.UtcNow;
                    
                    _unitOfWork.GetRepository<LecturerSubject>().UpdateAsync(lecturerSubject); // Dùng Update ??ng b?
                    await _unitOfWork.CommitAsync();

                    // Get updated assignment with related data
                    var updatedAssignment = await _unitOfWork.GetRepository<LecturerSubject>().SingleOrDefaultAsync(
                        predicate: ls => ls.LecturerSubjectId == id,
                        include: q => q.Include(ls => ls.User)
                                      .Include(ls => ls.Subject)
                                      .Include(ls => ls.Semester)
                                      .Include(ls => ls.Exams)
                    );

                    var response = _mapper.Map<LecturerSubjectResponse>(updatedAssignment);
                    response.LecturerName = updatedAssignment.User?.Name ?? "Unknown Lecturer";
                    response.SubjectName = updatedAssignment.Subject?.Name ?? "Unknown Subject";
                    response.SemesterName = updatedAssignment.Semester?.Name ?? "Unknown Semester";
                    response.ExamCount = updatedAssignment.Exams?.Count ?? 0;
                    
                    return response;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi c?p nh?t phân công gi?ng viên {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteLecturerSubjectAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    // S?A: C?p nh?t predicate
                    var lecturerSubject = await _unitOfWork.GetRepository<LecturerSubject>().SingleOrDefaultAsync(predicate: ls => ls.LecturerSubjectId == id);
                    if (lecturerSubject == null)
                    {
                        throw new NotFoundException($"Không tìm th?y phân công gi?ng viên v?i ID: {id} ?? xóa.");
                    }

                    // Ki?m tra xem có exams không
                    var hasExams = await _unitOfWork.GetRepository<Exam>()
                        .CountAsync(predicate: e => e.LecturerSubjectId == id) > 0;

                    if (hasExams)
                    {
                        throw new BadRequestException($"Không th? xóa phân công này vì ?ã có bài ki?m tra ???c t?o.");
                    }

                    _unitOfWork.GetRepository<LecturerSubject>().DeleteAsync(lecturerSubject); // Dùng Delete ??ng b?
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi xóa phân công gi?ng viên {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LecturerSubjectResponse>> GetLecturerSubjectsByLecturerIdAsync(Guid lecturerId)
        {
            try
            {
                var lecturerSubjects = await _unitOfWork.GetRepository<LecturerSubject>().GetListAsync(
                    predicate: ls => ls.UserId == lecturerId,
                    include: q => q.Include(ls => ls.User)
                                  .Include(ls => ls.Subject)
                                  .Include(ls => ls.Semester)
                                  .Include(ls => ls.Exams)
                );

                var responses = lecturerSubjects.Select(ls =>
                {
                    var response = _mapper.Map<LecturerSubjectResponse>(ls);
                    response.LecturerName = ls.User?.Name ?? "Unknown Lecturer";
                    response.SubjectName = ls.Subject?.Name ?? "Unknown Subject";
                    response.SemesterName = ls.Semester?.Name ?? "Unknown Semester";
                    response.ExamCount = ls.Exams?.Count ?? 0;
                    return response;
                });

                return responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y phân công theo LecturerId {LecturerId}: {Message}", lecturerId, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LecturerSubjectResponse>> GetLecturerSubjectsBySubjectIdAsync(Guid subjectId)
        {
            try
            {
                var lecturerSubjects = await _unitOfWork.GetRepository<LecturerSubject>().GetListAsync(
                    predicate: ls => ls.SubjectId == subjectId,
                    include: q => q.Include(ls => ls.User)
                                  .Include(ls => ls.Subject)
                                  .Include(ls => ls.Semester)
                                  .Include(ls => ls.Exams)
                );

                var responses = lecturerSubjects.Select(ls =>
                {
                    var response = _mapper.Map<LecturerSubjectResponse>(ls);
                    response.LecturerName = ls.User?.Name ?? "Unknown Lecturer";
                    response.SubjectName = ls.Subject?.Name ?? "Unknown Subject";
                    response.SemesterName = ls.Semester?.Name ?? "Unknown Semester";
                    response.ExamCount = ls.Exams?.Count ?? 0;
                    return response;
                });

                return responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y phân công theo SubjectId {SubjectId}: {Message}", subjectId, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<LecturerSubjectResponse>> GetLecturerSubjectsBySemesterIdAsync(Guid semesterId)
        {
            try
            {
                var lecturerSubjects = await _unitOfWork.GetRepository<LecturerSubject>().GetListAsync(
                    predicate: ls => ls.SemesterId == semesterId,
                    include: q => q.Include(ls => ls.User)
                                  .Include(ls => ls.Subject)
                                  .Include(ls => ls.Semester)
                                  .Include(ls => ls.Exams)
                );

                var responses = lecturerSubjects.Select(ls =>
                {
                    var response = _mapper.Map<LecturerSubjectResponse>(ls);
                    response.LecturerName = ls.User?.Name ?? "Unknown Lecturer";
                    response.SubjectName = ls.Subject?.Name ?? "Unknown Subject";
                    response.SemesterName = ls.Semester?.Name ?? "Unknown Semester";
                    response.ExamCount = ls.Exams?.Count ?? 0;
                    return response;
                });

                return responses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y phân công theo SemesterId {SemesterId}: {Message}", semesterId, ex.Message);
                throw;
            }
        }

        private async Task ValidateLecturerSubjectSemesterExistAsync(Guid lecturerId, Guid subjectId, Guid semesterId)
        {
            // Validate that the Lecturer exists
            var lecturerExists = await _unitOfWork.GetRepository<User>()
                .SingleOrDefaultAsync(predicate: l => l.UserId == lecturerId) != null;

            if (!lecturerExists)
            {
                throw new NotFoundException($"Không tìm th?y gi?ng viên v?i ID: {lecturerId}");
            }

            // Validate that the Subject exists
            var subjectExists = await _unitOfWork.GetRepository<Subject>()
                .SingleOrDefaultAsync(predicate: s => s.SubjectId == subjectId) != null;

            if (!subjectExists)
            {
                throw new NotFoundException($"Không tìm th?y môn h?c v?i ID: {subjectId}");
            }

            // Validate that the Semester exists
            var semesterExists = await _unitOfWork.GetRepository<Semester>()
                .SingleOrDefaultAsync(predicate: s => s.SemesterId == semesterId) != null;

            if (!semesterExists)
            {
                throw new NotFoundException($"Không tìm th?y h?c k? v?i ID: {semesterId}");
            }
        }
    }
}