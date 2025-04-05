using System.Runtime.CompilerServices;

namespace SpeakEase.Infrastructure.Filters;

[Serializable]
public class ResponseBase
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; protected set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string ErrorMessage { get; set; }

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
        IsSuccess = true;
        Code = 200;
    }

    public Response(string errorMessage = "", int code = 500)
    {
        IsSuccess = false;
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