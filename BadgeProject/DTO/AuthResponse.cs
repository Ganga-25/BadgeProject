namespace BadgeProject.DTO
{
    public class AuthResponse
    {
        public int StatusCode { get; set; } 
        public string Message { get; set; } = null!;

        public AuthResponse(int statuscode,string message)
        {
            StatusCode = statuscode;
            Message = message;

        }
    }

    

}
