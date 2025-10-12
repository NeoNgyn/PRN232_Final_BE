using AutoMapper;
using EzyFix.BLL.Services.Interfaces;
using EzyFix.DAL.Data.MetaDatas;
using EzyFix.DAL.Data.Requests.Keywords;
using EzyFix.DAL.Data.Responses.Keywords;
using EzyFix.DAL.Models;
using EzyFix.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzyFix.BLL.Services.Implements
{
    public class KeywordService : IKeywordService
    {
        private readonly IKeywordRepository _keywordRepository;
        private readonly IMapper _mapper;

        public KeywordService(IKeywordRepository keywordRepository, IMapper mapper)
        {
            _keywordRepository = keywordRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<KeywordResponseDto>>> GetAllKeywordsAsync()
        {
            try
            {
                var keywords = await _keywordRepository.GetAllAsync();
                var responseDto = _mapper.Map<IEnumerable<KeywordResponseDto>>(keywords);
                return ApiResponse<IEnumerable<KeywordResponseDto>>.SuccessResponse(responseDto, "Lấy danh sách từ khóa thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<KeywordResponseDto>>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<KeywordResponseDto?>> GetKeywordByIdAsync(int keywordId)
        {
            try
            {
                var keyword = await _keywordRepository.GetByIdAsync(keywordId);
                if (keyword == null)
                {
                    return ApiResponse<KeywordResponseDto?>.FailResponse($"Không tìm thấy từ khóa với ID: {keywordId}", 404);
                }
                var responseDto = _mapper.Map<KeywordResponseDto>(keyword);
                return ApiResponse<KeywordResponseDto?>.SuccessResponse(responseDto, "Lấy thông tin từ khóa thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<KeywordResponseDto?>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<KeywordResponseDto>> CreateKeywordAsync(CreateKeywordRequestDto createDto)
        {
            try
            {
                // Lưu ý: Không có kiểm tra trùng lặp vì ID là tự tăng (int).
                var keyword = _mapper.Map<Keyword>(createDto);
                await _keywordRepository.AddAsync(keyword);
                await _keywordRepository.SaveChangesAsync();

                var responseDto = _mapper.Map<KeywordResponseDto>(keyword);
                return ApiResponse<KeywordResponseDto>.SuccessResponse(responseDto, "Tạo từ khóa mới thành công.", 201);
            }
            catch (Exception ex)
            {
                return ApiResponse<KeywordResponseDto>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateKeywordAsync(int keywordId, UpdateKeywordRequestDto updateDto)
        {
            try
            {
                var keyword = await _keywordRepository.GetByIdAsync(keywordId);
                if (keyword == null)
                {
                    return ApiResponse<bool>.FailResponse($"Không tìm thấy từ khóa với ID: {keywordId}", 404);
                }

                _mapper.Map(updateDto, keyword);
                _keywordRepository.Update(keyword);
                await _keywordRepository.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Cập nhật từ khóa thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteKeywordAsync(int keywordId)
        {
            try
            {
                var keyword = await _keywordRepository.GetByIdAsync(keywordId);
                if (keyword == null)
                {
                    return ApiResponse<bool>.FailResponse($"Không tìm thấy từ khóa với ID: {keywordId}", 404);
                }

                _keywordRepository.Delete(keyword);
                await _keywordRepository.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Xóa từ khóa thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse($"Lỗi hệ thống: {ex.Message}", 500);
            }
        }
    }
}
