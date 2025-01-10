using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private InterfaceReference<IDestroyableEntity> displayingEntityHealth;
    [SerializeField] private SpriteRendererProgressBar progressBar;

    [SerializeField] private bool showed;

    private Camera cam;
    //TODO: init this from global init sys
    private void Start()
    {
        IDestroyableEntity destroyableEntity = displayingEntityHealth.Reference;
        destroyableEntity.vitality.onHealthChange += HealthChanged;
        destroyableEntity.onDie += HideBar;

        progressBar.SetProgress(destroyableEntity.vitality.hpPercent);
        if (showed) progressBar.Show(true);
        else progressBar.Hide(true);

        cam = Camera.main;
    }

    private void Update()
    {
        if (showed)
        {
            transform.LookAt(cam.transform.position);
        }
    }

    public void HealthChanged(float percent)
    {
        if(percent >= 1) HideBar();
        else ShowBar();
        progressBar.SetProgress(percent);
    }

    public void HideBar()
    {
        if (!showed) return;
        showed = false;
        progressBar.Hide();
    }

    public void ShowBar()
    {
        if (showed) return;
        showed = true;
        progressBar.Show();
    }

    [System.Serializable]
    private class SpriteRendererProgressBar
    {
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private SpriteRenderer backgroundRenderer;
        [SerializeField] private float fullWidth = 5.8f;

        private const float showHideDur = 0.5f;

        public void SetProgress(float percent)
        {
            Vector2 size = renderer.size;
            size.x = Mathf.Lerp(0, fullWidth, percent);
            renderer.size = size;
        }

        public void Show(bool permanent = false)
        {
            SpriteRenderer[] renderers = new SpriteRenderer[] { renderer, backgroundRenderer };
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.DOKill();
                if (!permanent) renderer.DOFade(1, showHideDur);
                else SetAlphaOfRenderer(renderer, 1);
            }
        }

        public void Hide(bool permanent = false)
        {
            SpriteRenderer[] renderers = new SpriteRenderer[] { renderer, backgroundRenderer };
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.DOKill();
                if (!permanent) renderer.DOFade(0, showHideDur);
                else SetAlphaOfRenderer(renderer, 0);
            }
        }

        private void SetAlphaOfRenderer(SpriteRenderer renderer, float alpha)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }

    }
}
