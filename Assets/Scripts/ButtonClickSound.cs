using JAS.MediDeci;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayButtonClick();
        });
    }
}