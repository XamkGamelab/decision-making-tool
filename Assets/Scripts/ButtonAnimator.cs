using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

namespace JAS.MediDeci
{
    [RequireComponent(typeof(Button))]
    public class ButtonPopAnimator : MonoBehaviour, IPointerClickHandler
    {
        [Header("Visuals")]
        public RectTransform animWrapper;
        public TextMeshProUGUI text;

        [Header("Animation Settings")]
        public float animScale = 1.1f;
        public float animDuration = 0.1f;

        private Button button;

        private Vector3 originalScale;
        private bool isAnimating = false;

        private void Awake()
        {
            button = GetComponent<Button>();

            if (animWrapper == null)
            {
                return;
            }

            originalScale = animWrapper.localScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (button.interactable && !isAnimating && animWrapper != null)
            {
                StartCoroutine(DoPopAnimation());
            }
        }

        private IEnumerator DoPopAnimation()
        {
            isAnimating = true;

            Vector3 targetScale = originalScale * animScale;
            float t = 0f;

            // Scale up
            while (t < animDuration)
            {
                t += Time.unscaledDeltaTime;
                float p = t / animDuration;
                animWrapper.localScale = Vector3.Lerp(originalScale, targetScale, p);

                if (text != null)
                    text.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, p);

                yield return null;
            }

            t = 0f;

            // Scale back
            while (t < animDuration)
            {
                t += Time.unscaledDeltaTime;
                float p = t / animDuration;
                animWrapper.localScale = Vector3.Lerp(targetScale, originalScale, p);

                if (text != null)
                    text.rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, p);

                yield return null;
            }

            animWrapper.localScale = originalScale;

            if (text != null)
                text.rectTransform.localScale = originalScale;

            isAnimating = false;
        }
    }
}