namespace Talabat.Api.Errors
{
    public class ExceptionErrorResponse : ApiErrorResponse
    {
        public string? Details {  get; set; }
        public ExceptionErrorResponse(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {
            this.Details = details;
        }
    }
}
