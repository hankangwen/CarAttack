using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WeaponController : MonoBehaviour
{
    [SerializeField]private Camera _mainCamera;
    [SerializeField] private ContolsType controls = ContolsType.Mobile;
    [SerializeField] private float rotationSpeed = 5f;
    private List<Gun> controlledGuns = new List<Gun>();
    private LayerMask targetLayer;

    private bool isDragging;
    private Vector2 lastTouchPosition;
    public void RegisterGun(Gun gun) => controlledGuns.Add(gun);
    public void UnregisterGun(Gun gun) => controlledGuns.Remove(gun);

    private void Start()
    {
        targetLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        if(controls == ContolsType.PC)
        {
            Vector3 screenPoint = Input.mousePosition;
            Ray ray = _mainCamera.ScreenPointToRay(screenPoint);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, targetLayer))
            {
                Vector3 hitPoint = hit.point;
                foreach (Gun gun in controlledGuns)
                {
                    gun.LoockAt(hitPoint, Time.deltaTime);
                }
            }
        }
        else if (controls == ContolsType.Mobile)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouseInput();//for testing
#else
            HandleTouchInput();
#endif
        }

    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 currentInputPosition = Input.mousePosition;
            Vector2 inputDelta = currentInputPosition - lastTouchPosition;

            float rotationAmount = inputDelta.x * rotationSpeed * Time.deltaTime;
            foreach (Gun gun in controlledGuns)
            {
                gun.Rotate(0, rotationAmount, 0);
            }

            lastTouchPosition = currentInputPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isDragging = true;
                    lastTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector2 touchDelta = touch.position - lastTouchPosition;
                        float rotationAmount = touchDelta.x * rotationSpeed * Time.deltaTime;
                        foreach (Gun gun in controlledGuns)
                        {
                            gun.Rotate(0, rotationAmount, 0);
                        }

                        lastTouchPosition = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
    }
    public enum ContolsType { PC, Mobile}
}
