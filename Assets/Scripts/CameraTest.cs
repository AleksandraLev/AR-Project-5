using UnityEngine;
using UnityEngine.UI;

public class CameraTest : MonoBehaviour
{
    public RawImage display;
    private WebCamTexture webcamTexture;

    void Start()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            webcamTexture = new WebCamTexture();
            display.texture = webcamTexture;
            webcamTexture.Play();
        }
        else
        {
            Debug.LogError("Нет доступных камер!");
        }
    }
}
