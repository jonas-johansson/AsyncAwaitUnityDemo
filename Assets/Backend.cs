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

    public static async Task BuyProductViaRestApiAsync(string productId)
    {
        Debug.Log($"Attempting to buy {productId}...");
        await Task.Delay(2000);

        // Pretend that there's an exception when attempting to buy an orange.
        if (productId.Contains("Orange"))
            throw new BackendException();

        Debug.Log($"Successfully bought {productId}");
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