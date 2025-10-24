using EzyFix.API.Constants;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.Requests.Exams;
using EzyFix.DAL.Data.Responses.Exams;
using EzyFix.DAL.Data.MetaDatas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace EzyFix.API.Controllers
{
    [ApiController]
    public class ExamsController : BaseController<ExamsController>
    {
        private readonly IExamService _examService;
        private readonly IWebHostEnvironment _env;

        public ExamsController(ILogger<ExamsController> logger, IExamService examService, IWebHostEnvironment env) : base(logger)
        {
            _examService = examService;
            _env = env;
        }

        [HttpGet(ApiEndPointConstant.Exams.ExamsEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllExams()
        {
            var exams = await _examService.GetAllExamsAsync();
            return Ok(ApiResponseBuilder.BuildResponse(StatusCodes.Status200OK, "Exams retrieved", exams));
        }

        [HttpGet(ApiEndPointConstant.Exams.ExamEndpointById)]
        [ProducesResponseType(typeof(ApiResponse<ExamResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExamById(Guid id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse(StatusCodes.Status200OK, "Exam retrieved", exam));
        }

        [HttpPost(ApiEndPointConstant.Exams.CreateExamEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ExamResponse>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateExam([FromBody] CreateExamRequest createDto)
        {
            var created = await _examService.CreateExamAsync(createDto);
            return CreatedAtAction(nameof(GetExamById), new { id = created.ExamId }, ApiResponseBuilder.BuildResponse(StatusCodes.Status201Created, "Exam created", created));
        }

        [HttpPut(ApiEndPointConstant.Exams.UpdateExamEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<ExamResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateExam(Guid id, [FromBody] UpdateExamRequest updateDto)
        {
            var updated = await _examService.UpdateExamAsync(id, updateDto);
            return Ok(ApiResponseBuilder.BuildResponse(StatusCodes.Status200OK, "Exam updated", updated));
        }

        [HttpDelete(ApiEndPointConstant.Exams.DeleteExamEndpoint)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExam(Guid id)
        {
            await _examService.DeleteExamAsync(id);
            return Ok(ApiResponseBuilder.BuildResponse<object>(StatusCodes.Status200OK, "Exam deleted", null));
        }

        //// Upload file endpoint - saves file to wwwroot/uploads/exams and updates exam.FilePath / UploadedAt
        //[HttpPost(ApiEndPointConstant.Exams.UploadExamFile)]
        //[Consumes("multipart/form-data")]
        //[ProducesResponseType(typeof(ApiResponse<ExamResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UploadExamFile(Guid id, [FromForm] IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest(ApiResponseBuilder.BuildResponse<object>(StatusCodes.Status400BadRequest, "File is required", null));
        //    }

        //    var existing = await _examService.GetExamByIdAsync(id);
        //    if (existing == null)
        //    {
        //        return NotFound(ApiResponseBuilder.BuildResponse<object>(StatusCodes.Status404NotFound, "Exam not found", null));
        //    }

        //    var uploadsRoot = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "exams");
        //    Directory.CreateDirectory(uploadsRoot);

        //    var safeFileName = $"{id}_{Path.GetFileName(file.FileName)}";
        //    var fullPath = Path.Combine(uploadsRoot, safeFileName);

        //    using (var stream = new FileStream(fullPath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    // Save relative path
        //    var relativePath = Path.Combine("uploads", "exams", safeFileName).Replace("\\", "/");

        //    var updateDto = new UpdateExamRequest
        //    {
        //        Title = existing.Title,
        //        LecturerSubjectId = existing.LecturerSubjectId,
        //        FilePath = relativePath
        //    };

        //    var updated = await _examService.UpdateExamAsync(id, updateDto);

        //    return Ok(ApiResponseBuilder.BuildResponse(StatusCodes.Status200OK, "File uploaded and exam updated", updated));
        //}
    }
}
