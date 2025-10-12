using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Semesters;
using EzyFix.DAL.Data.Responses.Semesters;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EzyFix.BLL.Services.Implements
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IMapper _mapper;

        public SemesterService(ISemesterRepository semesterRepository, IMapper mapper)
        {
            _semesterRepository = semesterRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<SemesterResponseDto>>> GetAllSemestersAsync()
        {
            try
            {
                var semesters = await _semesterRepository.GetAllAsync();
                var responseDto = _mapper.Map<IEnumerable<SemesterResponseDto>>(semesters);
                return ApiResponse<IEnumerable<SemesterResponseDto>>.SuccessResponse(responseDto, "Lấy danh sách học kỳ thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SemesterResponseDto>>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<SemesterResponseDto?>> GetSemesterByIdAsync(string id)
        {
            try
            {
                var semester = await _semesterRepository.GetByIdAsync(id);
                if (semester == null)
                {
                    return ApiResponse<SemesterResponseDto?>.FailResponse($"Không tìm thấy học kỳ với ID: {id}", 404);
                }
                var responseDto = _mapper.Map<SemesterResponseDto>(semester);
                return ApiResponse<SemesterResponseDto?>.SuccessResponse(responseDto, "Lấy thông tin học kỳ thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<SemesterResponseDto?>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<SemesterResponseDto>> CreateSemesterAsync(CreateSemesterRequestDto createDto)
        {
            try
            {
                // Kiểm tra xem ID đã tồn tại chưa (nếu cần)
                var existing = await _semesterRepository.GetByIdAsync(createDto.SemesterId);
                if (existing != null)
                {
                    return ApiResponse<SemesterResponseDto>.FailResponse($"Học kỳ với ID '{createDto.SemesterId}' đã tồn tại.", 409); // 409 Conflict
                }

                var semester = _mapper.Map<Semester>(createDto);
                await _semesterRepository.AddAsync(semester);
                await _semesterRepository.SaveChangesAsync();

                var responseDto = _mapper.Map<SemesterResponseDto>(semester);
                return ApiResponse<SemesterResponseDto>.SuccessResponse(responseDto, "Tạo học kỳ mới thành công.", 201); // 201 Created
            }
            catch (Exception ex)
            {
                return ApiResponse<SemesterResponseDto>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        // Tương tự cho Update và Delete...
        public async Task<ApiResponse<bool>> UpdateSemesterAsync(string id, UpdateSemesterRequestDto updateDto)
        {
            try
            {
                var semester = await _semesterRepository.GetByIdAsync(id);
                if (semester == null)
                {
                    return ApiResponse<bool>.FailResponse($"Không tìm thấy học kỳ với ID: {id}", 404);
                }

                _mapper.Map(updateDto, semester);
                _semesterRepository.Update(semester);
                await _semesterRepository.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Cập nhật học kỳ thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteSemesterAsync(string id)
        {
            try
            {
                var semester = await _semesterRepository.GetByIdAsync(id);
                if (semester == null)
                {
                    return ApiResponse<bool>.FailResponse($"Không tìm thấy học kỳ với ID: {id}", 404);
                }

                _semesterRepository.Delete(semester);
                await _semesterRepository.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Xóa học kỳ thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }
    }
}