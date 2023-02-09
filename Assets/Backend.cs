using System;
using System.Threading.Tasks;
using UnityEngine;

class Backend
{
    public static async Task<Color> FetchColorAsync()
    {
        await RandomDelay.Wait();
        return RandomColor.Next();
    }
}

class BackendWithGrandpaCallbacks
{
    public static void FetchColor(Action<Color> doneCallback)
    {
        RandomDelay.Wait().ContinueWith(task =>
        {
            doneCallback(RandomColor.Next());
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
}