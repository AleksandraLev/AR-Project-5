using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTargetController : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    private GameObject _spawnedObject;

    void OnEnable()
    {
        _aRTrackedImageManager.trackedImagesChanged += ArTrackedImageManagerOntrakedImagesChanged;
    }

    void OnDisable()
    {
        _aRTrackedImageManager.trackedImagesChanged -= ArTrackedImageManagerOntrakedImagesChanged;
    }

    private void ArTrackedImageManagerOntrakedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (var addedImage in obj.added)
        {
            print(message: $"Added image: {addedImage.referenceImage.name}");
            UpdatePrefab(addedImage.transform);
        }
        foreach (var updatedImage in obj.updated)
        {
            print(message: $"Updated image: {updatedImage.referenceImage.name}");
            UpdatePrefab(updatedImage.transform);
        }
    }

    private void UpdatePrefab(Transform trackedImage)
    {
        if (_spawnedObject == null)
        {
            _spawnedObject = Instantiate(_aRTrackedImageManager.trackedImagePrefab);
        }

        _spawnedObject.transform.position = trackedImage.position;
    }
}
