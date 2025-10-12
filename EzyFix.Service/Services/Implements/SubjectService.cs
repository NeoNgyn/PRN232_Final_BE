using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Subjects;
using EzyFix.DAL.Data.Responses.Subjects;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Implements
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectService(ISubjectRepository subjectRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<SubjectResponseDto>>> GetAllSubjectsAsync()
        {
            try
            {
                var subjects = await _subjectRepository.GetAllAsync();
                var responseDto = _mapper.Map<IEnumerable<SubjectResponseDto>>(subjects);
                return ApiResponse<IEnumerable<SubjectResponseDto>>.SuccessResponse(responseDto, "Lấy danh sách môn học thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SubjectResponseDto>>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<SubjectResponseDto?>> GetSubjectByIdAsync(string subjectId)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(subjectId);
                if (subject == null)
                {
                    return ApiResponse<SubjectResponseDto?>.FailResponse($"Không tìm thấy môn học với ID: {subjectId}", 404);
                }
                var responseDto = _mapper.Map<SubjectResponseDto>(subject);
                return ApiResponse<SubjectResponseDto?>.SuccessResponse(responseDto, "Lấy thông tin môn học thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<SubjectResponseDto?>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<SubjectResponseDto>> CreateSubjectAsync(CreateSubjectRequestDto createSubjectDto)
        {
            try
            {
                var existingSubject = await _subjectRepository.GetByIdAsync(createSubjectDto.SubjectId);
                if (existingSubject != null)
                {
                    return ApiResponse<SubjectResponseDto>.FailResponse($"Môn học với ID '{createSubjectDto.SubjectId}' đã tồn tại.", 409); // 409 Conflict
                }

                var subject = _mapper.Map<Subject>(createSubjectDto);
                await _subjectRepository.AddAsync(subject);
                await _subjectRepository.SaveChangesAsync(); // Lưu thay đổi vào DB

                var responseDto = _mapper.Map<SubjectResponseDto>(subject);
                return ApiResponse<SubjectResponseDto>.SuccessResponse(responseDto, "Tạo môn học mới thành công.", 201); // 201 Created
            }
            catch (Exception ex)
            {
                return ApiResponse<SubjectResponseDto>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateSubjectAsync(string subjectId, UpdateSubjectRequestDto updateSubjectDto)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(subjectId);
                if (subject == null)
                {
                    return ApiResponse<bool>.FailResponse($"Không tìm thấy môn học với ID: {subjectId}", 404);
                }

                _mapper.Map(updateSubjectDto, subject);
                _subjectRepository.Update(subject); // Chỉ đánh dấu là đã thay đổi
                await _subjectRepository.SaveChangesAsync(); // Lưu thay đổi vào DB

                return ApiResponse<bool>.SuccessResponse(true, "Cập nhật môn học thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteSubjectAsync(string subjectId)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(subjectId);
                if (subject == null)
                {
                    return ApiResponse<bool>.FailResponse($"Không tìm thấy môn học với ID: {subjectId}", 404);
                }

                _subjectRepository.Delete(subject); // Chỉ đánh dấu là đã xóa
                await _subjectRepository.SaveChangesAsync(); // Lưu thay đổi vào DB

                return ApiResponse<bool>.SuccessResponse(true, "Xóa môn học thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }
    }
}
