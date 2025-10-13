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
            CreateMap<Semester, SemesterResponseDto>();
            CreateMap<CreateSemesterRequestDto, Semester>();
            CreateMap<UpdateSemesterRequestDto, Semester>();

            CreateMap<Subject, SubjectResponseDto>();
            CreateMap<CreateSubjectRequestDto, Subject>();
            CreateMap<UpdateSubjectRequestDto, Subject>();

            CreateMap<Keyword, KeywordResponseDto>();
            CreateMap<CreateKeywordRequestDto, Keyword>();
            CreateMap<UpdateKeywordRequestDto, Keyword>();
        }
    }
}