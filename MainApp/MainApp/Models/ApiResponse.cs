using System.Net;

namespace TestOrderMaker.Models
{
    public class ApiResponse<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public ApiResponse(T data)
        {
            Code = HttpStatusCode.OK;
            IsSuccess = true;
            Data = data;
        }
        public ApiResponse(HttpStatusCode code, string messageError)
        {
            Code = code;
            Message = messageError;
            IsSuccess = false;
        }
    }

}