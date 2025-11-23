using backend.Models.DTOS.Response;
using Microsoft.AspNetCore.Mvc;

namespace backend.Helpers.Results
{
    public class ApiResponseResult<T> : ObjectResult
    {
        public ApiResponseResult(ApiResponseDTO<T> apiResponse) : base(apiResponse)
        {
            StatusCode = apiResponse.Status;
        }
    }
}