using UnityEngine;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class CheckingCapabilities : MonoBehaviour
{
    bool ARCore = false;
    bool cameraPermission = false;

    public void Checker()
    {
#if UNITY_EDITOR
        SceneManager.LoadSceneAsync("ARScene", LoadSceneMode.Additive);
        return;
#endif
        ARCoreChecker();
        CameraPermissionChecker();
        if (ARCore && cameraPermission)
        {
            SceneManager.LoadSceneAsync("ARScene", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadSceneAsync("3DScene", LoadSceneMode.Additive);
        }
    }

    private void ARCoreChecker()
    {
        if (ARSession.state == ARSessionState.Unsupported || !IsARCoreSupported())
        {
            Debug.Log("ARCore не поддерживается");
            ARCore = false;
        }
        else
        {
            Debug.Log("ARCore поддерживается!");
            ARCore = true;
        }
    }

    private bool IsARCoreSupported()
    {
        var loader = (ARCoreLoader)UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager.activeLoader;
        return loader != null;
    }

    private void CameraPermissionChecker()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Нет разрешения на использование камеры, запрашиваем...");
            cameraPermission = false;
            Permission.RequestUserPermission(Permission.Camera);
        }
        else
        {
            Debug.Log("Разрешение на камеру уже есть!");
            cameraPermission = true;
        }
        if (!cameraPermission)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Debug.Log("Нет разрешения на использование камеры.");
                cameraPermission = false;
            }
            else
            {
                Debug.Log("Разрешение на камеру получено!");
                cameraPermission = true;
            }
        }
    }
}
