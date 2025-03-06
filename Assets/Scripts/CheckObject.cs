using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation.Samples;

public class CheckObject : MonoBehaviour
{
    [SerializeField] List<Button> Buttons = new List<Button>();
    [SerializeField] List<GameObject> ARobjects = new List<GameObject>();
    [SerializeField] GameObject XROrigin;

    private PlaceOnPlane placeOnPlane;  // Компонент, который управляет PlacedPrefab

    void Start()
    {
        // Найдем компонент PlaceOnPlane в нужном объекте
        placeOnPlane = XROrigin.GetComponent<PlaceOnPlane>();

        // Подписываемся на события кнопок
        for (int i = 0; i < Buttons.Count; i++)
        {
            int index = i;  // Сохраняем индекс кнопки для использования в обработчике
            Buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    // Метод для обработки нажатия на кнопку
    private void OnButtonClicked(int index)
    {
        if (index >= 0 && index < ARobjects.Count)
        {
            // Меняем Prefab в компоненте PlaceOnPlane
            placeOnPlane.placedPrefab = ARobjects[index];
            Debug.Log("Выбран объект в CheckObject (для установки): " + ARobjects[index].name);
        }
    }
    public void MakePlacedPrefabNull()
    {
        placeOnPlane.placedPrefab = null;
        Destroy(placeOnPlane.placedPrefab);
    }
}
