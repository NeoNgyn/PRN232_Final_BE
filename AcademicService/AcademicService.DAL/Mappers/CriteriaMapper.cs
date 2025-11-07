using AcademicService.DAL.Data.Requests.Criteria;
using AcademicService.DAL.Data.Responses.Criteria;
using AcademicService.DAL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Mappers
{
    public class CriteriaMapper : Profile
    {
        public CriteriaMapper()
        {
            CreateMap<CreateCriteriaRequest, Criteria>();
            CreateMap<UpdateCriteriaRequest, Criteria>();

            CreateMap<Criteria, CriteriaListResponse>();
        }
    }
}
