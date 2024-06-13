using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public partial class AppManager
{
    public GameObject[] pages;

    [Header("Info Fields")]
    public TextMeshProUGUI nameField;
    public Image productImage;
    public TextMeshProUGUI categoryField;
    public TextMeshProUGUI refField;
    public TextMeshProUGUI descriptionField;
    public TextMeshProUGUI priceField;


    public void SetPage(int page)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == page);
        }
    }

    void SetProduct()
    {
        nameField.text = allProducts[0].name;
        categoryField.text = allProducts[0].category;
        refField.text = allProducts[0].reference;
        descriptionField.text = allProducts[0].description;
        priceField.text = allProducts[0].price.ToString("00.00") + " $";

        StartCoroutine(c_LoadImage());
    }

    IEnumerator c_LoadImage()
    {
        string url = urlImages + allProducts[0].reference + imageExtension;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
            yield return 0;
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            productImage.sprite = sprite;
        }
    }
}
