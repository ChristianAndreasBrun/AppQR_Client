using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public partial class AppManager : MonoBehaviour
{
    string urlGetProducts = "http://localhost:8080/appqr/getproducts.php";
    string urlGetSingleProduct = "http://localhost:8080/appqr/singleproduct.php";
    string urlImages = "http://localhost:8080/appqr/images/";
    string imageExtension = ".png";

    [Header("Inputs")]
    public TMP_InputField referenceField;
    public TMP_Dropdown listField;

    [Header("List of Products")]
    public Product[] allProducts;


    void Start()
    {
        SetPage(0);
        SelectCamera();
        //StartCoroutine(c_GetAllProducts());
    }


    void SelectCamera()
    {
        WebCamDevice[] webcam = WebCamTexture.devices;
        List<string> webcamNames = new List<string>();
        for (int i = 0; i < webcam.Length; i++)
        {
            webcamNames.Add(webcam[i].name);
        }
        listField.ClearOptions();
        listField.AddOptions(webcamNames);
    }

    public void GetSingleProduct()
    {
        if (referenceField.text.Equals("") == false)
        {
            SetPage(-1);
            StartCoroutine(c_GetSingleProduct());
        }
    }

    public void BackToMenu()
    {
        SetPage(0);
    }

    IEnumerator c_GetSingleProduct()
    {
        WWWForm form = new WWWForm();
        form.AddField("ref", referenceField.text);

        UnityWebRequest request = UnityWebRequest.Post(urlGetSingleProduct, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
            SetPage(0);
            yield return 0;
        }
        else
        {
            string jsonData = request.downloadHandler.text;
            if (jsonData.Equals("none"))
            {
                // No se puede obtener el producto
                Debug.Log($"El producto con referencia: {referenceField.text}, no existe.");
                SetPage(0);
            }
            else
            {
                RootObject obj = JsonUtility.FromJson<RootObject>("{\"products\":" + jsonData + "}");
                allProducts = obj.products;
                SetProduct();
                SetPage(1);
            }
        }
    }

    IEnumerator c_GetAllProducts()
    {
        UnityWebRequest request = UnityWebRequest.Get(urlGetProducts);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
            yield return 0;
        }
        else
        {
            string jsonData = request.downloadHandler.text;
            RootObject obj = JsonUtility.FromJson<RootObject>("{\"products\":" + jsonData + "}");
            allProducts = obj.products;
        }
    }
}


[System.Serializable]
public class Product
{
    public int id;
    public string name;

    [TextArea(3, 10)]
    public string description;

    public string category;
    public float price;
    public string reference;
}


[System.Serializable]
public class RootObject
{
    public Product[] products;
}