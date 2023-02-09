using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem
{
    static Task LastTask = Task.CompletedTask;

    public static void BindButtonToHandler(Button button, Func<Task> handler)
    {
        button.onClick.AddListener(() =>
        {
            if (IsAllowedToInteract)
            {
                InteractionAllowedChanged?.Invoke(false);
                LastTask = handler();
                LastTask.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Debug.LogException(t.Exception);
                    }
                    InteractionAllowedChanged?.Invoke(true);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                Debug.Log("Prevented interaction because the last task has not completed.");
            }
        });
    }

    static bool IsAllowedToInteract => LastTask.IsCompleted;
    public static event Action<bool> InteractionAllowedChanged;
}