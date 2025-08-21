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
                GlobalCoroutineRunner.Instance.RunCoroutine(DoPopAnimation(animWrapper, text, animScale, animDuration));
            }
        }

        private IEnumerator DoPopAnimation(RectTransform wrapper, TextMeshProUGUI label, float scale, float duration)
        {
            isAnimating = true;

            Vector3 original = wrapper.localScale;
            Vector3 target = original * scale;
            float t = 0f;

            // Scale up
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                float p = t / duration;
                wrapper.localScale = Vector3.Lerp(original, target, p);

                if (label != null)
                    label.rectTransform.localScale = Vector3.Lerp(original, target, p);

                yield return null;
            }

            t = 0f;

            // Scale down
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                float p = t / duration;
                wrapper.localScale = Vector3.Lerp(target, original, p);

                if (label != null)
                    label.rectTransform.localScale = Vector3.Lerp(target, original, p);

                yield return null;
            }

            wrapper.localScale = original;

            if (label != null)
                label.rectTransform.localScale = original;

            isAnimating = false;
        }
    }
}