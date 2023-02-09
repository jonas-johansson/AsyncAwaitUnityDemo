using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShopModel
{
    public static async Task<List<ProductModel>> GetProductsAsync()
    {
        await Task.Delay(100);

        return new List<ProductModel>()
        {
            new()
            {
                Title = "Apple",
                ProductImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/Red_Apple.jpg/1200px-Red_Apple.jpg"
            },
            new()
            {
                Title = "Banana",
                ProductImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8a/Banana-Single.jpg/1200px-Banana-Single.jpg"
            },
            new()
            {
                Title = "Orange",
                ProductImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/43/Ambersweet_oranges.jpg/1280px-Ambersweet_oranges.jpg"
            },
        };
    }

    public static async Task BuyProductAsync(ProductModel product)
    {
        Debug.Log($"Attempting to buy {product.Title}...");
        await Task.Delay(2000);

        // Pretend that there's an exception when attempting to buy an orange.
        if (product.Title.Contains("Orange"))
            throw new BackendException();

        Debug.Log($"Successfully bought {product.Title}");
        product.IsOwned = true;
    }
}