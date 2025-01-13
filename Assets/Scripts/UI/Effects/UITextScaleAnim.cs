using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UITextScaleAnim : MonoBehaviour
{
    [SerializeField] private float animDuration = 2;
    [SerializeField] private float scaleChange = 1.1f;

    private new RectTransform transform;
    private Vector3 defaultScale;

    private void Start()
    {
        this.transform = base.transform as RectTransform;
        defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        float halfAnimDur = animDuration / 2f;
         
        transform.DOScale(defaultScale * scaleChange, halfAnimDur)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnDisable()
    {
        transform.DOKill();
        transform.localScale = defaultScale;
    }
}
