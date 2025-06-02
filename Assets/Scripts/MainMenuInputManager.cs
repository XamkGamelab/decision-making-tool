using TMPro;
using UnityEngine;

public class MainMenuInputManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField yearInput;

    private void Start()
    {
        // Lataa tallennetut arvot PlayerPrefsistä, jos niitä on
        if (PlayerPrefs.HasKey("username"))
            usernameInput.text = PlayerPrefs.GetString("username");

        if (PlayerPrefs.HasKey("year"))
            yearInput.text = PlayerPrefs.GetString("year");
    }

    public void OnUsernameChanged()
    {
        PlayerPrefs.SetString("username", usernameInput.text);
        PlayerPrefs.Save();
    }

    public void OnYearChanged()
    {
        PlayerPrefs.SetString("year", yearInput.text);
        PlayerPrefs.Save();
    }
}