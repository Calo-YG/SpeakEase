namespace SpeakEase.Infrastructure.Shared;

[Serializable]
public enum ResponseStatus
{
    Success,
    Unauthorized,
    Error
}

[Serializable]
public class ResponseBase
{
    public bool Success { get; protected set; }
    public ResponseStatus Status { get; protected set; }
    public string ErrorMessage { get; set; }
    public bool UnAuthorizedRequest { get; protected set; }

    /// <summary>
    /// 状态码
    /// </summary>
    public int Code { get; set; }
}

[Serializable]
public class Response<TResult> : ResponseBase
{
    public TResult Result { get; set; }

    public Response(TResult result)
    {
        Result = result;
        Success = true;
        Status = ResponseStatus.Success;
        Code = 200;
    }

    public Response(bool isUnauthorized = false, string errorMessage = "", int code = 500)
    {
        Success = false;
        Status = isUnauthorized ? ResponseStatus.Unauthorized : ResponseStatus.Error;
        UnAuthorizedRequest = isUnauthorized;
        ErrorMessage = errorMessage;
        Code = code;
    }

    public Response()
    {
    }
}

[Serializable]
public class Response : Response<object>
{
    public Response(object result) : base(result)
    {
    }

    public Response(bool isUnauthorized) : base(isUnauthorized)
    {
    }

    public Response() : base()
    {
    }
}