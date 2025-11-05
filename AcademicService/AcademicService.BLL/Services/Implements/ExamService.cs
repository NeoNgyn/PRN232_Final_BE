using AutoMapper;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.Exceptions;
using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;
using AcademicService.DAL.Models;
using AcademicService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace AcademicService.BLL.Services.Implements;

public class ExamService : IExamService
{
    private readonly IUnitOfWork<AcademicDbContext> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ExamService> _logger;

    public ExamService(IUnitOfWork<AcademicDbContext> unitOfWork, IMapper mapper, ILogger<ExamService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ExamResponse>> GetAllExamsAsync()
    {
        var repository = _unitOfWork.GetRepository<Exam>();
        var exams = await repository.GetListAsync();
        return _mapper.Map<IEnumerable<ExamResponse>>(exams);
    }

    public async Task<ExamResponse?> GetExamByIdAsync(Guid id)
    {
        var repository = _unitOfWork.GetRepository<Exam>();
        var exam = await repository.GetByIdAsync(id);
        if (exam == null)
            throw new NotFoundException($"Exam with ID {id} not found");
        
        return _mapper.Map<ExamResponse>(exam);
    }

    public async Task<ExamResponse> CreateExamAsync(CreateExamRequest request)
    {
        var exam = _mapper.Map<Exam>(request);
        
        var repository = _unitOfWork.GetRepository<Exam>();
        await repository.InsertAsync(exam);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ExamResponse>(exam);
    }

    public async Task<ExamResponse> UpdateExamAsync(Guid id, UpdateExamRequest request)
    {
        var repository = _unitOfWork.GetRepository<Exam>();
        var exam = await repository.GetByIdAsync(id);
        if (exam == null)
            throw new NotFoundException($"Exam with ID {id} not found");

        _mapper.Map(request, exam);
        repository.UpdateAsync(exam);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ExamResponse>(exam);
    }

    public async Task DeleteExamAsync(Guid id)
    {
        var repository = _unitOfWork.GetRepository<Exam>();
        var exam = await repository.GetByIdAsync(id);
        if (exam == null)
            throw new NotFoundException($"Exam with ID {id} not found");

        repository.DeleteAsync(exam);
        await _unitOfWork.CommitAsync();
    }
}
