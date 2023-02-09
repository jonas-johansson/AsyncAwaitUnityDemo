using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public ProductCardController productCardPrefab;
    public GridLayoutGroup productCardGridLayoutGroup;

    void Start()
    {
        PopulateShopAsync().ReportErrors();
    }

    async Task PopulateShopAsync()
    {
        var products = await ShopModel.GetProductsAsync();
        foreach (var product in products)
        {
            CreateProductCard(product);
        }
    }

    void CreateProductCard(ProductModel product)
    {
        var productCard = Instantiate(productCardPrefab, productCardGridLayoutGroup.transform);
        productCard.Init(product, () => BuyButtonClickedAsync(product));
    }

    static async Task BuyButtonClickedAsync(ProductModel product)
    {
        try
        {
            await ShopModel.BuyProductAsync(product);
        }
        catch (Exception e)
        {
            await CommonDialogs.ShowOkDialogAsync("Failed", $"{e.Message}\n\nPlease try again later.");
        }
    }
}