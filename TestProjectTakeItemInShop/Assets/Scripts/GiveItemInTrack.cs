using UnityEngine;

public class GiveItemInTrack : MonoBehaviour
{
    [SerializeField] GameObject winMenu;                    // ��������� ����� ��� ����
    short countfruts = 0;                                   //����������� ��������� ������� ( ���� ������� ���, �� ����� �������� ���� ��������)

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PickupItem>() != null)       //��������� ���� �� � ���������� ������ PickupItem
        {
            Destroy(other.gameObject);                      //������� ������
            countfruts++;                                   //�������
            if (countfruts == 4)
            {
                Win();
            }
        }
    }
    private void Win()                                      //����� ��� ��������� ������� � ��������� ����� ����
    {
        Time.timeScale = 0;
        winMenu.SetActive(true);
    }
}
