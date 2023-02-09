using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductCardController : MonoBehaviour
{
    [SerializeField] Button buyButton;
    [SerializeField] TMP_Text title;
    [SerializeField] AsyncImage asyncImage;
    [SerializeField] List<GameObject> activeWhenOwned;
    [SerializeField] List<GameObject> activeWhenNotOwned;

    public void Init(ProductModel productModel, Func<Task> buyButtonClickHandler)
    {
        buyButton.SetHandler(buyButtonClickHandler);
        productModel.Changed += UpdateView;
        UpdateView(productModel);
    }

    void UpdateView(ProductModel productModel)
    {
        title.text = productModel.Title;
        asyncImage.Url = productModel.ProductImageUrl;

        activeWhenOwned.ForEach(go => go.SetActive(productModel.IsOwned));
        activeWhenNotOwned.ForEach(go => go.SetActive(!productModel.IsOwned));
    }
}