using backend.Helpers.Results;
using backend.Models.DTOS.Response;
using Microsoft.AspNetCore.Mvc;

namespace backend.Helpers.Extensions
{
    public static class ControllerExtension
    {
        public static ApiResponseResult<T> ApiOk<T>(this ControllerBase controller, T? data, string message = "Success")
            => new(new ApiResponseDTO<T> { Status = 200, Message = message, Data = data });
        public static ApiResponseResult<T> ApiCreated<T>(this ControllerBase controller, T? data, string message = "Created successfully")
                => new(new ApiResponseDTO<T> { Status = 201, Message = message, Data = data });

        public static ApiResponseResult<T> ApiBadRequest<T>(this ControllerBase controller, string message = "Bad Request", List<string>? errors = null)
            => new(new ApiResponseDTO<T> { Status = 400, Message = message, Errors = errors });

        public static ApiResponseResult<T> ApiNotFound<T>(this ControllerBase controller, string message = "Resource not found")
            => new(new ApiResponseDTO<T> { Status = 404, Message = message });

        public static ApiResponseResult<T> ApiUnauthorized<T>(this ControllerBase controller, string message = "Unauthorized")
            => new(new ApiResponseDTO<T> { Status = 401, Message = message });
        public static ApiResponseResult<T> ApiConflict<T>(this ControllerBase controller, string message = "Conflict")
                => new(new ApiResponseDTO<T> { Status = 409, Message = message });
    }
}