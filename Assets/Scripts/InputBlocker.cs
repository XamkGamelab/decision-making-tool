using UnityEngine;
using UnityEngine.UI;

public class InputBlocker : MonoBehaviour
{
    // Jotta poistuessa pelist‰ klikkaus ei avaa main menun nappulaa ne pit‰‰ disabloida hetkeksi


    public Button menuButton1; // Peli 1 button
    public Button menuButton2; // Peli 2 button
    public Button menuButton3; // Asetukset button
    public Button menuButton4; // Ohjeet button
    public Button menuButton5; // Lopeta peli button
    public Button menuButton6; // Peli 3 button

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
        menuButton6.interactable = false;
        Invoke(nameof(EnableButton), 0.1f); // wait 100 ms
    }

    void EnableButton()
    {
        menuButton1.interactable = true;
        menuButton2.interactable = true;
        menuButton3.interactable = true;
        menuButton4.interactable = true;
        menuButton5.interactable = true;
        menuButton6.interactable = true;
    }
}