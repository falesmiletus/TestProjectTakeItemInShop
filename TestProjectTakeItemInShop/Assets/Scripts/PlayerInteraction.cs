using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Transform holdPosition;                                  // Позиция в руках
    public Button actionButton;                                     // Кнопка взаимодействия(взять и выкинуть)

    private PickupItem currentItem = null;                          // Текущий предмет в руках
    private PickupItem nearbyItem = null;                           // Предмет в триггере

    void Start()
    {
        actionButton.onClick.AddListener(OnActionButtonPressed);    //подписка на событие нажатие кнопки
    }

    void OnActionButtonPressed()                                    //метод для нажатия
    {
        if (currentItem == null && nearbyItem != null)              //проверка на то, что предмет не в руках и рядом есть предмет
        {
            nearbyItem.PickUp(holdPosition);                        //подбираем и 
            currentItem = nearbyItem;                               //устанавливаем его
        }
        else if (currentItem != null)                               //если уже есть
        {
            currentItem.Throw();
            currentItem = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item != null && currentItem == null)
        {
            nearbyItem = item;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupItem item = other.GetComponent<PickupItem>();
        if (item != null && nearbyItem == item)
        {
            nearbyItem = null;
        }
    }
}
