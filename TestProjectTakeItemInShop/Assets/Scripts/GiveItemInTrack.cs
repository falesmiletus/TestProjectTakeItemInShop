using UnityEngine;

public class GiveItemInTrack : MonoBehaviour
{
    [SerializeField] GameObject winMenu;                    // обьявляем юайку для меню
    short countfruts = 0;                                   //колличество собранных фруктов ( если соберет все, то будем вызывать меню выигрыша)

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PickupItem>() != null)       //проверяем есль ли у коллайдера скрипт PickupItem
        {
            Destroy(other.gameObject);                      //удаляем обьект
            countfruts++;                                   //плюсуем
            if (countfruts == 4)
            {
                Win();
            }
        }
    }
    private void Win()                                      //метод для остановки времени и включения юайки меню
    {
        Time.timeScale = 0;
        winMenu.SetActive(true);
    }
}
