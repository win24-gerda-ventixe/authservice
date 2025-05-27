namespace Business.Services;

public class AuthResult
{
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; } = string.Empty;
    public string Token { get; private set; } = string.Empty;
    public static AuthResult Success(string token = "")
    {
        return new AuthResult
        {
            IsSuccess = true,
            Token = token
        };
    }
    public static AuthResult Failure(string error)
    {
        return new AuthResult
        {
            IsSuccess = false,
            Error = error
        };
    }
}