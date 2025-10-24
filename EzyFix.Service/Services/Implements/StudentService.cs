using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Exceptions;
using EzyFix.DAL.Data.Requests.Students;
using EzyFix.DAL.Data.Responses.Students;
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
    public class StudentService : BaseService<StudentService>, IStudentService
    {
        public StudentService(
            IUnitOfWork<AppDbContext> unitOfWork,
            ILogger<StudentService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<StudentResponse>> GetAllStudentsAsync()
        {
            try
            {
                var students = await _unitOfWork.GetRepository<Student>().GetListAsync(
                    include: q => q.Include(s => s.Assignments)
                );
                var studentResponses = students.Select(student =>   
                {
                    var response = _mapper.Map<StudentResponse>(student);
                    response.AssignmentCount = student.Assignments?.Count ?? 0;
                    return response;
                });
                return studentResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y danh sách sinh viên: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<StudentResponse?> GetStudentByIdAsync(Guid id)
        {
            try
            {
                var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(
                    predicate: s => s.StudentId == id,
                    include: q => q.Include(s => s.Assignments)
                );

                if (student == null)
                {
                    
                    throw new NotFoundException($"Không tìm th?y sinh viên v?i ID: {id}");
                }

                var response = _mapper.Map<StudentResponse>(student);
                response.AssignmentCount = student.Assignments?.Count ?? 0;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi l?y sinh viên theo ID {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<StudentResponse> CreateStudentAsync(CreateStudentRequest createDto)
        {
            try
            {
                // Ki?m tra email trùng l?p
                var existingStudent = await _unitOfWork.GetRepository<Student>()
                    .SingleOrDefaultAsync(predicate: s => s.Email.ToLower() == createDto.Email.ToLower());

                if (existingStudent != null)
                {
                    throw new BadRequestException($"Email '{createDto.Email}' ?ã ???c s? d?ng b?i sinh viên khác.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var student = _mapper.Map<Student>(createDto);

                    // 1. T? sinh Guid m?i cho ID
                    student.StudentId = Guid.NewGuid();
                    student.CreatedAt = DateTime.UtcNow;
                    student.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.GetRepository<Student>().InsertAsync(student);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<StudentResponse>(student);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi t?o sinh viên m?i: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<StudentResponse> UpdateStudentAsync(Guid id, UpdateStudentRequest updateDto)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    // S?A: C?p nh?t predicate
                    var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(predicate: s => s.StudentId == id);
                    if (student == null)
                    {
                        throw new NotFoundException($"Không tìm th?y sinh viên v?i ID: {id} ?? c?p nh?t.");
                    }

                    // Ki?m tra email trùng l?p n?u có thay ??i email
                    if (!string.IsNullOrEmpty(updateDto.Email) && 
                        !string.Equals(student.Email, updateDto.Email, StringComparison.OrdinalIgnoreCase))
                    {
                        var existingStudent = await _unitOfWork.GetRepository<Student>()
                            .SingleOrDefaultAsync(predicate: s => s.Email.ToLower() == updateDto.Email.ToLower() && s.StudentId != id);

                        if (existingStudent != null)
                        {
                            throw new BadRequestException($"Email '{updateDto.Email}' ?ã ???c s? d?ng b?i sinh viên khác.");
                        }
                    }

                    _mapper.Map(updateDto, student);
                    student.UpdatedAt = DateTime.UtcNow;
                    
                    _unitOfWork.GetRepository<Student>().UpdateAsync(student); // Dùng Update ??ng b?
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<StudentResponse>(student);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi c?p nh?t sinh viên {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    // S?A: C?p nh?t predicate
                    var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(predicate: s => s.StudentId == id);
                    if (student == null)
                    {
                        throw new NotFoundException($"Không tìm th?y sinh viên v?i ID: {id} ?? xóa.");
                    }

                    // Ki?m tra xem sinh viên có assignments không
                    var hasAssignments = await _unitOfWork.GetRepository<Assignment>()
                        .CountAsync(predicate: a => a.StudentId == id) > 0;

                    if (hasAssignments)
                    {
                        throw new BadRequestException($"Không th? xóa sinh viên vì sinh viên này có bài t?p ???c giao.");
                    }

                    _unitOfWork.GetRepository<Student>().DeleteAsync(student); // Dùng Delete ??ng b?
                    await _unitOfWork.CommitAsync();
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "L?i khi xóa sinh viên {Id}: {Message}", id, ex.Message);
                throw;
            }
        }
    }
}