using AutoMapper;
using EzyFix.DAL.Data.Requests.Assignments;
using EzyFix.DAL.Data.Requests.ExamGradingCriteria;
using EzyFix.DAL.Data.Requests.ExamKeyword;
using EzyFix.DAL.Data.Requests.Exams;
using EzyFix.DAL.Data.Requests.GradingDetails;
using EzyFix.DAL.Data.Requests.Keywords;
using EzyFix.DAL.Data.Requests.LecturerSubjects;
using EzyFix.DAL.Data.Requests.Roles;
using EzyFix.DAL.Data.Requests.ScoreColumns;
using EzyFix.DAL.Data.Requests.Semesters;
using EzyFix.DAL.Data.Requests.Students;
using EzyFix.DAL.Data.Requests.Subjects;
using EzyFix.DAL.Data.Responses.Assignments;
using EzyFix.DAL.Data.Responses.ExamGradingCriteria;
using EzyFix.DAL.Data.Responses.ExamKeyword;
using EzyFix.DAL.Data.Responses.Exams;
using EzyFix.DAL.Data.Responses.GradingDetails;
using EzyFix.DAL.Data.Responses.Keywords;
using EzyFix.DAL.Data.Responses.LecturerSubjects;
using EzyFix.DAL.Data.Responses.Roles;
using EzyFix.DAL.Data.Responses.ScoreColumns;
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

            //CreateMap<Keyword, KeywordResponse>();
            //CreateMap<CreateKeywordRequest, Keyword>();
            //CreateMap<UpdateKeywordRequest, Keyword>();

            //CreateMap<ScoreColumn, ScoreColumnResponse>();
            //CreateMap<CreateScoreColumnRequest, ScoreColumn>();
            //CreateMap<UpdateScoreColumnRequest, ScoreColumn>();

            //CreateMap<ExamGradingCriterion, ExamGradingCriterionResponse>();
            //CreateMap<CreateExamGradingCriterionRequest, ExamGradingCriterion>();
            //CreateMap<UpdateExamGradingCriterionRequest, ExamGradingCriterion>();

            CreateMap<Student, StudentResponse>();
            CreateMap<CreateStudentRequest, Student>();
            CreateMap<UpdateStudentRequest, Student>();

            //CreateMap<GradingDetail, GradingDetailResponse>();
            //CreateMap<CreateGradingDetailRequest, GradingDetail>();
            //CreateMap<UpdateGradingDetailRequest, GradingDetail>();

            //CreateMap<LecturerSubject, LecturerSubjectResponse>();
            //CreateMap<CreateLecturerSubjectRequest, LecturerSubject>();
            //CreateMap<UpdateLecturerSubjectRequest, LecturerSubject>();

            //CreateMap<Assignment, AssignmentResponse>();
            //CreateMap<CreateAssignmentRequest, Assignment>();
            //CreateMap<UpdateAssignmentRequest, Assignment>();

            CreateMap<Exam, ExamResponse>();
            CreateMap<CreateExamRequest, Exam>();
            CreateMap<UpdateExamRequest, Exam>();

            CreateMap<Role, RoleResponse>();
            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();

            //CreateMap<ExamKeyword, ExamKeywordResponse>();
            //CreateMap<CreateExamKeywordRequest, ExamKeyword>();
            //CreateMap<UpdateExamKeywordRequest, ExamKeyword>();
        }
    } 
}