using UnityEngine;
using UnityEngine.SceneManagement;

namespace JAS.MediDeci
{
    
    public class ButtonUi : MonoBehaviour
    {
        public float startTime;
        public float clickTime;
        public float resultTime;

        private void Start()
        {
            startTime = Time.time;
        }

        // T‰m‰ on vain ajan testailua varten, poistetaan varmaan lopuksi
        public void BackToMenu() 
        {
            SceneManager.LoadScene("Main Menu");
        }


        // Buttons 1-4 on Main Game Sceness‰, Play game, how to play ja quit game napit on main menussa
        // Klikkaa nappulaa ja se laskee kuluneen ajan ja heitt‰‰ main menuun
        public void Clicked1() 
        {
            Debug.Log("You clicked button 1");
            clickTime = Time.time;
            resultTime = clickTime - startTime;
            Debug.Log("Your reactiontime is: " + resultTime);
            BackToMenu();
        }
        public void Clicked2()
        {
            Debug.Log("You clicked button 2");
            clickTime = Time.time;
            resultTime = clickTime - startTime;
            Debug.Log("Your reactiontime is: " + resultTime);
            BackToMenu();
        }
        public void Clicked3()
        {
            Debug.Log("You clicked button 3");
            clickTime = Time.time;
            resultTime = clickTime - startTime;
            Debug.Log("Your reactiontime is: " + resultTime);
            BackToMenu();
        }
        public void Clicked4()
        {
            Debug.Log("You clicked button 4");
            clickTime = Time.time;
            resultTime = clickTime - startTime;
            Debug.Log("Your reactiontime is: " + resultTime);
            BackToMenu();
        }

        // Aloita peli, lataa main game scene
        public void PlayGameButton()
        {
            SceneManager.LoadScene("Main game");
        }

        // Sulje peli nappia painamalla
        public void QuitQameButton()
        {
            Debug.Log("You have quit the game");
            Application.Quit();
        }

    }
}
