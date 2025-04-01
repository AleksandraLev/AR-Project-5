#if UNITY_EDITOR
using UnityEngine;

public class EditorARCamera : MonoBehaviour
{
    private WebCamTexture webcamTexture;

    void Start()
    {
        webcamTexture = new WebCamTexture();
        var renderer = GetComponent<MeshRenderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }
}
#endif
