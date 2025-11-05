namespace IdentityService.DAL.Data.MetaDatas
{
    public class ApiResponseBuilder
    {
        public static ApiResponse<T> BuildResponse<T>(int statusCode, string message, T data, string? reason = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data,
                IsSuccess = statusCode >= 200 && statusCode < 300,
                Reason = reason
            };
        }

        public static ApiResponse<T> BuildErrorResponse<T>(T data, int statusCode, string message, string reason)
        {
            return new ApiResponse<T>
            {
                Data = data,
                StatusCode = statusCode,
                Message = message,
                Reason = reason,
                IsSuccess = false
            };
        }
    }
}
