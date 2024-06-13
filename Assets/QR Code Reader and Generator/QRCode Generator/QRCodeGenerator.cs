using System;
using UnityEngine;
using System.Collections;
using System.IO;
using QRCodeReaderAndGenerator;
using UnityEngine.UI;

public class QRCodeGenerator : MonoBehaviour
{

    [SerializeField]
    InputField inputField;

    [SerializeField]
    RawImage image;

    ZXing.BarcodeFormat codeFormat = ZXing.BarcodeFormat.QR_CODE;

    void OnEnable()
    {
        QRCodeManager.onError += QRCodeManager_onError;
    }

    void QRCodeManager_onError(string err)
    {
        Debug.Log(err);
    }

    public void GenerateQRCode()
    {
        if (inputField && image)
        {
            image.texture = QRCodeManager.Instance.GenerateQRCode(inputField.text, codeFormat);
        }
    }

    public void OnPayloadGeneratorClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnGenerateQRCode()
    {
        codeFormat = ZXing.BarcodeFormat.QR_CODE;
    }

    public void OnGenerateCODE_128()
    {
        codeFormat = ZXing.BarcodeFormat.CODE_128;
    }
}
