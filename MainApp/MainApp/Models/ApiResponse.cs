namespace TestOrderMaker.Models
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public ApiResponse(T data)
        {
            Code = 200;
            IsSuccess = true;
            Data = data;
        }
        public ApiResponse(int code, string messageError)
        {
            Code = code;
            Message = messageError;
            IsSuccess = false;
        }
    }

}