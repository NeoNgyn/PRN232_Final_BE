using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Requests.Keywords;
using Microsoft.AspNetCore.Mvc;

namespace EzyFix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordsController : ControllerBase
    {
        private readonly IKeywordService _keywordService;

        public KeywordsController(IKeywordService keywordService)
        {
            _keywordService = keywordService;
        }

        // GET: api/keywords
        [HttpGet]
        public async Task<IActionResult> GetAllKeywords()
        {
            var response = await _keywordService.GetAllKeywordsAsync();
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/keywords/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKeywordById(int id)
        {
            var response = await _keywordService.GetKeywordByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/keywords
        [HttpPost]
        public async Task<IActionResult> CreateKeyword([FromBody] CreateKeywordRequestDto createDto)
        {
            var response = await _keywordService.CreateKeywordAsync(createDto);
            return StatusCode(response.StatusCode, response);
        }

        // PUT: api/keywords/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKeyword(int id, [FromBody] UpdateKeywordRequestDto updateDto)
        {
            var response = await _keywordService.UpdateKeywordAsync(id, updateDto);
            return StatusCode(response.StatusCode, response);
        }

        // DELETE: api/keywords/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKeyword(int id)
        {
            var response = await _keywordService.DeleteKeywordAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
