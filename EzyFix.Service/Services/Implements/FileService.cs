using AutoMapper;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Implements
{
    public class FileService : BaseService<FileService>
    {
        public FileService(IUnitOfWork<AppDbContext> unitOfWork, ILogger<FileService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
    }
}
