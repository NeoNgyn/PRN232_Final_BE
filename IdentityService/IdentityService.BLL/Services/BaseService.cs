using AutoMapper;
using IdentityService.DAL.Models;
using IdentityService.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityService.BLL.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<IdentityDbContext> _unitOfWork;
        protected ILogger<T> _logger;
        protected IMapper _mapper;
        protected IHttpContextAccessor _httpContextAccessor;

        public BaseService(IUnitOfWork<IdentityDbContext> unitOfWork, ILogger<T> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
