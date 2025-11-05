using AutoMapper;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.Exceptions;
using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;
using AcademicService.DAL.Models;
using AcademicService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace AcademicService.BLL.Services.Implements;

public class SubjectService : ISubjectService
{
    private readonly IUnitOfWork<AcademicDbContext> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SubjectService> _logger;

    public SubjectService(IUnitOfWork<AcademicDbContext> unitOfWork, IMapper mapper, ILogger<SubjectService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<SubjectResponse>> GetAllSubjectsAsync()
    {
        var repository = _unitOfWork.GetRepository<Subject>();
        var subjects = await repository.GetListAsync();
        return _mapper.Map<IEnumerable<SubjectResponse>>(subjects);
    }

    public async Task<SubjectResponse?> GetSubjectByIdAsync(Guid id)
    {
        var repository = _unitOfWork.GetRepository<Subject>();
        var subject = await repository.GetByIdAsync(id);
        if (subject == null)
            throw new NotFoundException($"Subject with ID {id} not found");
        
        return _mapper.Map<SubjectResponse>(subject);
    }

    public async Task<SubjectResponse> CreateSubjectAsync(CreateSubjectRequest request)
    {
        var subject = _mapper.Map<Subject>(request);
        
        var repository = _unitOfWork.GetRepository<Subject>();
        await repository.InsertAsync(subject);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<SubjectResponse>(subject);
    }

    public async Task<SubjectResponse> UpdateSubjectAsync(Guid id, UpdateSubjectRequest request)
    {
        var repository = _unitOfWork.GetRepository<Subject>();
        var subject = await repository.GetByIdAsync(id);
        if (subject == null)
            throw new NotFoundException($"Subject with ID {id} not found");

        _mapper.Map(request, subject);
        repository.UpdateAsync(subject);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<SubjectResponse>(subject);
    }

    public async Task DeleteSubjectAsync(Guid id)
    {
        var repository = _unitOfWork.GetRepository<Subject>();
        var subject = await repository.GetByIdAsync(id);
        if (subject == null)
            throw new NotFoundException($"Subject with ID {id} not found");

        repository.DeleteAsync(subject);
        await _unitOfWork.CommitAsync();
    }
}
