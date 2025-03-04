using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 3f;
    public float deadZone = 0.2f; 

    public CharacterController characterController;
    public Camera playerCamera;

    public Joystick movementJoystick;                   // Джойстик для передвижения
    public RectTransform touchArea;                     // Область для управления камерой

    private Vector2 lastTouchPosition;                  // Последняя позиция касания
    private bool isTouching = false;                    // Идет ли свайп
    private bool isUsingJoystick = false;               // Используется ли джойстик в данный момент

    private float rotationX = 0f;

    void Update()
    {
        HandleMovement();
        HandleCameraRotation();
    }

    void HandleMovement()
    {
                                                        // Проверяем, используется ли сейчас джойстик
        if (movementJoystick.Horizontal != 0 || movementJoystick.Vertical != 0)
        {
            isUsingJoystick = true;
        }
        else
        {
            isUsingJoystick = false;
        }

        float horizontal = movementJoystick.Horizontal; // Получаем ввод с джойстика
        float vertical = movementJoystick.Vertical;

                                                        // Проверяем, превышает ли он мертвую зону
        if (Mathf.Abs(horizontal) < deadZone) horizontal = 0;
        if (Mathf.Abs(vertical) < deadZone) vertical = 0;

                                                    // Если после фильтрации ничего не осталось — не двигаемся
        if (horizontal == 0 && vertical == 0) return;

        // Направление движения
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection.y = 0; 

        // Применяем движение
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        if (isUsingJoystick) return;            // Если используется джойстик, не даем камере поворачиваться

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

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

                    transform.Rotate(Vector3.up * delta.x * rotationSpeed * Time.deltaTime);

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
