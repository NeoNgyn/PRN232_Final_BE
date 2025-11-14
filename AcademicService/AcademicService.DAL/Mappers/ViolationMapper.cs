using AcademicService.DAL.Data.Requests.Violation;
using AcademicService.DAL.Data.Responses.Violation;
using AcademicService.DAL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Mappers
{
    public class ViolationMapper : Profile
    {
        public ViolationMapper()
        {
            CreateMap<CreateViolationRequest, Violation>();
            CreateMap<UpdateViolationRequest, Violation>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null)
                );

            CreateMap<Violation, ViolationListResponse>();
        }
    }
}
