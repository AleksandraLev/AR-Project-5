using UnityEngine;
using UnityEngine.UI;

public class ScrollViewList : MonoBehaviour
{
    public GameObject buttonPrefab;  // Префаб кнопки
    public Transform content;  // Контейнер для кнопок (Content)
    public Sprite[] buttonImages;  // Массив с изображениями для кнопок

    void Start()
    {
        // Создаем кнопки на основе массива изображений
        foreach (Sprite img in buttonImages)
        {
            CreateButton(img);
        }
    }

    // Метод для создания кнопки
    void CreateButton(Sprite buttonImage)
    {
        // Создаем кнопку
        GameObject newButton = Instantiate(buttonPrefab, content);

        // Получаем компонент Image для кнопки и устанавливаем картинку
        Image buttonImg = newButton.GetComponent<Image>();
        buttonImg.sprite = buttonImage;

        // Получаем компонент Button и добавляем обработчик события
        Button button = newButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClick(buttonImage));
    }

    // Метод, который вызывается при нажатии на кнопку
    void OnButtonClick(Sprite clickedImage)
    {
        Debug.Log("Выбрана кнопка с изображением: " + clickedImage.name);
    }
}
