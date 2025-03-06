using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class PlaceOnPlane : PressInputBase
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;

        public GameObject spawnedObject { get; set; }
        bool m_Pressed;

        //public GameObject spawnedObject { get; private set; }

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set
            {
                m_PlacedPrefab = value;

                // Когда меняем префаб, нужно создать новый объект на месте старого, если он есть
                if (spawnedObject != null)
                {
                    Destroy(spawnedObject); // Удаляем старый объект
                }

                // Создаем новый объект на текущем месте
                if (m_RaycastManager != null && m_RaycastManager.Raycast(Pointer.current.position.ReadValue(), s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = s_Hits[0].pose;
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    // Получаем позицию камеры
                    Vector3 cameraPosition = Camera.main.transform.position;

                    // Вычисляем направление к камере, игнорируя высоту (Y)
                    Vector3 directionToCamera = new Vector3(cameraPosition.x - hitPose.position.x, 0, cameraPosition.z - hitPose.position.z);

                    // Разворачиваем объект к камере (только по Y)
                    spawnedObject.transform.rotation = Quaternion.LookRotation(directionToCamera);
                }
            }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>

        protected override void Awake()
        {
            base.Awake();
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }

        void Update()
        {

            if (Pointer.current == null || m_Pressed == false || EventSystem.current.IsPointerOverGameObject())
                return;

            var touchPosition = Pointer.current.position.ReadValue();

            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                }
                else
                {
                    spawnedObject.transform.position = hitPose.position;
                }
                // Получаем позицию камеры
                Vector3 cameraPosition = Camera.main.transform.position;

                // Вычисляем направление к камере, игнорируя высоту (Y)
                Vector3 directionToCamera = new Vector3(cameraPosition.x - hitPose.position.x, 0, cameraPosition.z - hitPose.position.z);

                // Разворачиваем объект к камере (только по Y)
                spawnedObject.transform.rotation = Quaternion.LookRotation(directionToCamera);
            }
        }

        public GameObject GetSpawnedObject() => spawnedObject;

        protected override void OnPress(Vector3 position) => m_Pressed = true;

        protected override void OnPressCancel() => m_Pressed = false;

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;
    }
}