using System.Threading.Tasks;
using UnityEngine;

public static class TaskExtensions
{
    public static void TrustMeJustLetItRipYo(this Task task)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Debug.LogException(t.Exception);
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
}