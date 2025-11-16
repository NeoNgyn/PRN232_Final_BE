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
    public class SemesterMapper : Profile
    {
        public SemesterMapper()
        {
            // Semester mappings
            CreateMap<Semester, SemesterResponse>();
            CreateMap<CreateSemesterRequest, Semester>();
            CreateMap<UpdateSemesterRequest, Semester>().ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null)
                ); ;
        }
    }
}
