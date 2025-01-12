using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DamageRecieveEffect : MonoBehaviour, IOrderedInitializable
{
    [SerializeField] private InterfaceReference<IDestroyableEntity> displayingEntityHealth;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color hurtEffectColor;
    [SerializeField] private float hurtScaleChange = 0.9f;
    [SerializeField] private float effectDuration;

    private Color defaultColor;
    private Vector3 defaultScale;
    public string initOrderId => new InitOrderByType(InitOrder.Other);

    [Inject]
    private void Construct(GlobalInitSystems initSys)
    {
        initSys.order.AddToInitOrder(this);
    }

    public void PerformInit(InitArgs initializeArgs)
    {
        defaultScale = transform.localScale;
        defaultColor = meshRenderer.material.color;
        displayingEntityHealth.Reference.vitality.onRecieveDamage += DamageRecieved;
    }

    private void DamageRecieved(float damage)
    {
        meshRenderer.material.DOKill();
        meshRenderer.material.DOColor(hurtEffectColor, effectDuration).OnComplete(() =>
        meshRenderer.material.DOColor(defaultColor, effectDuration));
        meshRenderer.transform.DOKill();
        meshRenderer.transform.DOScale(defaultScale * hurtScaleChange,effectDuration).OnComplete(() =>
        meshRenderer.transform.DOScale(defaultScale, effectDuration)); 
    }
}
