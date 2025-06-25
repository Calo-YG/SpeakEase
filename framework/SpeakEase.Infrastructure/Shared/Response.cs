namespace SpeakEase.Infrastructure.Shared;

[Serializable]
public class ResponseBase
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; protected set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 状态码
    /// </summary>
    public int Status { get; set; }
}

[Serializable]
public class Response<TResult> : ResponseBase
{
    public TResult Data { get; set; }

    public Response(TResult result)
    {
        Data = result;
        Success = true;
        Status = 200;
    }

    public Response(string errorMessage = "", int code = 500)
    {
        Success = false;
        Message = errorMessage;
        Status = code;
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

    public Response(string errorMessage, int code = 500) : base(errorMessage,code)
    {
    }

    public Response() : base()
    {
    }

    public static Response Sucess(object result)
    {
       return new Response(result);
    }

    public static Response Fail(string errorMessage,int errorCode)
    {
        return new Response(errorMessage,500);
    }
}