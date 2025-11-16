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
    public class ExamMapper : Profile
    {
        public ExamMapper()
        {
            // Define your mappings here if needed
            CreateMap<Exam, ExamResponse>()
    .ForMember(dest => dest.Semester, opt => opt.MapFrom(src => src.Semester))
    .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject));
            CreateMap<CreateExamRequest, Exam>();
            CreateMap<UpdateExamRequest, Exam>();
        }
    }
}
