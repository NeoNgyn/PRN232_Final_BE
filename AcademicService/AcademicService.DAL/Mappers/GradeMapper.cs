using AcademicService.DAL.Data.Requests.Grade;
using AcademicService.DAL.Data.Responses.Grade;
using AcademicService.DAL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Mappers
{
    public class GradeMapper : Profile
    {
        public GradeMapper()
        {
            CreateMap<CreateGradeRequest, Grade>();
            CreateMap<UpdateGradeRequest, Grade>().ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null)
                ); ;

            CreateMap<Grade, GradeListResponse>();
        }
    }
}
