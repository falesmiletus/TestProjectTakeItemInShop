using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public float throwForce = 10f;                              // сила кидка

    private Rigidbody itemRigidbody;
    private Collider itemCollider;
    private bool inHand = false;                                // чекаем есть ли в руке обьект

    private void Start()
    {
        itemRigidbody = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
    }

    public void PickUp(Transform holdPosition)
    {
        if (inHand) return;                                     // если удерживает то выходим

        inHand = true;                                          //в руке
        itemRigidbody.isKinematic = true;                       //кениматик дл€ отключени€ физики
        itemCollider.enabled = false;                           //офаем коллайдер

        transform.SetParent(holdPosition);                      //дочерний к руке и изменени€ его позиции и вращение
        transform.localPosition = Vector3.zero; 
        transform.localRotation = Quaternion.identity;
    }

    public void Throw()
    {
        if (!inHand) return;                                    //если не в руке - выходим

        inHand = false;                                         //не в руке
        transform.SetParent(null);                              //убираем как дочерний
        itemRigidbody.isKinematic = false;                      //кинематику отключаем
        itemCollider.enabled = true;                            //коллайдер включаем

        itemRigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);  //делаем импульс по зет
    }
}
