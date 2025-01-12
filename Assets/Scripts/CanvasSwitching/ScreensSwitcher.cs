using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScreensSwitcher : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private List<AwailableCanvas> awailableScreens;

    #region swithing main
    private CanvasGroup currentCanvas;
    private Coroutine switchCanvasRoutine;
    public void SwitchScreen(UIScreeen screen)
    {
        if (switchCanvasRoutine != null) throw new System.Exception("CanvasSwitcher.SwitchCanvas cant" +
            " run more than one operation. Wait for current operation to end");
        AwailableCanvas canvasToSwith = awailableScreens.FirstOrDefault(canvas => canvas.canvasType == screen);
        if (canvasToSwith == null) throw new System.Exception($"ScreensSwitcher.SwitchScreen screen \"{screen}\" not found");
        switchCanvasRoutine = StartCoroutine(SwitchCoroutine(canvasToSwith));
    }

    private IEnumerator SwitchCoroutine(AwailableCanvas canvasToSwith)
    {
        CanvasGroup nextCanvasGroupModule = canvasToSwith.canvasGroup;
        nextCanvasGroupModule.interactable = false;
        nextCanvasGroupModule.blocksRaycasts = false;
        if (currentCanvas != null)
        {
            currentCanvas.interactable = false;
            currentCanvas.blocksRaycasts = false;
            yield return FadeCanvasGroup(currentCanvas, 1f, 0f);
            currentCanvas.alpha = 0f;
        }
        if(nextCanvasGroupModule.gameObject.activeSelf == false) nextCanvasGroupModule.gameObject.SetActive(true);
        yield return FadeCanvasGroup(nextCanvasGroupModule, 0f, 1f);
        nextCanvasGroupModule.interactable = true;
        nextCanvasGroupModule.blocksRaycasts = true;
        switchCanvasRoutine = null;
        currentCanvas = nextCanvasGroupModule;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
    #endregion

    public void OpenInOverlay(UIScreeen screen)
    {

    }

    [System.Serializable]
    private class AwailableCanvas
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private UIScreeen _canvasType;
        public CanvasGroup canvasGroup { get { return _canvasGroup; } }
        public UIScreeen canvasType { get { return _canvasType; } }

        public void TurnOff()
        {
            canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}

public enum UIScreeen
{
    MainMenu,
    GameRunning,
    WinWindow,
    LooseWindow
}