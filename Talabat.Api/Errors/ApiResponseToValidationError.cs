namespace Talabat.Api.Errors
{
    public class ApiResponseToValidationError: ApiResponseToNotFound_Badrequest_Unauthorized
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiResponseToValidationError():base(400)
        {
            Errors = new List<string>();
        }
       
      
    }
}
