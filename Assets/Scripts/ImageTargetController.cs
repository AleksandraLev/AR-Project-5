using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTargetController : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    //[SerializeField] private XRReferenceImageLibrary referenceLibrary;
    private GameObject _spawnedObject;
    private Dictionary<Guid, GameObject> _spawnedObjects = new Dictionary<Guid, GameObject>();
    

    void OnEnable()
    {
        _aRTrackedImageManager.trackedImagesChanged += ArTrackedImageManagerOntrakedImagesChanged;
        Debug.Log("_aRTrackedImageManager.trackedImagesChanged += ArTrackedImageManagerOntrakedImagesChanged");
    }

    void OnDisable()
    {
        _aRTrackedImageManager.trackedImagesChanged -= ArTrackedImageManagerOntrakedImagesChanged;
        Debug.Log("_aRTrackedImageManager.trackedImagesChanged -= ArTrackedImageManagerOntrakedImagesChanged");

    }

    private void ArTrackedImageManagerOntrakedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (var addedImage in obj.added)
        {
            print(message: $"Added image: {addedImage.referenceImage.name}");
            UpdatePrefab(addedImage);
        }
        foreach (var updatedImage in obj.updated)
        {
            print(message: $"Updated image: {updatedImage.referenceImage.name}");
            UpdatePrefab(updatedImage);
        }
    }

    private void UpdatePrefab(ARTrackedImage trackedImage)
    {
        // Проверяем, есть ли уже объект для этого изображения
        if (!_spawnedObjects.ContainsKey(trackedImage.referenceImage.guid))
        {
            // Создаем объект, если его ещё нет
            var spawnedObject = Instantiate(_aRTrackedImageManager.trackedImagePrefab);
            _spawnedObjects[trackedImage.referenceImage.guid] = spawnedObject;
        }

        // Обновляем позицию и ориентацию объекта
        var existingObject = _spawnedObjects[trackedImage.referenceImage.guid];
        existingObject.transform.position = trackedImage.transform.position;
        existingObject.transform.rotation = trackedImage.transform.rotation;
    }
}
