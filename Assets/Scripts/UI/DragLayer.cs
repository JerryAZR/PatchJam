using System.Collections;
using UnityEngine;

public class DragLayer : MonoBehaviour
{
    public static DragLayer Instance { get; private set; }

    [SerializeField] private GameObject _hintBoxUI;
     
    public Texture2D cursorSprite;

    public Texture2D cursorClickSprite2;

    void Awake()
    {
        Instance = this;
        ContainerBase.OnTransferReject += ShowHintBoxUI;
        Cursor.SetCursor(cursorSprite, Vector2.zero, CursorMode.Auto);
    }

    void OnDestroy()
    {
        ContainerBase.OnTransferReject -= ShowHintBoxUI;
    }

    void ShowHintBoxUI()
    {
        _hintBoxUI.SetActive(true);
        _hintBoxUI.GetComponent<CanvasGroup>().alpha = 1f;
        //LeanTween.alphaCanvas(_hintBoxUI.GetComponent<CanvasGroup>(), 0f, 1f).setDelay(2f).setOnComplete(() => _hintBoxUI.SetActive(false));
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        CanvasGroup canvasGroup = _hintBoxUI.GetComponent<CanvasGroup>();

        yield return new WaitForSeconds(0.5f);

        float duration = 1f;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        _hintBoxUI.SetActive(false);
    }
}