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

    public async Task<IEnumerable<StudentResponse>> ImportStudentsFromFileAsync(string filePath, IFileService fileService)
    {
        // B1: Đọc JSON từ FileService
        var studentsFromJson = await fileService.ReadStudentsFromJsonAsync(filePath);

        // B2: Chuẩn bị repository
        var repository = _unitOfWork.GetRepository<Student>();
        var importedStudents = new List<Student>();

        foreach (var studentReq in studentsFromJson)
        {
            var studentEntity = _mapper.Map<Student>(studentReq);

            // Có thể kiểm tra trùng ID trước khi thêm
            var existing = await repository.SingleOrDefaultAsync(predicate: s => s.StudentId == studentEntity.StudentId);
            if (existing == null)
            {
                await repository.InsertAsync(studentEntity);
                importedStudents.Add(studentEntity);
            }
            else
            {
                _logger.LogWarning($"Student {studentEntity.StudentId} đã tồn tại, bỏ qua.");
            }
        }

        await _unitOfWork.CommitAsync();

        return _mapper.Map<IEnumerable<StudentResponse>>(importedStudents);
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
