using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("��������� ��������")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 3f;
    public float deadZone = 0.2f; // ������� ���� ���������

    [Header("������ �� �������")]
    public CharacterController characterController;
    public Camera playerCamera;

    [Header("��������� �������� ����������")]
    public Joystick movementJoystick; // �������� ��� ������������
    public RectTransform touchArea; // ������� ��� ���������� �������

    private Vector2 lastTouchPosition; // ��������� ������� �������
    private bool isTouching = false; // ���� �� �����
    private bool isUsingJoystick = false; // ������������ �� �������� � ������ ������

    private float rotationX = 0f;

    void Update()
    {
        HandleMovement();
        HandleCameraRotation();
    }

    void HandleMovement()
    {
        // ���������, ������������ �� ������ ��������
        if (movementJoystick.Horizontal != 0 || movementJoystick.Vertical != 0)
        {
            isUsingJoystick = true;
        }
        else
        {
            isUsingJoystick = false;
        }

        // �������� ���� � ���������
        float horizontal = movementJoystick.Horizontal;
        float vertical = movementJoystick.Vertical;

        // ���������, ��������� �� �� ������� ����
        if (Mathf.Abs(horizontal) < deadZone) horizontal = 0;
        if (Mathf.Abs(vertical) < deadZone) vertical = 0;

        // ���� ����� ���������� ������ �� �������� � �� ���������
        if (horizontal == 0 && vertical == 0) return;

        // ����������� ��������
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection.y = 0; // ��������� ��������� �� ����� ������

        // ��������� ��������
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        // ���� ������������ ��������, �� ���� ������ ��������������
        if (isUsingJoystick) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // ���������, ����� �� ����� � ������� ���������� ����������
            if (RectTransformUtility.RectangleContainsScreenPoint(touchArea, touch.position))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    isTouching = true;
                    lastTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved && isTouching)
                {
                    Vector2 delta = touch.position - lastTouchPosition;
                    lastTouchPosition = touch.position;

                    // ������������ ��������� �����/������
                    transform.Rotate(Vector3.up * delta.x * rotationSpeed * Time.deltaTime);

                    // ��������� ������ �����/���� (������������ ����)
                    rotationX -= delta.y * rotationSpeed * Time.deltaTime;
                    rotationX = Mathf.Clamp(rotationX, -80f, 80f);
                    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isTouching = false;
                }
            }
        }
    }
}
