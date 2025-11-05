using AutoMapper;
using EzyFix.DAL.Data.Requests.Exams;
using EzyFix.DAL.Data.Requests.Roles;
using EzyFix.DAL.Data.Requests.Semesters;
using EzyFix.DAL.Data.Requests.Students;
using EzyFix.DAL.Data.Requests.Subjects;
using EzyFix.DAL.Data.Responses.Exams;
using EzyFix.DAL.Data.Responses.Roles;
using EzyFix.DAL.Data.Responses.Semesters;
using EzyFix.DAL.Data.Responses.Students;
using EzyFix.DAL.Data.Responses.Subjects;
using EzyFix.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.DAL.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Semester, SemesterResponse>();
            CreateMap<CreateSemesterRequest, Semester>();
            CreateMap<UpdateSemesterRequest, Semester>();

            CreateMap<Subject, SubjectResponse>();
            CreateMap<CreateSubjectRequest, Subject>();
            CreateMap<UpdateSubjectRequest, Subject>();

            CreateMap<Student, StudentResponse>();
            CreateMap<CreateStudentRequest, Student>();
            CreateMap<UpdateStudentRequest, Student>();

            CreateMap<Exam, ExamResponse>();
            CreateMap<CreateExamRequest, Exam>();
            CreateMap<UpdateExamRequest, Exam>();

            CreateMap<Role, RoleResponse>();
            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();
        }
    } 
}