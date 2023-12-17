namespace Talabat.Api.Errors
{
    public class InvalidErrorResponse : ApiErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public InvalidErrorResponse() : base(404)
        {
            Errors = new List<string>();
        }
    }
}
