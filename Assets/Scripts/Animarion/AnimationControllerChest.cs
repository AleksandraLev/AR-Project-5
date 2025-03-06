using UnityEngine;

public class AnimationControllerChest : MonoBehaviour
{
    private Animator animator;
    private int animationIndex = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) // Для мобильных
        {
            animationIndex = (animationIndex + 1) % 2; // Переключение между 0 и 1
            animator.SetInteger("AnimationIndex", animationIndex);
        }*/
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
                animationIndex = (animationIndex + 1) % 2; // Переключение анимации
                animator.SetInteger("AnimationIndex", animationIndex);
            }
        }
    }
}
