using UnityEngine;
using UnityEngine.UI;

public class InputBlocker : MonoBehaviour
{
    // Jotta poistuessa pelist‰ klikkaus ei avaa main menun nappulaa ne pit‰‰ disabloida hetkeksi


    public Button menuButton1;
    public Button menuButton2;
    public Button menuButton3;
    public Button menuButton4;
    public Button menuButton5;

    void Start()
    {
        menuButton1.interactable = false;
        Invoke(nameof(EnableButton), 0.1f); // wait 100 ms
        menuButton2.interactable = false;
        Invoke(nameof(EnableButton), 0.1f); // wait 100 ms
        menuButton3.interactable = false;
        Invoke(nameof(EnableButton), 0.1f); // wait 100 ms
        menuButton4.interactable = false;
        Invoke(nameof(EnableButton), 0.1f); // wait 100 ms
        menuButton5.interactable = false;
        Invoke(nameof(EnableButton), 0.1f); // wait 100 ms
    }

    void EnableButton()
    {
        menuButton1.interactable = true;
        menuButton2.interactable = true;
        menuButton3.interactable = true;
        menuButton4.interactable = true;
        menuButton5.interactable = true;
    }
}