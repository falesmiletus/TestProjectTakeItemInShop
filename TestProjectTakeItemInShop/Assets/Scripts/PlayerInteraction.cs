using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Transform holdPosition;                                  // ������� � �����
    public Button actionButton;                                     // ������ ��������������(����� � ��������)

    private PickupItem currentItem = null;                          // ������� ������� � �����
    private PickupItem nearbyItem = null;                           // ������� � ��������

    void Start()
    {
        actionButton.onClick.AddListener(OnActionButtonPressed);    //�������� �� ������� ������� ������
    }

    void OnActionButtonPressed()                                    //����� ��� �������
    {
        if (currentItem == null && nearbyItem != null)              //�������� �� ��, ��� ������� �� � ����� � ����� ���� �������
        {
            nearbyItem.PickUp(holdPosition);                        //��������� � 
            currentItem = nearbyItem;                               //������������� ���
        }
        else if (currentItem != null)                               //���� ��� ����
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
