using AutoMapper;
using EzyFix.DAL.Data.Requests.Keywords;
using EzyFix.DAL.Data.Requests.Semesters;
using EzyFix.DAL.Data.Requests.Subjects;
using EzyFix.DAL.Data.Responses.Keywords;
using EzyFix.DAL.Data.Responses.Semesters;
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

            CreateMap<Keyword, KeywordResponse>();
            CreateMap<CreateKeywordRequest, Keyword>();
            CreateMap<UpdateKeywordRequest, Keyword>();
        }
    }
}