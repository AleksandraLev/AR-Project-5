using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

public class ImageTargetController2 : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private Button fixButton;  // Кнопка "Зафиксировать"
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject trackedImagePrefab;

    private Dictionary<Guid, GameObject> _spawnedObjects = new Dictionary<Guid, GameObject>();
    private HashSet<Guid> _fixedObjects = new HashSet<Guid>(); // Храним зафиксированные объекты
    private GameObject _selectedObject = null; // Выбранный объект

    void Start()
    {
        fixButton.onClick.AddListener(ToggleFixation);
    }

    void OnEnable()
    {
        _aRTrackedImageManager.trackedImagesChanged += ArTrackedImageManagerOnTrackedImagesChanged;
    }

    void OnDisable()
    {
        _aRTrackedImageManager.trackedImagesChanged -= ArTrackedImageManagerOnTrackedImagesChanged;
    }

    private void ArTrackedImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (var addedImage in obj.added)
        {
            print(message: $"Added image: {addedImage.referenceImage.name}");
            text.text = $"Added image: {addedImage.referenceImage.name}";
            UpdatePrefab(addedImage);
        }
        foreach (var updatedImage in obj.updated)
        {
            if (!_fixedObjects.Contains(updatedImage.referenceImage.guid))
            {
                //print($"Updated image: {updatedImage.referenceImage.name}");
                //text.text = $"Updated image: {updatedImage.referenceImage.name}";
                UpdatePrefab(updatedImage);
            }
        }
    }

    private void UpdatePrefab(ARTrackedImage trackedImage)
    {
        Guid imageGuid = trackedImage.referenceImage.guid;

        // Проверяем, зафиксирован ли объект
        if (_fixedObjects.Contains(imageGuid))
        {
            Debug.Log($"Объект {imageGuid} зафиксирован и не обновляется.");
            text.text = $"Объект {imageGuid} зафиксирован и не обновляется.";
            return; // Если объект зафиксирован, не обновляем его позицию
        }

        //var lockable = trackedImage.GetComponent<LockableTrackedImage>();

        //if (lockable == null)
        //{
        //    lockable = trackedImage.gameObject.AddComponent<LockableTrackedImage>();
        //}

        //if (lockable.IsLocked)
        //{
        //    Debug.Log($"Объект {imageGuid} зафиксирован и не обновляется.");
        //    text.text = $"Объект {imageGuid} зафиксирован и не обновляется.";
        //    return;
        //}

        // Попытка найти объект, который уже был создан AR Foundation (через trackedImagePrefab)
        //if (_spawnedObjects.ContainsKey(imageGuid))
        //{
        //    var existingObject = _spawnedObjects[imageGuid];
        //    existingObject.transform.position = trackedImage.transform.position;
        //    existingObject.transform.rotation = trackedImage.transform.rotation;
        //}
        //else
        //{
        //    // Если объект не существует в списке _spawnedObjects, значит он был создан AR Foundation
        //    var existingObject = Instantiate(trackedImagePrefab);

        //    // Добавляем его в наш список отслеживаемых объектов
        //    _spawnedObjects[imageGuid] = existingObject;

        //    // Обновляем позицию и вращение
        //    existingObject.transform.position = trackedImage.transform.position;
        //    existingObject.transform.rotation = trackedImage.transform.rotation;
        //}

        // Проверяем, есть ли уже объект для этого изображения
        if (!_spawnedObjects.ContainsKey(trackedImage.referenceImage.guid))
        {
            // Создаем объект, если его ещё нет
            var spawnedObject = Instantiate(trackedImagePrefab);
            _spawnedObjects[trackedImage.referenceImage.guid] = spawnedObject;
        }

        // Обновляем позицию и ориентацию объекта
        var existingObject = _spawnedObjects[trackedImage.referenceImage.guid];
        existingObject.transform.position = trackedImage.transform.position;
        existingObject.transform.rotation = trackedImage.transform.rotation;
    }


    void Update()
    {
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                SelectObject(Input.GetTouch(0).position);
                //Debug.Log("Телефон");
               // text.text = "Телефон";
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectObject(Input.mousePosition);
                //Debug.Log("Компьютер");
                //text.text = "Компьютер";
            }
        }
    }


    private void SelectObject(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (_spawnedObjects.ContainsValue(hit.transform.gameObject))
            {
                _selectedObject = hit.transform.gameObject;
                Debug.Log($"Выбран объект: {_selectedObject.name}");
                text.text = $"Выбран объект: {_selectedObject.name}";
            }
        }
    }

    public void ToggleFixation()
    {
        //Debug.Log($"Метод ToggleFixation");
        text.text = "Метод ToggleFixation";
        if (_selectedObject == null) return;

        Guid key = Guid.Empty;
        foreach (var pair in _spawnedObjects)
        {
            if (pair.Value == _selectedObject)
            {
                key = pair.Key;
                break;
            }
        }

        if (key != Guid.Empty)
        {
            if (_fixedObjects.Contains(key))
            {
                _fixedObjects.Remove(key);
                Debug.Log($"Объект {key} отвязан");
                //text.text = $"Объект {key} отвязан";
                text.text = $"Объект {_selectedObject.name} отвязан";
            }
            else
            {
                _fixedObjects.Add(key);
                Debug.Log($"Объект {key} зафиксирован");
                //text.text = $"Объект {key} зафиксирован";
                text.text = $"Объект {_selectedObject.name} зафиксирован";
            }
        }

        //if (_selectedObject == null) return;

        //var lockable = _selectedObject.GetComponent<LockableTrackedImage>();

        //if (lockable != null)
        //{
        //    if (lockable.IsLocked)
        //    {
        //        lockable.IsLocked = false;
        //        text.text = "Объект отвязан";

        //    }
        //    else
        //    {
        //        lockable.IsLocked = true;
        //        text.text = "Объект зафиксирован";
        //    }
        //    //lockable.IsLocked = !lockable.IsLocked;
        //    //text.text = lockable.IsLocked ? "Объект зафиксирован" : "Объект отвязан";
        //    Debug.Log(text.text);
        //}
        //else
        //{
        //    text.text = "lockable != null";
        //}
    }
}
