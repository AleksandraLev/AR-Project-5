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
    [SerializeField] private Button fixButton;  // ������ "�������������"
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject trackedImagePrefab;

    private Dictionary<Guid, GameObject> _spawnedObjects = new Dictionary<Guid, GameObject>();
    private HashSet<Guid> _fixedObjects = new HashSet<Guid>(); // ������ ��������������� �������
    private GameObject _selectedObject = null; // ��������� ������

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

        // ���������, ������������ �� ������
        if (_fixedObjects.Contains(imageGuid))
        {
            Debug.Log($"������ {imageGuid} ������������ � �� �����������.");
            text.text = $"������ {imageGuid} ������������ � �� �����������.";
            return; // ���� ������ ������������, �� ��������� ��� �������
        }

        //var lockable = trackedImage.GetComponent<LockableTrackedImage>();

        //if (lockable == null)
        //{
        //    lockable = trackedImage.gameObject.AddComponent<LockableTrackedImage>();
        //}

        //if (lockable.IsLocked)
        //{
        //    Debug.Log($"������ {imageGuid} ������������ � �� �����������.");
        //    text.text = $"������ {imageGuid} ������������ � �� �����������.";
        //    return;
        //}

        // ������� ����� ������, ������� ��� ��� ������ AR Foundation (����� trackedImagePrefab)
        //if (_spawnedObjects.ContainsKey(imageGuid))
        //{
        //    var existingObject = _spawnedObjects[imageGuid];
        //    existingObject.transform.position = trackedImage.transform.position;
        //    existingObject.transform.rotation = trackedImage.transform.rotation;
        //}
        //else
        //{
        //    // ���� ������ �� ���������� � ������ _spawnedObjects, ������ �� ��� ������ AR Foundation
        //    var existingObject = Instantiate(trackedImagePrefab);

        //    // ��������� ��� � ��� ������ ������������� ��������
        //    _spawnedObjects[imageGuid] = existingObject;

        //    // ��������� ������� � ��������
        //    existingObject.transform.position = trackedImage.transform.position;
        //    existingObject.transform.rotation = trackedImage.transform.rotation;
        //}

        // ���������, ���� �� ��� ������ ��� ����� �����������
        if (!_spawnedObjects.ContainsKey(trackedImage.referenceImage.guid))
        {
            // ������� ������, ���� ��� ��� ���
            var spawnedObject = Instantiate(trackedImagePrefab);
            _spawnedObjects[trackedImage.referenceImage.guid] = spawnedObject;
        }

        // ��������� ������� � ���������� �������
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
                //Debug.Log("�������");
               // text.text = "�������";
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectObject(Input.mousePosition);
                //Debug.Log("���������");
                //text.text = "���������";
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
                Debug.Log($"������ ������: {_selectedObject.name}");
                text.text = $"������ ������: {_selectedObject.name}";
            }
        }
    }

    public void ToggleFixation()
    {
        //Debug.Log($"����� ToggleFixation");
        text.text = "����� ToggleFixation";
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
                Debug.Log($"������ {key} �������");
                //text.text = $"������ {key} �������";
                text.text = $"������ {_selectedObject.name} �������";
            }
            else
            {
                _fixedObjects.Add(key);
                Debug.Log($"������ {key} ������������");
                //text.text = $"������ {key} ������������";
                text.text = $"������ {_selectedObject.name} ������������";
            }
        }

        //if (_selectedObject == null) return;

        //var lockable = _selectedObject.GetComponent<LockableTrackedImage>();

        //if (lockable != null)
        //{
        //    if (lockable.IsLocked)
        //    {
        //        lockable.IsLocked = false;
        //        text.text = "������ �������";

        //    }
        //    else
        //    {
        //        lockable.IsLocked = true;
        //        text.text = "������ ������������";
        //    }
        //    //lockable.IsLocked = !lockable.IsLocked;
        //    //text.text = lockable.IsLocked ? "������ ������������" : "������ �������";
        //    Debug.Log(text.text);
        //}
        //else
        //{
        //    text.text = "lockable != null";
        //}
    }
}
