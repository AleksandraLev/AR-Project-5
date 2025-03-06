using UnityEngine;
using UnityEngine.SceneManagement;

public class _3DandARSceneManager: MonoBehaviour
{
    public void OpenScene ()
    {
        SceneManager.LoadSceneAsync("3DScene", LoadSceneMode.Additive);
    }
    public void CloseScene()
    {
        SceneManager.UnloadSceneAsync("3DScene");
        SceneManager.UnloadSceneAsync("ARScene");
        SceneManager.UnloadSceneAsync("3DScene");
        SceneManager.UnloadSceneAsync("ARScene");
    }
}
