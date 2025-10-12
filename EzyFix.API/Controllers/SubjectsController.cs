using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Requests.Subjects;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        // GET: api/subjects
        [HttpGet]
        public async Task<IActionResult> GetAllSubjects()
        {
            var response = await _subjectService.GetAllSubjectsAsync();
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/subjects/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById(string id)
        {
            var response = await _subjectService.GetSubjectByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/subjects
        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequestDto createDto)
        {
            var response = await _subjectService.CreateSubjectAsync(createDto);
            return StatusCode(response.StatusCode, response);
        }

        // PUT: api/subjects/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(string id, [FromBody] UpdateSubjectRequestDto updateDto)
        {
            var response = await _subjectService.UpdateSubjectAsync(id, updateDto);
            return StatusCode(response.StatusCode, response);
        }

        // DELETE: api/subjects/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(string id)
        {
            var response = await _subjectService.DeleteSubjectAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
