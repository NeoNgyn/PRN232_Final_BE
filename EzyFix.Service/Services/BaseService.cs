using AutoMapper;
using EzyFix.DAL.Data;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EzyFix.BLL.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<AppDbContext> _unitOfWork;
        protected ILogger<T> _logger;
        protected IMapper _mapper;
        protected IHttpContextAccessor _httpContextAccessor;

        public BaseService(IUnitOfWork<AppDbContext> unitOfWork, ILogger<T> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
