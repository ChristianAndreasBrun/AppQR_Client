using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using QRCodeReaderAndGenerator;

public class QRCodeReader : MonoBehaviour
{

    [SerializeField]
    RawImage rawImage;

    [SerializeField]
    TMP_InputField txtResult;

    //UnityAction

    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    }


    void OnEnable()
    {
        QRCodeManager.onError += HandleOnError;
        QRCodeManager.onQrCodeFound += HandleOnQRCodeFound;
    }

    void OnDisable()
    {
        QRCodeManager.onError -= HandleOnError;
        QRCodeManager.onQrCodeFound -= HandleOnQRCodeFound;
    }

    void HandleOnQRCodeFound(ZXing.BarcodeFormat barCodeType, string barCodeValue)
    {
        //Debug.Log(barCodeType + " __ " + barCodeValue);
        txtResult.text = barCodeValue;
        GetComponent<AppManager>().GetSingleProduct();
    }

    void HandleOnError(string err)
    {
        Debug.LogError(err);
    }

    public void ScanQRCode()
    {
        if (rawImage)
        {
            QRCodeManager.CameraSettings camSettings = new QRCodeManager.CameraSettings();
            string rearCamName = GetCustomCam();
            print(rearCamName);
            if (rearCamName != null)
            {
                camSettings.deviceName = rearCamName;
                print(camSettings.deviceName);
                camSettings.maintainAspectRatio = true;
                camSettings.scanType = ScanType.ONCE;
                QRCodeManager.Instance.ScanQRCode(camSettings, rawImage, 1f);
            }
        }
    }

    public void StopScanning()
    {
        QRCodeManager.Instance.StopScanning();
    }

    string GetRearCamName()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            return devices[i].name;
        }
        return null;
    }

    string GetCustomCam()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        return devices[GetComponent<AppManager>().listField.value].name;
    }

    public void OnPayloadGeneratorClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnQRCodeReader()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}
