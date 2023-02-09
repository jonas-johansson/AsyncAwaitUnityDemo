using System;

class GrandpasOldSystem
{
    internal enum ErrorCode
    {
        None,
        Timeout,
        NetworkError,
        ServerError,
    }

    public static void FetchMessage(Action<string> successCallback, Action<ErrorCode> errorCallback)
    {
        successCallback("Hello from Grandpas old system");

        // Use this line instead to see the error handling in action.
        //errorCallback(ErrorCode.Timeout);
    }

    public static void FetchColor(Action<string> successCallback, Action<ErrorCode> errorCallback)
    {
        successCallback("Green");

        // Use this line instead to see the error handling in action.
        //errorCallback(ErrorCode.Timeout);
    }
}