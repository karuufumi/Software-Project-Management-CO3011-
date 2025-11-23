namespace backend.Models.DTOS.Response
{
    public class ApiResponseDTO<T>
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

    }
}