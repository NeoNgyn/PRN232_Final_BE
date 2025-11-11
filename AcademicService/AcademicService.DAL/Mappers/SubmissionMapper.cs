using AcademicService.DAL.Data.Requests.Submission;
using AcademicService.DAL.Data.Responses.Submission;
using AcademicService.DAL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.DAL.Mappers
{
    public class SubmissionMapper : Profile
    {
        public SubmissionMapper()
        {
            
            CreateMap<CreateSubmissionRequest, Submission>();
            CreateMap<UpdateSubmissionRequest, Submission>();

            CreateMap<Submission, SubmissionListResponse>();
            CreateMap<Submission, SubmissionDetailResponse>();
        }
    }
}
