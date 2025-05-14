using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace JAS.MediDeci
{

    public class ButtonUi : MonoBehaviour
    {
        public float startTime;
        public float clickTime;
        public float resultTime;

        public int RandomScene;


        public TextMeshProUGUI resultText;

        public GameObject AnswerButtonsVisible;
        public GameObject BackToMenuButtonVisible;


        private void Start()
        {
            startTime = Time.time;
            AnswerButtonsVisible.SetActive(true);
            BackToMenuButtonVisible.SetActive(false);
            RandomScene = Random.Range(1, 4); // T‰ss‰ on vain scenet 1-3
            Debug.Log("Random scene: " + RandomScene);
        }

        // T‰m‰ on vain ajan testailua varten, poistetaan varmaan lopuksi
        public void BackToMenu()
        {
            SceneManager.LoadScene(0); // Main Menu
        }


        // Buttons 1-4 on Main Game Sceness‰, Play game, how to play ja quit game napit on main menussa
        // Klikkaa nappulaa ja se laskee kuluneen ajan ja antaa tulosruudun, josta voi palata menuun
        // T‰ss‰ vaiheessa button1 on aina se oikea vastaus. Sen paikkaa voi muuttaa sceness‰, mutta pit‰k‰‰ viel‰ se button 1 oikeana vastauksena
        public void Clicked1()
        {
            Debug.Log("You clicked button 1");
            clickTime = Time.time;
            resultTime = clickTime - startTime;
            Debug.Log("Your reactiontime is: " + resultTime);
            AnswerButtonsVisible.SetActive(false);
            BackToMenuButtonVisible.SetActive(true);
            resultText.text = "Olit oikeassa. Reaktioaikasi oli: " + resultTime + " sekuntia";
            //BackToMenu();
        }
        public void Clicked2()
        {
            Debug.Log("You clicked button 2");
            clickTime = Time.time;
            resultTime = clickTime - startTime;
            Debug.Log("Your reactiontime is: " + resultTime);
            AnswerButtonsVisible.SetActive(false);
            BackToMenuButtonVisible.SetActive(true);
            resultText.text = "Olit v‰‰r‰ss‰. Reaktioaikasi oli: " + resultTime + " sekuntia";
            //BackToMenu();
        }
        public void Clicked3()
        {
            Debug.Log("You clicked button 3");
            clickTime = Time.time;
            resultTime = clickTime - startTime;
            Debug.Log("Your reactiontime is: " + resultTime);
            AnswerButtonsVisible.SetActive(false);
            BackToMenuButtonVisible.SetActive(true);
            resultText.text = "Olit v‰‰r‰ss‰. Reaktioaikasi oli: " + resultTime + " sekuntia";
            //BackToMenu();
        }
        public void Clicked4()
        {
            Debug.Log("You clicked button 4");
            clickTime = Time.time;
            resultTime = clickTime - startTime;
            Debug.Log("Your reactiontime is: " + resultTime);
            AnswerButtonsVisible.SetActive(false);
            BackToMenuButtonVisible.SetActive(true);
            resultText.text = "Olit v‰‰r‰ss‰. Reaktioaikasi oli: " + resultTime + " sekuntia";
            //BackToMenu();
        }

        // Aloita peli, lataa main game scene
        public void PlayGame1Button()
        {
            AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
            SceneManager.LoadScene(RandomScene);
        }

        public void PlayGame2Button()
        {
            AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
            SceneManager.LoadScene(4); // Game 2
        }

        public void PlayGame3Button()
        {
            AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
            SceneManager.LoadScene(5); // Testi / Game 3
        }

        // Sulje peli nappia painamalla
        public void QuitQameButton()
        {
            AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
            Debug.Log("You have quit the game");
            Application.Quit();
        }

    }
}
