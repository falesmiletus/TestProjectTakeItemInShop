using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public float throwForce = 10f;                              // ���� �����

    private Rigidbody itemRigidbody;
    private Collider itemCollider;
    private bool inHand = false;                                // ������ ���� �� � ���� ������

    private void Start()
    {
        itemRigidbody = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
    }

    public void PickUp(Transform holdPosition)
    {
        if (inHand) return;                                     // ���� ���������� �� �������

        inHand = true;                                          //� ����
        itemRigidbody.isKinematic = true;                       //��������� ��� ���������� ������
        itemCollider.enabled = false;                           //����� ���������

        transform.SetParent(holdPosition);                      //�������� � ���� � ��������� ��� ������� � ��������
        transform.localPosition = Vector3.zero; 
        transform.localRotation = Quaternion.identity;
    }

    public void Throw()
    {
        if (!inHand) return;                                    //���� �� � ���� - �������

        inHand = false;                                         //�� � ����
        transform.SetParent(null);                              //������� ��� ��������
        itemRigidbody.isKinematic = false;                      //���������� ���������
        itemCollider.enabled = true;                            //��������� ��������

        itemRigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);  //������ ������� �� ���
    }
}
