using UnityEngine;

public class AnimationControllerRabbit : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Для ПК
        {
            CheckForObjectClick(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // Для мобильных
        {
            CheckForObjectClick(Input.GetTouch(0).position);
        }
    }
    void CheckForObjectClick(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition); // Создаем луч из точки касания
        if (Physics.Raycast(ray, out RaycastHit hit)) // Проверяем, попал ли луч в коллайдер
        {
            if (hit.collider.gameObject == gameObject) // Проверяем, что это именно наш объект
            {
                animator.SetTrigger("PlayDanceAnimation");
            }
        }
    }
}
