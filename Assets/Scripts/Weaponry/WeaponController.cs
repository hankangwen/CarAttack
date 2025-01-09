using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]private Camera _mainCamera;
    private List<Gun> controlledGuns = new List<Gun>();
    private LayerMask targetLayer;
    public void RegisterGun(Gun gun) => controlledGuns.Add(gun);
    public void UnregisterGun(Gun gun) => controlledGuns.Remove(gun);

    private void Start()
    {
        targetLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
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
}
