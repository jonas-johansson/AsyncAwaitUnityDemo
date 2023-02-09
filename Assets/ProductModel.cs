using System;

public class ProductModel
{
    public string Title { get; set; }
    public string ProductImageUrl { get; set; }

    public bool IsOwned
    {
        get => isOwned;
        set
        {
            isOwned = value;
            OnChanged();
        }
    }

    public string ProductId => Title;

    public event Action<ProductModel> Changed;
    protected void OnChanged() => Changed?.Invoke(this);
    bool isOwned;
}