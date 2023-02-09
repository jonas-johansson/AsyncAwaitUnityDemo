using System.Threading.Tasks;

class ModernFacade
{






    public static Task<string> FetchMessageAsync()
    {
        var taskCompletionSource = new TaskCompletionSource<string>();

        GrandpasOldSystem.FetchMessage(
            successCallback: message =>
            {
                taskCompletionSource.SetResult(message);
            },
            errorCallback: errorCode =>
            {
                taskCompletionSource.SetException(new GrandpaException(errorCode.ToString()));
            }
        );

        return taskCompletionSource.Task;
    }








    public static Task<string> FetchColorAsync()
    {
        var taskCompletionSource = new TaskCompletionSource<string>();

        GrandpasOldSystem.FetchColor(
            successCallback: message =>
            {
                taskCompletionSource.SetResult(message);
            },
            errorCallback: errorCode =>
            {
                taskCompletionSource.SetException(new GrandpaException(errorCode.ToString()));
            }
        );

        return taskCompletionSource.Task;
    }
}