namespace Dentlike.Api.DTOs
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public Meta? Meta { get; set; }

        public ApiResponse(int code, T? data, string? message = null, Meta? meta = null)
        {
            Data = data;
            Message = message;
            Code = code;
            Meta = meta;
        }
    }

    public class Meta
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
