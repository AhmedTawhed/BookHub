namespace BookHub.Core.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public static ApiResponse<T> Ok(T? data = default, string message = "")
            => new()
            { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(string message, IEnumerable<string>? errors = null)
            => new()
            { Success = false, Message = message, Errors = errors ?? Enumerable.Empty<string>() };
    }
}
