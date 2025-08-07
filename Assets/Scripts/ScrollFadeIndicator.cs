using UnityEngine;
using UnityEngine.UI;

public class ScrollFadeIndicator : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject fadeTop;
    public GameObject fadeBottom;

    private void Update()
    {
        if (scrollRect == null || scrollRect.content == null)
            return;

        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float scrollPos = scrollRect.verticalNormalizedPosition;

        bool canScroll = contentHeight > viewportHeight;

        fadeTop.SetActive(canScroll && scrollPos < 0.98f);
        fadeBottom.SetActive(canScroll && scrollPos > 0.02f);
    }
}
