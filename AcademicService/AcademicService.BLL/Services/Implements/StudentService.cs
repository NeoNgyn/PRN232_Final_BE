using AutoMapper;
using AcademicService.BLL.Services.Interfaces;
using AcademicService.DAL.Data.Exceptions;
using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;
using AcademicService.DAL.Models;
using AcademicService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace AcademicService.BLL.Services.Implements;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork<AcademicDbContext> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IUnitOfWork<AcademicDbContext> unitOfWork, IMapper mapper, ILogger<StudentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<StudentResponse>> GetAllStudentsAsync()
    {
        var repository = _unitOfWork.GetRepository<Student>();
        var students = await repository.GetListAsync();
        return _mapper.Map<IEnumerable<StudentResponse>>(students);
    }

    public async Task<StudentResponse?> GetStudentByIdAsync(Guid id)
    {
        var repository = _unitOfWork.GetRepository<Student>();
        var student = await repository.GetByIdAsync(id);
        if (student == null)
            throw new NotFoundException($"Student with ID {id} not found");
        
        return _mapper.Map<StudentResponse>(student);
    }

    public async Task<StudentResponse> CreateStudentAsync(CreateStudentRequest request)
    {
        var student = _mapper.Map<Student>(request);
        
        var repository = _unitOfWork.GetRepository<Student>();
        await repository.InsertAsync(student);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<StudentResponse>(student);
    }

    public async Task<StudentResponse> UpdateStudentAsync(Guid id, UpdateStudentRequest request)
    {
        var repository = _unitOfWork.GetRepository<Student>();
        var student = await repository.GetByIdAsync(id);
        if (student == null)
            throw new NotFoundException($"Student with ID {id} not found");

        _mapper.Map(request, student);
        repository.UpdateAsync(student);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<StudentResponse>(student);
    }

    public async Task DeleteStudentAsync(Guid id)
    {
        var repository = _unitOfWork.GetRepository<Student>();
        var student = await repository.GetByIdAsync(id);
        if (student == null)
            throw new NotFoundException($"Student with ID {id} not found");

        repository.DeleteAsync(student);
        await _unitOfWork.CommitAsync();
    }
}
