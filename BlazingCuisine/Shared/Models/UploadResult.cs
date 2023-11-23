namespace BlazingCuisine.Shared.Models
{
    public class UploadResult
    {
        public bool IsUploaded { get; set; }
        public bool IsAuthorized { get; set; } = true;
        public string? FileName { get; set; }
        public string? StoredFileName { get; set; }
        public string? Message { get; set; }
    }
}
