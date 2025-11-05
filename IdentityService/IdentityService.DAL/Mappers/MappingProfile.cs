using AutoMapper;
using IdentityService.DAL.Data.Requests.Roles;
using IdentityService.DAL.Data.Responses.Roles;
using IdentityService.DAL.Models;

namespace IdentityService.DAL.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Role, RoleResponse>();
            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();
        }
    } 
}
