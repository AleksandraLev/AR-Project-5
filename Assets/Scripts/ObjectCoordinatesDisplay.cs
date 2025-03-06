using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ObjectCoordinatesDisplay : MonoBehaviour
{
    public string prefabName = "Root";
    private Transform targetObject;
    private Text coordinatesText;

    void Start()
    {
        coordinatesText = GetComponent<Text>();
        InvokeRepeating(nameof(FindTargetObject), 0, 1f); // Повторяем поиск раз в секунду
    }

    void FindTargetObject()
    {
        if (targetObject == null)
        {
            GameObject foundObject = GameObject.Find(prefabName);
            if (foundObject != null)
            {
                targetObject = foundObject.transform;
                Debug.Log($"[ObjectCoordinatesDisplay] Объект {prefabName} найден!");
                CancelInvoke(nameof(FindTargetObject)); // Остановить поиск
            }
        }
    }

    void Update()
    {
        if (targetObject != null && coordinatesText != null)
        {
            Vector3 position = targetObject.position;
            coordinatesText.text = $"X: {position.x:F2} Y: {position.y:F2} Z: {position.z:F2}";
        }
    }
}
