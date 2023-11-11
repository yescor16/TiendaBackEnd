namespace Api.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
           return statusCode switch
           {
               400 => "A Bad Request, you have made",
               401 => "No authorized",
               404 => "Not Found",
               500 => "Error Internal Server",
               _ => null
           };
        }

       
    }
}
