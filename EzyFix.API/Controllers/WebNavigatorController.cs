/*
using EzyFix.API.Constants;
using EzyFix.BLL.Services.Implements;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class WebNavigatorController : BaseController<WebNavigatorController>
    {
        private readonly ILogger<WebNavigatorController> _logger;
        private readonly IWebNavigatorService _webNavigatorService;
        public WebNavigatorController(ILogger<WebNavigatorController> logger, WebNavigatorService webNavigatorService) : base(logger)
        {
            _logger = logger;
            _webNavigatorService = webNavigatorService;

        }

        [HttpGet(ApiEndPointConstant.Navigator.SideBarEndpoint)]
        public async Task<IActionResult> GetSidebarElement()
        {
                var sidebarElement = await _webNavigatorService.GetSidebarElement();
                return Ok(ApiResponseBuilder.BuildResponse(StatusCodes.Status302Found,"Retrieve sidebar successful",sidebarElement));
        }
    }
}

*/
