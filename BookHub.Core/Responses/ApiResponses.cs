namespace BookHub.Core.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "")
            => new ApiResponse<T> { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(string message, IEnumerable<string> errors = null)
            => new ApiResponse<T> { Success = false, Message = message, Errors = errors };
    }
}
