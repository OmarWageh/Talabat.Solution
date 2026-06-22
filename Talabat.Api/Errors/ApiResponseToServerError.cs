namespace Talabat.Api.Errors
{
    public class ApiResponseToServerError: ApiResponseToNotFound_Badrequest_Unauthorized
    { 
        public string _Details { get; set; }
        public ApiResponseToServerError(int statusCode,string Message,string Details) :base(statusCode, Message)
        {
            _Details=Details;
        }
        public ApiResponseToServerError(int statusCode) : base(500)
        {
           
        }

    }
}
