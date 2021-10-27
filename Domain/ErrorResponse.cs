namespace Domain
{
    public class ErrorResponse
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorResponse(string statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}