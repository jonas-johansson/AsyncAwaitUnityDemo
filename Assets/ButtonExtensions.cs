using System;
using System.Threading.Tasks;
using UnityEngine.UI;

public static class ButtonExtensions
{
    public static void SetHandler(this Button button, Func<Task> handler)
    {
        InteractionSystem.BindButtonToHandler(button, handler);
    }
}