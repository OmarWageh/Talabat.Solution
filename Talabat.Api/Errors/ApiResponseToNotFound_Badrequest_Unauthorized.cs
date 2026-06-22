
namespace Talabat.Api.Errors
{
    public class ApiResponseToNotFound_Badrequest_Unauthorized
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponseToNotFound_Badrequest_Unauthorized(int statuscode ,string? message=null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefaultMassageForStatusCode(StatusCode);
        }

        private string? GetDefaultMassageForStatusCode( int StatusCode)
        {
            string message=string.Empty;
            switch (StatusCode)
            {
                case 400:
                    message= "A Bad Request";
                    break;
                case 401:
                    message = "Unauthorized";
                    break;
                case 404:
                    message = "Resoure Not Found";
                    break;
                case 500:
                    message = "Errors are the path to the Dark Side";
                    break;
                 default:
                    message= null;
                    break;
            }
            return message;

        }
    }
}
