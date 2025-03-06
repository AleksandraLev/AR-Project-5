using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARVisualizationController : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public ARPointCloudManager pointCloudManager;
    public Toggle planeToggle;
    public Toggle pointCloudToggle;

    void Start()
    {
        // Устанавливаем начальные значения
        planeToggle.isOn = planeManager.enabled;
        pointCloudToggle.isOn = pointCloudManager.enabled;

        // Добавляем слушатели событий
        planeToggle.onValueChanged.AddListener(TogglePlanes);
        pointCloudToggle.onValueChanged.AddListener(TogglePointCloud);
    }

    void TogglePlanes(bool isOn)
    {
        planeManager.enabled = isOn;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(isOn);
        }
    }

    void TogglePointCloud(bool isOn)
    {
        pointCloudManager.enabled = isOn;
        foreach (var point in pointCloudManager.trackables)
        {
            point.gameObject.SetActive(isOn);
        }
    }
}
