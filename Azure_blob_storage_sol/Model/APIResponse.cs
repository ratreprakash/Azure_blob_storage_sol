using System.Net;

namespace Azure_blob_storage_sol.Model
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public Boolean IsSuccess { get; set; }
        public List<string>? Message { get; set; }
        public object? Result { get; set; }
    }
}
