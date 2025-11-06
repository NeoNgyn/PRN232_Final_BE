using AutoMapper;
using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;
using AcademicService.DAL.Models;

namespace AcademicService.DAL.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Exam mappings
        CreateMap<Exam, ExamResponse>();
        CreateMap<CreateExamRequest, Exam>();
        CreateMap<UpdateExamRequest, Exam>();

        // Semester mappings
        CreateMap<Semester, SemesterResponse>();
        CreateMap<CreateSemesterRequest, Semester>();
        CreateMap<UpdateSemesterRequest, Semester>();

        // Subject mappings
        CreateMap<Subject, SubjectResponse>();
        CreateMap<CreateSubjectRequest, Subject>();
        CreateMap<UpdateSubjectRequest, Subject>();

        // Student mappings
        CreateMap<Student, StudentResponse>();
        CreateMap<CreateStudentRequest, Student>();
        CreateMap<UpdateStudentRequest, Student>();
    }
}
