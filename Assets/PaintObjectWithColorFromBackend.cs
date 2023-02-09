using System;

public class PaintObjectWithColorFromBackend : DemoMonoBehaviour
{
    void Start()
    {
        BackendWithGrandpaCallbacks.FetchColor(color =>
        {
            SetColor(color);
        });
    }
}
