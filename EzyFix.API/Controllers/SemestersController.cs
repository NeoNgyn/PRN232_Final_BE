using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Semesters;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemestersController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSemesters()
        {
            var response = await _semesterService.GetAllSemestersAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSemesterById(string id)
        {
            var response = await _semesterService.GetSemesterByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterRequestDto createDto)
        {
            // KHÔNG CẦN KIỂM TRA VALIDATION.
            // Nếu DTO không hợp lệ, request sẽ tự động bị chặn và trả về lỗi 400 ở bước trước.

            var response = await _semesterService.CreateSemesterAsync(createDto);          

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSemester(string id, [FromBody] UpdateSemesterRequestDto updateDto)
        {
            // KHÔNG CẦN KIỂM TRA VALIDATION.
            var response = await _semesterService.UpdateSemesterAsync(id, updateDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemester(string id)
        {
            var response = await _semesterService.DeleteSemesterAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}