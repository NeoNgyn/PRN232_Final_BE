using AcademicService.BLL.Services.Interfaces;
using AcademicService.BLL.Extension;
using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Requests.Submission;
using AcademicService.DAL.Data.Responses.Submission;
using AcademicService.DAL.Models;
using AcademicService.DAL.Repositories.Interfaces;
using AutoMapper;
using EzyFix.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AcademicService.BLL.Services.Implements
{
    public class SubmissionService : BaseService<SubmissionService>, ISubmissionService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private const string DefaultProfilePicture = "https://static.vecteezy.com/system/resources/previews/009/292/244/non_2x/default-avatar-icon-of-social-media-user-vector.jpg";

        public SubmissionService(
        IUnitOfWork<AcademicDbContext> unitOfWork,
        ILogger<SubmissionService> logger,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        ICloudinaryService cloudinaryService
    ) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _cloudinaryService = cloudinaryService;
        }

        public async Task<SubmissionListResponse> CreateSubmissionAsync(CreateSubmissionRequest request, IFormFile fileSubmit)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Request data cannot be null.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var newSubmission = _mapper.Map<Submission>(request);
                    var exam = (await _unitOfWork.GetRepository<Exam>()
                        .SingleOrDefaultAsync(predicate: s => s.ExamId == request.ExamId))
                        .ValidateExists(request.ExamId, "Exam not found or existed!");

                    newSubmission.SubmissionId = Guid.NewGuid();
                    newSubmission.ExamId = request.ExamId;

                    // Use StudentId from request if provided, otherwise parse from filename
                    if (!string.IsNullOrEmpty(request.StudentId))
                    {
                        newSubmission.StudentId = request.StudentId;
                    }
                    else
                    {
                        string lastPart = fileSubmit.FileName.Split('_').Last();
                        string fileName = lastPart.Length >= 8 ? lastPart.Substring(0, 8) : lastPart;

                        if (!(fileName.StartsWith("SE", StringComparison.OrdinalIgnoreCase)
                           || fileName.StartsWith("SS", StringComparison.OrdinalIgnoreCase)
                           || fileName.StartsWith("HE", StringComparison.OrdinalIgnoreCase)
                           || fileName.StartsWith("SA", StringComparison.OrdinalIgnoreCase)))
                        {
                            fileName = string.Empty;
                        }

                        newSubmission.StudentId = fileName;
                    }

                    newSubmission.ExaminerId = request.ExaminerId;
                    newSubmission.OriginalFileName = fileSubmit.FileName;
                    newSubmission.UploadedAt = DateTime.UtcNow;

                    // Upload file to Cloudinary if provided
                    if (fileSubmit != null && fileSubmit.Length > 0)
                    {
                        try
                        {
                            newSubmission.FilePath = await _cloudinaryService.UploadFileAsync(fileSubmit, "submisions");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Cloudinary upload failed, using placeholder URL");
                            // Use placeholder if Cloudinary fails
                            newSubmission.FilePath = $"https://placeholder.com/submissions/{fileSubmit.FileName}";
                        }
                    }
                    else
                    {
                        newSubmission.FilePath = DefaultProfilePicture;
                    }



                    await _unitOfWork.GetRepository<Submission>().InsertAsync(newSubmission);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<SubmissionListResponse>(newSubmission);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating submision: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteSubmissionAsync(Guid id)
        {
            try
            {
                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var submision = (await _unitOfWork.GetRepository<Submission>()
                        .SingleOrDefaultAsync(
                            predicate: s => s.SubmissionId == id
                        )).ValidateExists(id, "Can not find this Submission because it isn't existed");

                    _unitOfWork.GetRepository<Submission>().DeleteAsync(submision);

                    await _unitOfWork.CommitAsync();

                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting submission: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<SubmissionListResponse>> GetAllSubmissionsAsync()
        {
            try
            {
                var submissions = await _unitOfWork.GetRepository<Submission>()
                    .GetListAsync();

                return _mapper.Map<IEnumerable<SubmissionListResponse>>(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving submission list: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<SubmissionListResponse>> GetQuerySubmissionsAsync()
        {
            try
            {
                var submissions = _unitOfWork.GetRepository<Submission>()
                    .GetQueryable();

                return _mapper.Map<IEnumerable<SubmissionListResponse>>(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving submission list: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<SubmissionDetailResponse> GetSubmissionByIdAsync(Guid id)
        {
            try
            {
                var submission = (await _unitOfWork.GetRepository<Submission>()
                    .SingleOrDefaultAsync(
                        predicate: s => s.SubmissionId == id,
                        include: c => c.Include(e => e.Grades).ThenInclude(a => a.Criteria)
                                        .Include(v => v.Violations)
                    )).ValidateExists(id, "Can not find this Submission because it isn't existed");

                return _mapper.Map<SubmissionDetailResponse>(submission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service {Message}", ex.Message);
                throw;
            }
        }

        public async Task<SubmissionListResponse> UpdateSubmissionAsync(Guid id, UpdateSubmissionRequest request)
        {
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Request data cannot be null.");
                }

                return await _unitOfWork.ProcessInTransactionAsync(async () =>
                {
                    var submission = (await _unitOfWork.GetRepository<Submission>()
                        .SingleOrDefaultAsync(
                            predicate: s => s.SubmissionId == id,
                            include: c => c.Include(e => e.Grades).ThenInclude(a => a.Criteria)
                                            .Include(v => v.Violations)
                        )).ValidateExists(id, "Can't update because this Submission isn't existed!");

                    var grades = submission.Grades.ToList();
                    decimal scores = 0;
                    for (int i = 0; i < grades.Count; i++)
                    {
                        scores += grades[i].Score;
                    }

                    var violations = submission.Violations.ToList();
                    decimal penalty = 0;
                    for (int i = 0; i < violations.Count; i++)
                    {
                        penalty += violations[i].Penalty;
                    }

                    var finalScore = scores - penalty;

                   _mapper.Map(request, submission);
                    submission.TotalScore = finalScore <= 0 ? 0 : finalScore;
                    submission.GradingStatus = finalScore <= 0 ? "Failed" : "Passed";

                    _unitOfWork.GetRepository<Submission>().UpdateAsync(submission);

                    var student = (await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(
                        predicate: s => s.StudentId == submission.StudentId
                        ));

                    if (student != null)
                        throw new Exception("Student not found for updating status.");

                    student.Status = submission.TotalScore > 0 ? "Passed" : "Not Passed";

                    _unitOfWork.GetRepository<Student>().UpdateAsync(student);

                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<SubmissionListResponse>(submission);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating submission: {Message}", ex.Message);
                throw;
            }
        }
    }
}
