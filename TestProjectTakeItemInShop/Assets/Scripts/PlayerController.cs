using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 3f;
    public float deadZone = 0.2f; // Мертвая зона джойстика

    [Header("Ссылки на объекты")]
    public CharacterController characterController;
    public Camera playerCamera;

    [Header("Сенсорные элементы управления")]
    public Joystick movementJoystick; // Джойстик для передвижения
    public RectTransform touchArea; // Область для управления камерой

    private Vector2 lastTouchPosition; // Последняя позиция касания
    private bool isTouching = false; // Идет ли свайп
    private bool isUsingJoystick = false; // Используется ли джойстик в данный момент

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

        // Получаем ввод с джойстика
        float horizontal = movementJoystick.Horizontal;
        float vertical = movementJoystick.Vertical;

        // Проверяем, превышает ли он мертвую зону
        if (Mathf.Abs(horizontal) < deadZone) horizontal = 0;
        if (Mathf.Abs(vertical) < deadZone) vertical = 0;

        // Если после фильтрации ничего не осталось — не двигаемся
        if (horizontal == 0 && vertical == 0) return;

        // Направление движения
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection.y = 0; // Оставляем персонажа на одной высоте

        // Применяем движение
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        // Если используется джойстик, не даем камере поворачиваться
        if (isUsingJoystick) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Проверяем, попал ли палец в область сенсорного управления
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

                    // Поворачиваем персонажа влево/вправо
                    transform.Rotate(Vector3.up * delta.x * rotationSpeed * Time.deltaTime);

                    // Наклоняем камеру вверх/вниз (ограничиваем угол)
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
