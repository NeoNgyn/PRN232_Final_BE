using AutoMapper;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.Exceptions;
using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;
using AcademicService.DAL.Models;
using AcademicService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace AcademicService.BLL.Services.Implements;

public class SemesterService : ISemesterService
{
    private readonly IUnitOfWork<AcademicDbContext> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SemesterService> _logger;

    public SemesterService(IUnitOfWork<AcademicDbContext> unitOfWork, IMapper mapper, ILogger<SemesterService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<SemesterResponse>> GetAllSemestersAsync()
    {
        var repository = _unitOfWork.GetRepository<Semester>();
        var semesters = await repository.GetListAsync();
        return _mapper.Map<IEnumerable<SemesterResponse>>(semesters);
    }

    public async Task<SemesterResponse?> GetSemesterByIdAsync(Guid id)
    {
        var repository = _unitOfWork.GetRepository<Semester>();
        var semester = await repository.GetByIdAsync(id);
        if (semester == null)
            throw new NotFoundException($"Semester with ID {id} not found");
        
        return _mapper.Map<SemesterResponse>(semester);
    }

    public async Task<SemesterResponse> CreateSemesterAsync(CreateSemesterRequest request)
    {
        var semester = _mapper.Map<Semester>(request);
        
        var repository = _unitOfWork.GetRepository<Semester>();
        await repository.InsertAsync(semester);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<SemesterResponse>(semester);
    }

    public async Task<SemesterResponse> UpdateSemesterAsync(Guid id, UpdateSemesterRequest request)
    {
        var repository = _unitOfWork.GetRepository<Semester>();
        var semester = await repository.GetByIdAsync(id);
        if (semester == null)
            throw new NotFoundException($"Semester with ID {id} not found");

        _mapper.Map(request, semester);
        repository.UpdateAsync(semester);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<SemesterResponse>(semester);
    }

    public async Task DeleteSemesterAsync(Guid id)
    {
        var repository = _unitOfWork.GetRepository<Semester>();
        var semester = await repository.GetByIdAsync(id);
        if (semester == null)
            throw new NotFoundException($"Semester with ID {id} not found");

        repository.DeleteAsync(semester);
        await _unitOfWork.CommitAsync();
    }
}
