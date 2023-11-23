namespace BlazingCuisine.Shared.Models
{
    public class AuthServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public bool IsAuthorized { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
