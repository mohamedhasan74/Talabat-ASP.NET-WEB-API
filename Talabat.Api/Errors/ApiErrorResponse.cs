namespace Talabat.Api.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode {  get; set; }
        public string? Message { get; set; }
        public ApiErrorResponse(int StatusCode, string? Message = null)
        {
            this.StatusCode = StatusCode;
            this.Message = Message ?? DefaultMessageForStatusCode(StatusCode);
        }

        private string? DefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request, You Have Made",
                401 => "Authorized, You Are Not",
                404 => "Resource Was Not Found",
                500 => "Errors Are the Worest Thing",
                _ => null
            };
        }
    }
}
