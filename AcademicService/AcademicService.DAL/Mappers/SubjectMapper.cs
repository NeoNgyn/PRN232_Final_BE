using AcademicService.DAL.Data.Requests;
using AcademicService.DAL.Data.Responses;
using AcademicService.DAL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Mappers
{
    public class SubjectMapper : Profile
    {
        public SubjectMapper()
        {
            // Subject mappings
            CreateMap<Subject, SubjectResponse>();
            CreateMap<CreateSubjectRequest, Subject>();
            CreateMap<UpdateSubjectRequest, Subject>().ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null)
                ); ;

        }
    }
}
