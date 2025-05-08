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
            SceneManager.LoadScene("Main Menu");
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
            resultText.text = "You were correct. Your reactiontime was: " + resultTime + " seconds";
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
            resultText.text = "You were incorrect. Your reactiontime was: " + resultTime + " seconds";
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
            resultText.text = "You were incorrect. Your reactiontime was: " + resultTime + " seconds";
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
            resultText.text = "You were incorrect. Your reactiontime was: " + resultTime + " seconds";
            //BackToMenu();
        }

        // Aloita peli, lataa main game scene
        public void PlayGameButton()
        {
            SceneManager.LoadScene(RandomScene);
        }

        // Sulje peli nappia painamalla
        public void QuitQameButton()
        {
            Debug.Log("You have quit the game");
            Application.Quit();
        }

    }
}
